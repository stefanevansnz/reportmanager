using System;
using Xunit;
using PRTGService.Service;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace TestService
{
    public class TestSummaryCalculator
    {
        SummaryCalculator summaryCalculator;

        public TestSummaryCalculator() {
            summaryCalculator = new SummaryCalculator();
        }

        [Fact]
        public void TestExtractFloatValue()  {
       
            Console.WriteLine("testExtractFloatValue");

            string testInput = "0.07 kbit/s";
            float testResult = summaryCalculator.ExtractFloatValue(testInput);
            Assert.Equal(0.07F, testResult);

            testInput = "< 0.01 kbit/s";
            testResult = summaryCalculator.ExtractFloatValue(testInput);
            Assert.Equal(0.01F, testResult);

        }
    }
}
    