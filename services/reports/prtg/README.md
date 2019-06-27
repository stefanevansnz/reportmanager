
### SAM API to build html for PRTG report
cd services/reports/prtg/

## TO BUILD:

dotnet publish -c Release

## TO RUN:

Start Docker
sam local start-api --skip-pull-image --profile reporter

## TO TEST LOCAL:
Open Browser and run:

http://127.0.0.1:3000/prtg?title=Report%20for%20Production%20[EC2]%20-%20PRTG%20Network%20Monitor&subtitle=Amazon%20CloudWatch%20EC2%20BETA%20(15%20m%20Interval)&host=monitoring%2Econsegna%2Ecloud&id=4196&sdate=2019-01-01-00-00-00&edate=2019-02-01-00-00-00&username=stefan%2Eevans%2Bodm%40consegna%2Ecloud&passhash=

## TO RUN CODE PIPELINE
Generate and set github token
https://help.github.com/en/articles/creating-a-personal-access-token-for-the-command-line
export YOUR_GITHUB_TOKEN=
export PIPELINE_NAME=prtg-pipeline

cfn-create-or-update --profile reporter --region ap-southeast-2 --template-body file://codepipeline/codepipeline.yml --stack-name $PIPELINE_NAME --capabilities CAPABILITY_IAM --parameters ParameterKey=PipelineName,ParameterValue=$PIPELINE_NAME ParameterKey=GitHubOAuthToken,ParameterValue=$YOUR_GITHUB_TOKEN


## NOTES

* XRay
dotnet add package AWSXRayRecorder --version 2.4.0-beta