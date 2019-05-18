using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

using CloudHealth;

namespace CloudHealth.Tests
{
    public class Result {
        public Result(int Index, string Name, float Value ) {
            this.Index = Index;
            this.Name = Name;
            this.Value = Value;
        }
        public int Index { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }        
    }

    public class TestCloudHealthService
    {

        private readonly ITestOutputHelper output;
  
        public static readonly CultureInfo american = 
            CultureInfo.GetCultureInfo("en-US");

        public TestCloudHealthService(ITestOutputHelper output)
        {
            this.output = output;
        }


        [Fact]
        public void TestCloudHealthServiceAPI()
        {
            // set up by loading in json from file
            string inputFile = @"/Users/stefanevans/Development/reporter/services/cloudhealth/test/CloudHealthTestInputJsonFile.json";

            StreamReader streamReader = File.OpenText(inputFile);

            JsonSerializer serializer = new JsonSerializer();
            JsonTextReader serializerText = new JsonTextReader(streamReader);
            dynamic input = serializer.Deserialize(serializerText);

            // find correct index for time.
            int index = 0;
            int selectedIndex = -1;
            IList timeArray = input["dimensions"][0]["time"];
            foreach (dynamic timeItem in timeArray) {
                string name = (string)timeItem["name"];
                //output.WriteLine((string)timeItem["name"]);
                if (name == "2019-04") {
                    selectedIndex = index;
                }
                index++;
            }
            if (selectedIndex == -1) {
                // throw error as month can not be found.
                throw new Exception("Month not found");
            }
            output.WriteLine("Selected index is " + selectedIndex);

            // get services
            IList serviceList = input["dimensions"][1]["AWS-Service-Category"];
            // TODO: Fix this:
            string[] serviceArray = new string[200];
            index = 0;
            foreach (dynamic serviceItem in serviceList) {
                serviceArray[index] = (string)serviceItem["label"];
                index++;
                //output.WriteLine((string)serviceItem["name"]);
                //output.WriteLine((string)serviceItem["label"]);
            }

            // blacklists servers
            ArrayList blackList = new ArrayList();
            
            blackList.Add("Total");
            blackList.Add("Amazon Elastic Compute Cloud - Direct");

            // get values
            index = 0;
            IList selectedIndexList = input["data"][selectedIndex];
            IList<Result> results = new List<Result>();
            foreach (dynamic valueItem in selectedIndexList) {
                string service = serviceArray[index];
                string value = (string)valueItem[0];
                //output.WriteLine(service + " " + value);

                if (value == null) {
                     value = "0";
                }

                if (!blackList.Contains(service)) {
                    results.Add(new Result(index, service, float.Parse(value)));
                }

                index++;
            }

            // sort results
            IEnumerable<Result> sortedEnum = results.OrderByDescending(f=>f.Value);
            IList<Result> sortedList = sortedEnum.ToList();

            foreach (Result result in sortedList) {
                output.WriteLine(result.Name);
            }

            var cultureInfo = CultureInfo.CurrentCulture;   // You can also hardcode the culture, e.g. var cultureInfo = new CultureInfo("fr-FR"), but then you lose culture-specific formatting such as decimal point (. or ,) or the position of the currency symbol (before or after)
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "$";

            foreach (Result result in sortedList) {
                output.WriteLine(result.Value.ToString("C", numberFormatInfo));
            }            








            var cloudHealthService = new CloudHealthService();

        }
    }
}
