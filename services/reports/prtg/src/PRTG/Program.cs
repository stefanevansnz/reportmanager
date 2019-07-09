using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

using System.Xml.Linq;
using PRTGService.Service;
using System.IO;

using Amazon.XRay.Recorder.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PRTG
{

    public class Function
    {

        private static string tableName = "reporterbot-sam-build-ReportTable-6AWRKNVHJGSG";

        private static AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();

        private static readonly HttpClient httpClient = new HttpClient();


        private T TraceFunction<T>(Func<T> func, string subSegmentName)
        {
            AWSXRayRecorder.Instance.BeginSubsegment(subSegmentName);
            T result = func();
            AWSXRayRecorder.Instance.EndSubsegment();

            return result;
        } 

        private static async Task<string> GetCallingIP()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var stringTask = httpClient.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext:false);

            var msg = await stringTask;
            return msg.Replace("\n","");
        }

        private static async Task<Stream> GetPRTGSummary(IDictionary<string, string> requestParams) {

            string serviceUri = "https://" + requestParams["host"] +
                "/api/historicdata.xml" +
                "?id=" + requestParams["id"] +            
                "&avg=3600&pctavg=300&pctshow=false&pct=95&pctmode=false&clgid={D6BCEC62-05AD-4CE2-A08A-650E9DAEC7B8}" +
                "&sdate=" + requestParams["sdate"] + 
                "&edate=" + requestParams["edate"] + 
                "&username=" + requestParams["username"] +
                "&passhash=" + requestParams["passhash"];
            Console.WriteLine(serviceUri);

            //var client = new HttpClient();
            var response = await httpClient.GetAsync(serviceUri);

            if (!response.IsSuccessStatusCode) {
                throw new Exception("Service Request not successful " + response.StatusCode);
            } else {
                Console.WriteLine("Got successfull result");
            }

            var stream = await response.Content.ReadAsStreamAsync();


            return stream;
        }

        private async Task<IDictionary<string, string>> GetItemFromDynamoDB() 
        {
            IDictionary<string, string> requestParams = new Dictionary<string, string>();
 
            Table reports = Table.LoadTable(dynamoDBClient, tableName);
            await RetrieveItem(reports);

            return requestParams;

        }

        private async static Task RetrieveItem(Table reportsTable)
        {
            Table reports = Table.LoadTable(dynamoDBClient, tableName);
            Console.WriteLine("Executing RetrieveItem()");
            // Optional configuration.
            GetItemOperationConfig config = new GetItemOperationConfig
            {
                AttributesToGet = new List<string> { "userid", "id" },
                ConsistentRead = true
            };
            Document document = await reportsTable.GetItemAsync("111", config);
            string id = document["id"].AsString();
            Console.WriteLine("Found id which is " + id);
        }        


        public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try {
                //var queryStrings = apigProxyEvent.QueryStringParameters["sdate"];
                Console.WriteLine("apigProxyEvent count " + request.QueryStringParameters.Count());

                IDictionary<string, string> requestParams = new Dictionary<string, string>();
                if (request.QueryStringParameters != null) {
                    var queryStringParameters = request.QueryStringParameters;
                    foreach (var item in queryStringParameters) {
                        Console.WriteLine($"QueryStringParameter - " + item.Key + ":" + item.Value);
                        requestParams[item.Key] = item.Value;
                    }
                }
                requestParams["username"] = Uri.EscapeDataString(requestParams["username"]);

                // get parameters from dynamodb
                await GetItemFromDynamoDB();


                // load xml into XElement
                XElement dataFromPRTG = 
                    XElement.Load(GetPRTGSummary(requestParams).Result);
                IEnumerable<XElement> itemsList = 
                    from item in dataFromPRTG.Elements() select item;

                //var xrayTrace = TraceFunction(summaryCalculator.TestXRay ,"TestXRay");
                SummaryReportMaker reportMaker = new SummaryReportMaker();
                string output = reportMaker.MakeReport(itemsList, requestParams);

                Console.WriteLine("Return results");
                return new APIGatewayProxyResponse
                {
                    //Body = JsonConvert.SerializeObject(body),
                    Body = output,
                    StatusCode = 200,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/html" } }
                };


            } catch (Exception e) {
                Console.WriteLine("Exception: " +  e.Message );
                // catch error
                Dictionary<string, string> body = new Dictionary<string, string>
                {
                    { "status", "error" },
                    { "message", e.Message },
                    { "stacktrace", e.StackTrace },
                };

                return new APIGatewayProxyResponse
                {
                    Body = JsonConvert.SerializeObject(body),
                    StatusCode = 500,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }
    }
}
