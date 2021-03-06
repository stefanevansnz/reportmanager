# htmlbuilder

This is a sample template for htmlbuilder

TO INSTALL:
dotnet publish -c Release

TO RUN:
sam local start-api --skip-pull-image --profile reporter

TO TEST LOCAL:
http://127.0.0.1:3000/htmlbuilder?bucket=reporterbot-reportbucket&key=template/index.html&servicehost=wjz58f95q9.execute-api.ap-southeast-2.amazonaws.com/Prod&statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/&title=Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-01-01-00-00-00&edate=2019-02-01-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=

TO TEST PRODUCTION
https://oz4bxxr9tj.execute-api.ap-southeast-2.amazonaws.com/Prod/htmlbuilder?bucket=reporterbot-reportbucket&key=template/index.html&servicehost=eqjp1asntl.execute-api.ap-southeast-2.amazonaws.com/Prod&statichost=https://s3.amazonaws.com/reporterbot-reportbucket/template/&title=Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-01-01-00-00-00&edate=2019-02-01-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=



https://s3.amazonaws.com/reporterbot-reportbucket/template/index.html




## Requirements

* AWS CLI already configured with Administrator permission
* [Docker installed](https://www.docker.com/community-edition)
* [SAM Local installed](https://github.com/awslabs/aws-sam-cli)
* [DotNet Core installed](https://www.microsoft.com/net/download)

## Setup process

### Linux & macOS

```bash
sh build.sh --target=Package
```

### Windows (Powershell)

```powershell
build.ps1 --target=Package
```

### Local development

**Invoking function locally through local API Gateway**

```bash
sam local start-api
```

**SAM Local** is used to emulate both Lambda and API Gateway locally and uses our `template.yaml` to understand how to bootstrap this environment (runtime, where the source code is, etc.) - The following excerpt is what the CLI will read in order to initialize an API and its routes:

```yaml
...
Events:
    HelloWorldFunction:
        Type: Api # More info about API Event Source: https://github.com/awslabs/serverless-application-model/blob/master/versions/2016-10-31.md#api
        Properties:
            Path: /hello
            Method: get
```


If the previous command run successfully you should now be able to hit the following local endpoint to invoke your function `http://localhost:3000/hello`

## Packaging and deployment

AWS Lambda C# runtime requires a flat folder with all dependencies including the application. SAM will use `CodeUri` property to know where to look up for both application and dependencies:

```yaml
...
    HelloWorldFunction:
        Type: AWS::Serverless::Function
        Properties:
            CodeUri: artifacts/HelloWorld.zip            
            ...
```

First and foremost, we need an `S3 bucket` where we can upload our Lambda functions packaged as ZIP before we deploy anything - If you don't have a S3 bucket to store code artifacts then this is a good time to create one:

```bash
aws s3 mb s3://BUCKET_NAME
```

Next, run the following command to package our Lambda function to S3:

```bash
sam package \
    --template-file template.yaml \
    --output-template-file packaged.yaml \
    --s3-bucket REPLACE_THIS_WITH_YOUR_S3_BUCKET_NAME
```

Next, the following command will create a Cloudformation Stack and deploy your SAM resources.

```bash
sam deploy \
    --template-file packaged.yaml \
    --stack-name htmlbuilder \
    --capabilities CAPABILITY_IAM
```

> **See [Serverless Application Model (SAM) HOWTO Guide](https://github.com/awslabs/serverless-application-model/blob/master/HOWTO.md) for more details in how to get started.**

After deployment is complete you can run the following command to retrieve the API Gateway Endpoint URL:

```bash
aws cloudformation describe-stacks \
    --stack-name htmlbuilder \
    --query 'Stacks[].Outputs'
```
## Testing

For testing our code, we use XUnit and you can use `dotnet test` to run tests defined under `test/`

```bash
dotnet test test/HelloWorld.Test
```

Alternatively, you can use Cake. It discovers and executes all the tests.

### Linux & macOS

```bash
sh build.sh --target=Test
```

### Windows (Powershell)

```powershell
build.ps1 --target=Test
```

# Appendix

## AWS CLI commands

AWS CLI commands to package, deploy and describe outputs defined within the cloudformation stack:

```bash
aws cloudformation package \
    --template-file template.yaml \
    --output-template-file packaged.yaml \
    --s3-bucket REPLACE_THIS_WITH_YOUR_S3_BUCKET_NAME

aws cloudformation deploy \
    --template-file packaged.yaml \
    --stack-name htmlbuilder \
    --capabilities CAPABILITY_IAM \
    --parameter-overrides MyParameterSample=MySampleValue

aws cloudformation describe-stacks \
    --stack-name htmlbuilder --query 'Stacks[].Outputs'
```

## Bringing to the next level

Here are a few ideas that you can use to get more acquainted as to how this overall process works:

* Create an additional API resource (e.g. /hello/{proxy+}) and return the name requested through this new path
* Update unit test to capture that
* Package & Deploy

Next, you can use the following resources to know more about beyond hello world samples and how others structure their Serverless applications:

* [AWS Serverless Application Repository](https://aws.amazon.com/serverless/serverlessrepo/)
