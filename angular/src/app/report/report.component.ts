import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Report } from '../reports/reports.model';

import { DataStorageService } from '../shared/data-storage.service';
import { HtmlProxyService } from '../shared/html-proxy.service';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { AuthenticationService } from '../shared/authentication.service';

//import { SanitizeHtmlPipe } from './sanitizehtml.pipe';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  report: Report;
  htmlResult: string;
  reportLoading = true; 
  // html builder url
  url = "https://9ctfc6tfhh.execute-api.ap-southeast-2.amazonaws.com/Prod/htmlbuilder?bucket=reporterbot-reportbucket&key=template/index.html&servicehost=eqjp1asntl.execute-api.ap-southeast-2.amazonaws.com/Prod&statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/&title=March%202019%20Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-03-01-00-00-00&edate=2019-03-31-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=3250151625";
  // pdf generator url

  // email sender url

  private reportId;  

  constructor(
    private dataStorageService: DataStorageService, 
    private htmlProxyService: HtmlProxyService,      
    private route: ActivatedRoute) {}


    ngOnInit() {
      this.route.params
        .subscribe(
          (params: Params) => {
            // get all bookings with this booking id
            let id = params['id'];
            console.log('reportId = ' + id);
            this.reportId = id;
            this.loadReportDetails();
          }
        );
    }

    private loadReportDetails() {
      console.log('loading report object...');
      let self = this; 
      this.dataStorageService.getObjectFromServer('report', this.reportId, self, this.successfullLoadedDetailed);                
    } 

    successfullLoadedDetailed(self) {
      // now load html of report from url and put on page
      self.reportLoading = true;            
      self.htmlProxyService.getHtmlFromServer(self.url, self, self.successfullLoadedReport);
    }

    successfullLoadedReport(self, html) {   
      self.reportLoading = false; 
      console.log(html);  
      
      // remove stuff
      let regExp = new RegExp('<body>([^]+)<\/body>');
      let bodyArray = html.match(regExp);
      let body = "Error Getting Report";

      if (bodyArray.length > 0) {
        body = bodyArray[0];
      } 

      self.htmlResult = body;
    }


    onGenerateEmailPDF(reportId) {
      console.log('Generate PDF and send off Report ' + reportId);

      // call generate PDF

      // call send email
    }


}
