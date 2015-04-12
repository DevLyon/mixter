var userIdentity = require('../domain/identity/userIdentity');

var UnknownUserIdentity = exports.UnknownUserIdentity = function UnknownUserIdentity(userId){
    this.userId = userId;
};

var MessageRepository = function MessagesRepository(eventsStore){
    var getAllEvents = function getAllEvents(userId){
        var events = eventsStore.getEventsOfAggregate(userId);
        if(!events.length){
            throw new UnknownUserIdentity(userId);
        }

        return events;
    };

    this.getUserIdentity = function getUserIdentity(userId){
        var events = getAllEvents(userId);
        return userIdentity.create(events);
    };
};

exports.create = function create(eventsStore){
    return new MessageRepository(eventsStore);
};