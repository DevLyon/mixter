var _ = require('lodash');

var SessionsRepository = function SessionsRepository(){
    var self = this;

    var projections = {};

    self.getUserIdOfSession = function getUserIdOfSession(sessionId){
        var projection = projections[sessionId];
        if(!projection || !projection.isEnabled){
            return null;
        }

        return projection.userId;
    };

    self.save = function save(projection) {
        projections[projection.sessionId] = projection;
    };
};

exports.create = function create(){
    return new SessionsRepository();
};