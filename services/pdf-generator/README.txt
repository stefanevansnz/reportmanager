https://github.com/isummation/lambda-pdf-generator

TO INSTALL:
npm install

TO RUN:
sam local start-api --skip-pull-image --profile reporter



TO TEST LOCALLY:
http://127.0.0.1:3000/generatepdf?bucket=reporterbot-reportbucket&key=template/index.html&pdflocation=odm_report1_january2019.pdf&htmlbuilderhost=https://oz4bxxr9tj.execute-api.ap-southeast-2.amazonaws.com/Prod&servicehost=eqjp1asntl.execute-api.ap-southeast-2.amazonaws.com/Prod&statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/&title=Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-01-01-00-00-00&edate=2019-02-01-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=
 

TO TEST PRODUCTION:
https://924m2xxzz3.execute-api.ap-southeast-2.amazonaws.com/Prod/generatepdf?bucket=reporterbot-reportbucket&key=template/index.html&pdflocation=odm_report1_january2019.pdf&htmlbuilderhost=https://oz4bxxr9tj.execute-api.ap-southeast-2.amazonaws.com/Prod&servicehost=eqjp1asntl.execute-api.ap-southeast-2.amazonaws.com/Prod&statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/&title=Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-01-01-00-00-00&edate=2019-02-01-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=



NOTE

scale html inside iframe:
http://jsfiddle.net/AwokeKnowing/yy8yb/

https://github.com/alixaxel/chrome-aws-lambda

https://github.com/sambaiz/puppeteer-lambda-starter-kit
https://github.com/GoogleChrome/puppeteer

TRY WITH:
 https://try-puppeteer.appspot.com/

