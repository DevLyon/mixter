var _ = require('lodash');

var SessionsRepository = function SessionsRepository(){
    var self = this;

    var projections = [];

    self.getUserIdOfSession = function getUserIdOfSession(sessionId){
        var projection = _.find(projections, function(projection) {
            return projection.sessionId === sessionId && projection.isEnabled;
        });

        return projection ? projection.userId : null;
    };

    self.save = function save(projection) {
        projections.push(projection);
    };
};

exports.create = function create(){
    return new SessionsRepository();
};