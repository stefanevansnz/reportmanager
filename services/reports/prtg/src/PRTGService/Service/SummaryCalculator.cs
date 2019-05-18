using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace PRTGService.Service
{
    public class SummaryCalculator
    {
        public static string DoFormat(float myNumber)
        {
            string s = string.Format("{0:0.00}", myNumber);

            if (s.EndsWith("00", StringComparison.CurrentCulture)) {
                return ((int)myNumber).ToString();
            } else {
                return s;
            }
        }


        private string ExtractKey(string input)
        {
            Regex regex = new Regex(@"value channel=\""(.*)\""");
            Match match = regex.Match(input);
            if (match.Success) {
                return match.Groups[1].Value;
            } else {
                return null;
            }
        }

        public float ExtractFloatValue(string input) {
            input = input.Replace("< ", "");
            //Console.WriteLine("input value to extract is " + input);

            Regex regex = new Regex(@"(.*)\s");

            Match match = regex.Match(input);

            var value = match.Groups[1].Value;
            //Console.WriteLine("value to convert to float is " + value);
            return float.Parse(value);
        }

        private void AddToRawData(SummaryModelHolder holder, string key, float value) {

            var stats = holder._rawDataHolder;

            List<float> categoryListOfValues = new List<float>();
            bool exists = stats.TryGetValue(key, out categoryListOfValues);

            // removed && !sValue.Contains(value)
            if (exists) {
                // category list exists already so just add
                categoryListOfValues.Add(value);
                stats[key] = categoryListOfValues;
            } else if (!exists) {
                // category list does not exist yet so create and add
                categoryListOfValues = categoryListOfValues ?? new List<float>();
                categoryListOfValues.Add(value);
                stats.Add(key, categoryListOfValues);
            }
/*
            if (key == "CPU Utilization") {
                Console.WriteLine("CPU Utilization is " + value +  " exists is " + exists);
            }
 */
        }

        public SummaryModelHolder GetRawDataFromXml(SummaryModelHolder stats, IEnumerable<XElement> itemsList)
        {
            var elementCount = 0;            
            foreach (XElement item in itemsList)
            {
                elementCount++;
                var itemFieldList = from itemAttr in item.Elements() select itemAttr;

                //Console.WriteLine("* item.name " + item.Name);

                foreach (XElement itemField in itemFieldList)
                {
                    //Console.WriteLine("* itemField.Name " + itemField.Name + " value " + itemField.Value);

                    string key = ExtractKey(itemField.Name + " " + itemField.Attribute("channel"));
                    string rawValue = itemField.Value;
                    if (key != null && rawValue != null && rawValue != "") {
                        //Console.WriteLine("Key found is " + key);
                        //Console.WriteLine("rawValue " + rawValue);

                        // get value
                        float floatValue = ExtractFloatValue(rawValue);
                        //Console.WriteLine("floatValue is " + floatValue);
                        AddToRawData(stats, key, floatValue);
                    }


                }

            }
            Console.WriteLine("elementCount is " + elementCount);
            Console.WriteLine("stat count is " + stats._rawDataHolder.Count());
            

            return stats;
        }


        public SummaryModelHolder CalculateSummaryAverageFromRaw(SummaryModelHolder holder, string key) {

            IDictionary<string, List<float>> stats = holder._rawDataHolder;
            IDictionary<string, string> summary = holder._dataSummary;

            List<float> results = stats[key];

            float result = results.Average();
            string averageValue = DoFormat((float)Math.Round(result, 2));

            holder._dataSummary[key] = averageValue;

            return holder;            
        }

        public SummaryModelHolder CalculateAllSummaryAverages(SummaryModelHolder holder) {
            // work out averages
            Console.WriteLine("Get Summary result");
            List<string> summaryTitles = holder.GetListOfSummaryTitles();
            foreach (string title in summaryTitles) {
                holder = CalculateSummaryAverageFromRaw(holder, title);
            }
            return holder;
        }

        public DateTime TestXRay() {
            Console.WriteLine("Testing XRay");
            return DateTime.UtcNow;            
        }

    }        


}
