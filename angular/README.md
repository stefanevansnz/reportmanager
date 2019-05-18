## ReporterBot - an angular and SAM application

## Set up and test a local serverless backend API:

npm install --save amazon-cognito-identity-js
npm install --save serverless-finch
npm install serverless-domain-manager --save-dev
npm install --save moment
npm install --save uuid
npm install npm

2) Run locally
./run_serverless.sh

## Set up and test a local angular front end

1) Install and set up Angular project (required once)
npm install -g @angular/cli
ng new reporterbot
npm install

2) Run locally
./run_angular.sh


How to add components
ng g c Reports

## Other References

** Installing Bootstrap
npm install --save bootstrap

Add        
      "styles": [
        "../node_modules/bootstrap/dist/css/bootstrap.min.css",        
        "styles.css"
      ],
to
.angular-cli.json

Add
        "../node_modules/bootstrap/dist/js/bootstrap.min.js",


## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

./test_serverless.sh

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

./test_e2e.sh



