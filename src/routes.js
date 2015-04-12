exports.registerRoutes = function registerRoutes(app){
    app.get('/api/ping', function(req, res){
        res.status(200).send('pong');
    });
};
