'use strict';

const AWS = require('aws-sdk');
const uuid = require('uuid');
//const dynamo = new AWS.DynamoDB.DocumentClient();
const dynamo = new AWS.DynamoDB.DocumentClient({
    region: 'ap-southeast-2',
    endpoint: 'dynamodb.ap-southeast-2.amazonaws.com'
  });
const TABLE_NAME = 'ReportsTable';

const response = require('./response-maker');

// API call to create an item
exports.post = async (event) => {

    let object = JSON.parse(event.body);
    console.log('post object.id is ' + object.id);   
            
    // create/update in db
    if (object.id == null) {
        object.id = uuid();
    } 
    
    // add timestamp
    object.timestamp = new Date().toISOString();

    // set user session id
    var authorizer = event.requestContext.authorizer;
    if (authorizer != null) {
        console.log('found authorizer.claims');
        //body.userid = authorizer.claims.sub;
        object.userid = authorizer.claims.sub;
    } else {
        console.log('not found authorizer.claims');
        object.userid = 'admin'; 
    }
    console.log('Put TABLE_NAME is ' + TABLE_NAME + ' object is ' + JSON.stringify(object));   
            

    let params = {
        TableName: TABLE_NAME,
        Item: object
    };
    let data;
    
    try {
        data = await dynamo.put(params).promise();
    }
    catch(error) {
        console.log('Error: ' + error);
        return response.createResponse(500, error);
    }

    console.log('CREATE ITEM SUCCEEDED');
    return response.createResponse(200, '{"id":"' + object.id + '"}');
};

// API call to get items
exports.get = async (event, callback) => {

    let data;
    let params;

    if (event.pathParameters != null) {
        // id passed in.
        let id = event.pathParameters.id;
        params = {
            TableName: TABLE_NAME,
            KeyConditionExpression: "#userid = :userid and #id = :id",
            ExpressionAttributeNames: {"#userid": "userid", "#id": "id"},
            ExpressionAttributeValues: {":userid": "123", ":id": id }
        };
        console.log('get TABLE_NAME is ' + TABLE_NAME + ', where id is ' + id);                
    } else {
        params = {
            TableName: TABLE_NAME,
            ScanIndexForward: true,
            KeyConditionExpression: "#userid = :userid",
            ExpressionAttributeNames: {"#userid": "userid"},
            ExpressionAttributeValues: {":userid": "123" }
        };
        console.log('get TABLE_NAME is ' + TABLE_NAME);        
    }
    
    try {
        data = await dynamo.query(params).promise();
    }
    catch(error) {
        console.log(`GET ALL ITEMS FAILED, WITH ERROR: ${error}`);
        return response.createResponse(500, error);
    }
    
    if (!data.Items) {
        console.log('NO ITEMS FOUND FOR GET ALL API CALL');
        return response.createResponse(404, 'ITEMS NOT FOUND\n');
    }

    console.log('RETRIEVED ALL ITEMS SUCCESSFULLY WITH count = ${data.Count}');
    return response.createResponse(200, JSON.stringify(data.Items) + '\n');
};

// API call to delete an item
exports.delete = async (event) => {

    let object = JSON.parse(event.body);

    // add timestamp
    object.timestamp = new Date().toISOString();

    // set user session id
    //var authorizer = event.requestContext.authorizer;
    //if (authorizer.claims != null) {
        //console.log('found authorizer.claims');
        //body.userid = authorizer.claims.sub;
    //}
    object.userid = '123';    
            
    let keyNameValues = '"id": "' + object.id + '", "userid": "123"';
    console.log('delete from TABLE_NAME is ' + TABLE_NAME + ' key value is ' + keyNameValues);   

    let params = {
        TableName: TABLE_NAME,
        Key: JSON.parse('{' + keyNameValues + '}')
    };
    let data;
    
    try {
        data = await dynamo.delete(params).promise();
    }
    catch(error) {
        console.log('Error: ' + error);
        return response.createResponse(500, error);
    }

    console.log('DELETE ITEM SUCCEEDED');
    return response.createResponse(200, '{"id":"' + object.id + '"}');
};



exports.options = async (event, callback) => {
    return response.createResponse(200, "");   
};
