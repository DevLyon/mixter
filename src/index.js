var express = require('express');
var http = require('http');
var bodyParser = require('body-parser');

var createExpressMiddleware = function(port){
    var app = express();
    app.set('port', port);

    app.use(bodyParser.json());
    app.use(bodyParser.urlencoded({
        extended: false
    }));

    return app;
};

var registerRoutes = function(app){
    app.get('/api/ping', function(req, res){
        res.status(200).send('pong');
    });
};

var startServer = function(app){
    var server = http.createServer(app);
    server.listen(app.get('port'), function(){
        console.log('Express server listening on port ' + app.get('port'));
    });
};

exports.run = function run(port){
    var app = createExpressMiddleware(port);

    registerRoutes(app);

    startServer(app);
};
