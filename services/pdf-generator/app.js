'use strict';
const chromium = require('chrome-aws-lambda');
const puppeteer = require('puppeteer-core');
const BUCKET = process.env.S3_BUCKET;
const AWS = require('aws-sdk');

const S3 = new AWS.S3({
  signatureVersion: 'v4',
});

const pdfBuffer = async ( sourceurl, callback) => {
  console.log('start launch');

  try {
    console.log('await puppeteer.launch');
    //process.env['PUPPETEER_SKIP_CHROMIUM_DOWNLOAD'] = true;
    const browser = await puppeteer.launch({
      args: chromium.args,
      //headless: true,
      /*
      args: [
        '--no-sandbox',
        '--disable-setuid-sandbox',
        '--disable-gpu',
        '--hide-scrollbars',
        '--disable-web-security',
      ],*/
      defaultViewport: chromium.defaultViewport,
      executablePath: await chromium.executablePath,
      headless: chromium.headless
    }
    );
    console.log('await newPage');    
    const page = await browser.newPage();
    page.on('requestfailed', (a) => {
        console.log('Failed:', a.url);
      }
    );
    console.log('setDefaultNavigationTimeout to ' + process.env.TIMEOUT);    

    page.setDefaultNavigationTimeout(process.env.TIMEOUT);
    //await page.emulateMedia('screen');  
    console.log('await page.goto');    
    await page.goto(sourceurl, { waitUntil: ['domcontentloaded', 'networkidle0'] });
    
    console.log('await page.pdf');        
    var pdfbuffer = await page.pdf({    
      printBackground: true, 
      displayHeaderFooter: false,
      margin: {
        top: 0,
        right: 0,
        bottom: 0,
        left: 0,
      }
    });
    
    console.log('browser.close'); 
    await browser.close();
    
    return pdfbuffer;
  }
  catch (err) {
    callback(err, { statusCode: 500, body: '{ "message": "Error occured while generating PDF", "error": true }' });
  }
}


const uploadToS3 = async (buffer, bucket, pdflocation, callback) => {
  try {
    if (!buffer) {
      callback(null, { statusCode: 500, body: '{ "message": "No PDF buffer available.", "error": true }' })
      return;
    }
    var object = await S3.putObject({
      Body: buffer,
      Bucket: bucket,
      Key: pdflocation,
    }).promise();
    return object;
  }
  catch (err) {
    callback(err, { statusCode: 500, body: '{ "message": "Error occured while uploading PDF", "error": true }' })
  }
}


exports.handler = async (event, context, callback) => {
  console.log("start");  
  //var params = JSON.parse(event.body);
  var requestParams = event.queryStringParameters;

  //console.log("requestParams.username " + requestParams.username);  
  requestParams.username = encodeURIComponent(requestParams.username);
  //console.log("requestParams.username " + requestParams.username);  


  var serviceUri = requestParams.htmlbuilderhost +
  "/htmlbuilder?" +
  "bucket=" + requestParams.bucket +
  "&key=" + requestParams.key +
  "&servicehost=" + requestParams.servicehost +
  "&statichost=" + requestParams.statichost +
  "&title=" + requestParams.title +            
  "&subtitle=" + requestParams.subtitle + 
  "&host=" + requestParams.host + 
  "&id=" + requestParams.id + 
  "&sdate=" + requestParams.sdate +                 
  "&edate=" + requestParams.edate + 
  "&username=" + requestParams.username +
  "&passhash=" + requestParams.passhash;
  console.log(serviceUri);  

  var pdflocation = requestParams.pdflocation;

  context.callbackWaitsForEmptyEventLoop = false;

  try {
      console.log("getting pdf buffer");
      var buffer = await pdfBuffer(serviceUri, callback);

      console.log("uploadToS3");
      await uploadToS3(buffer, requestParams.bucket, pdflocation, callback);

      console.log("end");  
      callback(null, { body: JSON.stringify({"result":"PDF generated."}) });
  }
  catch (err) {
    console.log("error occured")
    console.log(err);
    callback(err, { statusCode: 500, body: '{ "message":"Error occured while generating pdf.","error":true }' })
  }
  return "done";
}
