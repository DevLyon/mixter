var express = require('express');
var http = require('http');
var bodyParser = require('body-parser');
var routes = require('./routes');

var createExpressMiddleware = function createExpressMiddleware(port){
    var app = express();
    app.set('port', port);

    app.use(bodyParser.json());
    app.use(bodyParser.urlencoded({
        extended: false
    }));

    return app;
};

var startServer = function startServer(app){
    var server = http.createServer(app);
    server.listen(app.get('port'), function(){
        console.log('Express server listening on port ' + app.get('port'));
    });
};

exports.run = function run(port){
    var app = createExpressMiddleware(port);

    routes.registerRoutes(app);

    startServer(app);
};
