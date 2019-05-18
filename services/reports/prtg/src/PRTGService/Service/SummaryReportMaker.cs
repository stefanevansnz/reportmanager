using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace PRTGService.Service
{
    public class SummaryReportMaker
    {
        public string MakeReport(IEnumerable<XElement> itemsList, 
                                 IDictionary<string, string> requestParams) {

            SummaryCalculator calculator = new SummaryCalculator();
            SummaryHTMLFormatter formatter = new SummaryHTMLFormatter();
            SummaryModelHolder model = new SummaryModelHolder();

            // get stats from xml
            model = calculator.GetRawDataFromXml(model, itemsList);
            // do calculations and get averages etc
            model = calculator.CalculateAllSummaryAverages(model);
            // generate html
            formatter.LoadTemplates(model, requestParams); 
            
            // return that formatted html
            return formatter.GetFormattedHTML();   
        }


    }
}