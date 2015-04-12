var Message = require('../domain/core/Message');

var MessageRepository = function MessagesRepository(eventsStore){
    var getAllEvents = function getAllEvents(messageId){
        return eventsStore.getEventsOfAggregate(messageId);
    };

    this.getMessage = function getMessage(messageId){
        var events = getAllEvents(messageId);
        return Message.create(events);
    };

    this.getDescription = function getDescription(messageId){
        var events = getAllEvents(messageId);
        var messagePublished = events[0];

        return new Message.MessageDescription(messagePublished.author, messagePublished.content);
    };
};

exports.create = function create(eventsStore){
    return new MessageRepository(eventsStore);
};