using System;
using Xunit;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace HelloWorld.Tests
{
    public class TestHtmlBuilder
    {

        [Fact]
        public void TestBuildHtml()
        {
            // set up by loading in xml from file
             string template = 
                System.IO.File.ReadAllText(@"/Users/stefanevans/Development/reporter/services/htmlbuilder/sam-app/test/HelloWorld.Test/TestService/HTMLTestOutputExpected.html");       

            IDictionary<string, string> requestParams = new Dictionary<string, string>();
            //requestParams["template"] = "monitoring.company.com";
            requestParams["id"] = "111";
            requestParams["sdate"] = "2019-01-01-00-00-00";            
            requestParams["edate"] = "2019-02-01-00-00-00";
            requestParams["username"] = "user%2Ename%2Bcompany%2Ecom";
            requestParams["passhash"] = "1234";
            requestParams["title"] = "Report for Production [EC2] - PRTG Network Monitor";            
            requestParams["subtitle"] = "Amazon CloudWatch EC2 BETA (15 m Interval)";                         

            HtmlBuilder htmlBuilder = new HtmlBuilder();
            string output = htmlBuilder.BuildHtml(template, requestParams);            
            output = output.Replace("\t","");

            string expected = 
                System.IO.File.ReadAllText(@"/Users/stefanevans/Development/reporter/services/htmlbuilder/sam-app/test/HelloWorld.Test/TestService/HTMLTestOutputExpected.html");       

            // check out from report maker is was is expected
            Assert.Equal(expected, output);

        }
    }
}
