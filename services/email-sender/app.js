var aws = require("aws-sdk");
var nodemailer = require("nodemailer");

const S3 = new aws.S3({
    signatureVersion: 'v4',
});

function getS3File(bucket, key) {
    return new Promise(function(resolve, reject) {
        S3.getObject(
            {
                Bucket: bucket,
                Key: key
            },
            function (err, data) {
                if (err) return reject(err);
                else return resolve(data);
            }
        );
    })
}


exports.handler = function (event, context, callback) {
    try {
        console.log("start function");

        aws.config.update({region: 'us-east-1'});
        var ses = new aws.SES();

        getS3File('reporterbot-reportbucket', 'February_2019_Consegna_ODM_Infrastructure_Report.pdf')
        .then(function (fileData) {

            var mailOptions = {
                from: "stefan.evans@consegna.cloud",
                subject: "Test Email",
                html: `<p>You got a contact message from: <b>${event.emailAddress}</b></p>`,
                to: "stefan.evans@consegna.cloud",
                // bcc: Any BCC address you want here in an array
                attachments: [
                    {
                        filename: "February_2019_Consegna_ODM_Infrastructure_Report.pdf",
                        content: fileData.Body
                    }
                ]            
            };
        
            // create Nodemailer SES transporter
            var transporter = nodemailer.createTransport({
                SES: ses
            });
    
            console.log("sending email");
            // send email
            transporter.sendMail(mailOptions, function (err, info) {
                if (err) {
                    console.log("Error sending email");
                    callback(err);
                } else {
                    emailSent = true;
                    console.log("Email sent successfully");
                    var response = {
                        'statusCode': 200,
                        'body': JSON.stringify({
                            message: 'done'
                        })
                    }
                    callback(null, response);
                }
            });


        });


   
    } catch (err) {
        console.log(err);
        return err;
    }

    //return response
};
