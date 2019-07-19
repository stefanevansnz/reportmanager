# https://help.github.com/en/articles/creating-a-personal-access-token-for-the-command-line
export YOUR_GITHUB_TOKEN=
export PIPELINE_NAME=prtg-pipeline
cfn-create-or-update --profile reporter --region ap-southeast-2 --template-body file://codepipeline.yml --stack-name $PIPELINE_NAME --capabilities CAPABILITY_IAM --parameters ParameterKey=PipelineName,ParameterValue=$PIPELINE_NAME ParameterKey=GitHubOAuthToken,ParameterValue=$YOUR_GITHUB_TOKEN