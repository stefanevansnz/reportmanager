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

        public async Task<IDictionary<string, string>> RetrieveReport(string hashKey, string rangeKey)
        {

            Table reportsTable = Table.LoadTable(dynamoDBClient, tableName);

            //string hashKey = "111";
            //string rangeKey = "2";
            Console.WriteLine("hashKey is " + hashKey);
            Console.WriteLine("rangeKey is " + rangeKey);

            Console.WriteLine("Executing RetrieveItem()");
            // Optional configuration.
            GetItemOperationConfig config = new GetItemOperationConfig
            {
                AttributesToGet = new List<string> { "userid", "id" , "title"},
                ConsistentRead = true
            };
            Document document = await reportsTable.GetItemAsync(hashKey, rangeKey, config);
            IDictionary<string, string> result = new Dictionary<string, string>();
            result["id"] = document["id"].AsString();
            result["title"] = document["title"].AsString();            
            Console.WriteLine("Found id which is " + result["id"] + " and title is " + result["title"]);
            return result;            
        }        

    }
}
