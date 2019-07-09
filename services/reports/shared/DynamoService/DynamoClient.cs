using System;

using System.Threading.Tasks;
using System.Collections.Generic;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoService
{
    public class DynamoClient
    {


        private static string tableName = "reporterbot-sam-build-ReportTable-6AWRKNVHJGSG";

        private static AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient();






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

    }
}
