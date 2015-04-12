var message = require('../domain/core/message');

var MessageRepository = function MessagesRepository(eventsStore){
    var getAllEvents = function getAllEvents(messageId){
        return eventsStore.getEventsOfAggregate(messageId);
    };

    this.getMessage = function getMessage(messageId){
        var events = getAllEvents(messageId);
        return message.create(events);
    };

    this.getDescription = function getDescription(messageId){
        var events = getAllEvents(messageId);
        var messageQuacked = events[0];

        return new message.MessageDescription(messageQuacked.author, messageQuacked.content);
    };
};

exports.create = function create(eventsStore){
    return new MessageRepository(eventsStore);
};