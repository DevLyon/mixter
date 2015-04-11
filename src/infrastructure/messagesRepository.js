var Message = require('../domain/core/message');

var MessageRepository = function MessagesRepository(eventsStore){
    this.getMessage = function(messageId){
        var events = eventsStore.getEventsOfAggregate(messageId);
        return Message.create(events);
    };
};

exports.create = function(eventsStore){
    return new MessageRepository(eventsStore);
};