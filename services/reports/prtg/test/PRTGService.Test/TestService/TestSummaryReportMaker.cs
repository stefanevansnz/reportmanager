using System;
using Xunit;
using PRTGService.Service;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace TestService
{
    public class TestSummaryReportMaker
    {

        [Fact]
        public void TestMakeReport()
        {
            // set up by loading in xml from file
            XElement dataFromFile = 
                XElement.Load(@"/Users/stefanevans/Development/reporter/services/prtg/sam-app/test/PRTGService.Test/TestService/PRTGTestInputXmlFile.xml");
            IEnumerable<XElement> itemsList = 
                from item in dataFromFile.Elements() select item;

            IDictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams["host"] = "monitoring.company.com";
            requestParams["id"] = "111";
            requestParams["sdate"] = "2019-01-01-00-00-00";            
            requestParams["edate"] = "2019-02-01-00-00-00";
            requestParams["username"] = "user%2Ename%2Bcompany%2Ecom";
            requestParams["passhash"] = "1234";
            requestParams["title"] = "Report for Production [EC2] - PRTG Network Monitor";            
            requestParams["subtitle"] = "Amazon CloudWatch EC2 BETA (15 m Interval)";                         

            SummaryReportMaker reportMaker = new SummaryReportMaker();
            string output = reportMaker.MakeReport(itemsList, requestParams);            
            output = output.Replace("\t","");
            string expected = 
                System.IO.File.ReadAllText(@"/Users/stefanevans/Development/reporter/services/prtg/sam-app/test/PRTGService.Test/TestService/PRTGTestOutputExpected.html");       

            // check out from report maker is was is expected
            Assert.Equal(expected, output);

        }
    }
}
