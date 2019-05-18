// API Gateway's Lambda proxy integration requires a
// Lambda function to return JSON in this format;
// see the Developer Guide for further details
exports.createResponse = async (statusCode, body) => {
    
    // to restrict the origin for CORS purposes, replace the wildcard
    // origin with a specific domain name
    return {
        statusCode: statusCode,
        body: body,
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET, POST, DELETE, OPTIONS',
            'Access-Control-Allow-Headers': '*'            
        }
    }
};