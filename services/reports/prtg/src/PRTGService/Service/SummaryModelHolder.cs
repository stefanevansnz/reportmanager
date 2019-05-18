using System;
using System.Collections.Generic;

namespace PRTGService.Service
{
    public class SummaryModelHolder
    {
        // private setter so no-one can change the dictionary itself
        // so create it in the constructor
        public IDictionary<string, List<float>> _rawDataHolder { get; private set; }
        public IDictionary<string, string> _dataSummary { get; private set; }

        public SummaryModelHolder()
        {
            _rawDataHolder = new Dictionary<string, List<float>>();
            _dataSummary = new Dictionary<string, string>();
        }

        public List<string> GetListOfSummaryTitles() {
            List<string> summaryTitles = 
                new List<string>(new string[] 
                    {   
                        "CPU Utilization", 
                        "Network In", 
                        "Network Out",
                        "Read Ops",
                        "Write Ops",
                        "Disk Read",
                        "Disk Write",
                        "CPU Credit Usage",
                        "CPU Credit Balance",
                        "Status (Ok)",   
                        "Status (Instance) (Ok)",   
                        "Status (System) (Ok)",   
                        "Downtime"                                                                                                                                                                                                                                              
                    });
            return summaryTitles;
        }        


    }
}
