cfn-create-or-update --region us-east-1 --template-body file://yaml/acm.yaml --stack-name autoreporto-certificatemanager --parameters ParameterKey=DomainName,ParameterValue=autoreporto.com
# confirm via emails
# check at https://ap-southeast-2.console.aws.amazon.com/acm/home?region=ap-southeast-2#/