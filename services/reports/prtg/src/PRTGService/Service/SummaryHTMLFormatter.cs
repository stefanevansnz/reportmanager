using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace PRTGService.Service
{
    public class SummaryHTMLFormatter
    {
        string formatedHTML;


        public string GetFormattedHTML() {
            return formatedHTML;
        }

        private string LoadSummary( SummaryModelHolder holder, 
                                    IDictionary<string, string> requestParams) {
            
            //string title = "Report for ODM Production (i-0c240a85cce740b9e) [EC2] - PRTG Network Monitor";
            //string timeRange = "01/01/2019 12:00:00 AM - 01/02/2019 12:00:00 AM";
            //string metrics = "Amazon CloudWatch EC2 BETA (15 m Interval)";
            string timeRange = requestParams["sdate"] + " - " + requestParams["edate"];

            string result = 
                "<h2>" + requestParams["title"] + "</h2>\n" +                
                "<table class=\"table\">\n" +  
                "\t<tbody>\n" +
                "\t\t<tr>\n" +
                "\t\t\t<th>Report Time Span:</th>\n" + 
                "\t\t\t<td colspan=\"6\">" + timeRange + "</td>\n" + 
                "\t\t</tr>\n" +
                "\t\t<tr>\n" +
                "\t\t\t<th>Sensor Type:</th>\n" + 
                "\t\t\t<td colspan=\"6\">" + requestParams["subtitle"] + "</td>\n" + 
                "\t\t</tr>\n" +
                "\t\t<tr>\n" +
                "\t\t\t<th class=\"title\">Uptime Stats:</th>\n" + 
                "\t\t\t<td>Up:</td>\n<td class=\"rightalign\">100 %<div class=\"colorflag colorflag-ok\">&nbsp;</div></td>\n" + 
                "\t\t\t<td>Down:</td>\n<td class=\"rightalign\">0 %<div class=\"colorflag colorflag-ok\">&nbsp;</div></td>\n" + 
                "\t\t</tr>\n" +
                "\t</tbody>\n" +
                "</table>\n";                            
            return result;
        }

        private string LoadImage(IDictionary<string, string> requestParams) {

                string imageUrl = "https://" + requestParams["host"] +
                    "/chart.svg?" +
                    "graphid=-1" +
                    "&id=" + requestParams["id"] +            
                    "&avg=3600&clgid={D6BCEC62-05AD-4CE2-A08A-650E9DAEC7B8}&width=1250&height=470&graphstyling=baseFontSize=%2714%27%20showLegend=%271%27%20tooltexts=%271%27" +
                    "&sdate=" + requestParams["sdate"] + 
                    "&edate=" + requestParams["edate"] + 
                    "&username=" + requestParams["username"] +
                    "&passhash=" + requestParams["passhash"];
                Console.WriteLine(imageUrl);

            var result = 
                "<img class=\"main-img\" src=\"" + imageUrl + "\">\n";
            return result;
        }

        private string LoadAverages(SummaryModelHolder holder) {
            var result = 
                "<table class=\"table\">\n" +                  
                "\t<tbody>\n" +
                "\t\t<tr>\n" +
                "\t\t\t<th>CPU Utilization</th>\n" +
                "\t\t\t<th>Network In</th>\n" +
                "\t\t\t<th>Network Out</th>\n" +
                "\t\t\t<th>Read Ops</th>\n" +
                "\t\t\t<th>Write Ops</th>\n" +      
                "\t\t\t<th>Disk Read</th>\n" +
                "\t\t\t<th>Disk Write</th>\n" + 
                "\t\t\t<th>CPU Credit Usage</th>\n" +
                "\t\t\t<th>CPU Credit Balance</th>\n" +    
                "\t\t\t<th>Status (Ok)</th>\n" +  
                "\t\t\t<th>Status (Instance) (Ok)</th>\n" +  
                "\t\t\t<th>Status (System) (Ok)</th>\n" +                                                              
                "\t\t\t<th>Downtime</th>\n" +  
                "\t\t</tr>\n" +
                "\t\t<tr>\n" +
                "\t\t\t<td>" + holder._dataSummary["CPU Utilization"] + " %</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["Network In"] + " kbit/s</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["Network Out"] + " kbit/s</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["Read Ops"] + " #/s</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["Write Ops"] + " #/s</td>\n" +  
                "\t\t\t<td>" + holder._dataSummary["Disk Read"] + " Mbit/s</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["Disk Write"] + " Mbit/s</td>\n" +    
                "\t\t\t<td>" + holder._dataSummary["CPU Credit Usage"] + " #</td>\n" +
                "\t\t\t<td>" + holder._dataSummary["CPU Credit Balance"] + " #</td>\n" +                                                              
                "\t\t\t<td>" + holder._dataSummary["Status (Ok)"] + " %</td>\n" + 
                "\t\t\t<td>" + holder._dataSummary["Status (Instance) (Ok)"] + " %</td>\n" +                     
                "\t\t\t<td>" + holder._dataSummary["Status (System) (Ok)"] + " %</td>\n" + 
                "\t\t\t<td>" + holder._dataSummary["Downtime"] + " %</td>\n" + 
                "\t\t</tr>\n" +
                "\t</tbody>\n" +
                "</table>\n";                                                          
            return result;
            
        }

        public void LoadTemplates(  SummaryModelHolder holder, 
                                    IDictionary<string, string> requestParams ) {

            var result = 
                "<div replace=\"prtg-summary\" class=\"table-responsive\">\n" +
                LoadSummary(holder, requestParams) +
                "</div>\n" +            
                "<div replace=\"prtg-img\">\n" +
                LoadImage(requestParams) +
                "</div>\n" +
                "<div replace=\"prtg-stats\" class=\"table-responsive\">\n" +
                LoadAverages(holder) +
                "</div>";   

            formatedHTML = result;
        }
    }
}