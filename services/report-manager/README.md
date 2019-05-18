Backend

TO INSTALL:
cd reporterbot
npm install

TO RUN:
sam local start-api --skip-pull-image --profile reporter

TO TEST:
curl -X PUT -H 'Content-Type: application/json' -d '{"title": "Report Test"}' http://localhost:3000/report

http://localhost:3000/report

PRODUCTION:
curl -X PUT -H 'Content-Type: application/json' -d '{"id": "222", "title": "Report Test2"}' https://prg9r0aujh.execute-api.ap-southeast-2.amazonaws.com/Prod/report


https://prg9r0aujh.execute-api.ap-southeast-2.amazonaws.com/Prod/report


NEXT SET UP dynamodb CREATE AND GET
https://github.com/aws-samples/startup-kit-serverless-workload/blob/master/index.js

DEPLOY

VIEW HTML - Hard code in app for now.
* Builder Host:
https://oz4bxxr9tj.execute-api.ap-southeast-2.amazonaws.com/Prod/htmlbuilder
* PDF Host
https://924m2xxzz3.execute-api.ap-southeast-2.amazonaws.com/Prod/generatepdf

- name=ODM Monthly Report
- title=February%202019%20Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor
- subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)
- pdflocation=February_2019_Consegna_ODM_Infrastructure_Report.pdf
- Frequency: Monthly
sdate=2019-02-01-00-00-00&edate=2019-02-28-00-00-00

- TYPE OF REPORT - DROPDOWN 
* PRTG Service Host:
eqjp1asntl.execute-api.ap-southeast-2.amazonaws.com/Prod
bucket=reporterbot-reportbucket
key=template/index.html
statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/

PRTG config
- host=monitoring%2Econsegna%2Ecloud
- username=stefan%2Eevans%2Bodm%40consegna%2Ecloud
- passhash=xyz
- id=4196 (of sensor)


