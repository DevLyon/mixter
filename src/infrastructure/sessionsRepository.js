var _ = require('lodash');
var session = require('../domain/identity/session');

var UnknownSession = exports.UnknownSession = function UnknownSession(sessionId){
    this.sessionId = sessionId;
};

var SessionsRepository = function SessionsRepository(eventsStore){
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

    var getAllEvents = function getAllEvents(sessionId){
        var events = eventsStore.getEventsOfAggregate(sessionId);
        if(!events.length){
            throw new UnknownSession(sessionId);
        }

        return events;
    };

    self.getSession = function getSession(sessionId){
        var events = getAllEvents(sessionId);
        return session.create(events);
    };
};

exports.create = function create(eventsStore){
    return new SessionsRepository(eventsStore);
};