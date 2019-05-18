using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{

    public class Function
    {

        private const string bucketName = "*** bucket name ***";
        private const string keyName = "*** object key ***";
        // Specify your bucket region (an example region is shown).
        //private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;    
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;


        private string ReadHtmlFromTemplate(IDictionary<string, string> requestParams)
        {
            string bucketName = requestParams["bucket"];
            string templateKey = requestParams["key"];


            Console.WriteLine("Get from S3 " + bucketName + " - " + templateKey);
            try {
                IAmazonS3 client = new AmazonS3Client(bucketRegion);
                Console.WriteLine("Create client");
                var response = client.GetObjectAsync(bucketName, templateKey);
                Console.WriteLine("Got response");
                return new StreamReader(response.Result.ResponseStream).ReadToEnd();
            } catch (Exception exceptionS3) {
                Console.WriteLine("exceptionS3" + exceptionS3.Message);                
                return exceptionS3.Message;
            }


        }

        private string ReplaceHtmlWithAbsoluteLinks(string host, string originalHtml) {
            Console.WriteLine("host is " + host);
            Console.WriteLine("originalHtml is " + originalHtml);
            var baseUri = new Uri(host);
            var pattern = @"(?<name>src|href)=""(?<value>[^""]*)""";
            var matchEvaluator = new MatchEvaluator(
                match =>
                {
                    var value = match.Groups["value"].Value;
                    Uri uri;

                    if (Uri.TryCreate(baseUri, value, out uri))
                    {
                        var name = match.Groups["name"].Value;
                        return string.Format("{0}=\"{1}\"", name, uri.AbsoluteUri);
                    }

                    return null;
                });
            string adjustedHtml = Regex.Replace(originalHtml, pattern, matchEvaluator);
            Console.WriteLine("adjustedHtml is " + adjustedHtml);
            return adjustedHtml;
        }

        private static async Task<Stream> GetPRTGReport(IDictionary<string, string> requestParams) {

            // hit api to get service
            string serviceUri = "https://" + requestParams["servicehost"] +
                "/prtg" +
                "?title=" + requestParams["title"] +            
                "&subtitle=" + requestParams["subtitle"] + 
                "&host=" + requestParams["host"] + 
                "&id=" + requestParams["id"] + 
                "&sdate=" + requestParams["sdate"] +                 
                "&edate=" + requestParams["edate"] + 
                "&username=" + requestParams["username"] +
                "&passhash=" + requestParams["passhash"];
            Console.WriteLine(serviceUri);

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(serviceUri);

            if (!response.IsSuccessStatusCode) {
                throw new Exception("Service Request not successful " + response.StatusCode);
            } else {
                Console.WriteLine("Got successfull result");
            }

            var stream = await response.Content.ReadAsStreamAsync();

            return stream;
        }


        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {

            IDictionary<string, string> requestParams = new Dictionary<string, string>();
            if (request.QueryStringParameters != null) {
                var queryStringParameters = request.QueryStringParameters;
                foreach (var item in queryStringParameters) {
                    Console.WriteLine($"QueryStringParameter - " + item.Key + ":" + item.Value);
                    requestParams[item.Key] = item.Value;
                }
            }     
            requestParams["username"] = Uri.EscapeDataString(requestParams["username"]);

            // get template html from template s3 bucket.
            string template = ReadHtmlFromTemplate(requestParams);            
            Console.WriteLine("Got template from S3");

            // replace href and src with absolute links
            // https://s3.amazonaws.com/reporterbot-reportbucket/template/
            template = ReplaceHtmlWithAbsoluteLinks(requestParams["statichost"], template );
            Console.WriteLine("Replaced HTML with Absolute Links");

            // get report to insert into template
            string responseValue = "";
            var stream = GetPRTGReport(requestParams).Result;
            using (var reader = new StreamReader(stream)) {
                responseValue = reader.ReadToEnd();
            }
            //Console.WriteLine("GetPRTGReport: " + responseValue);            

            // parse it for references (put into a dictionary key and value)
            string output = template.Replace("<!-- INSERT REPORT HERE -->", responseValue);

            Console.WriteLine("Return results");
            return new APIGatewayProxyResponse
            {
                Body = output,
                StatusCode = 200,
                Headers = new Dictionary<string, string> { 
                    { "Content-Type", "text/html" },
                    { "Access-Control-Allow-Origin", "*" }                                
                }
            };


        }
    }
}
