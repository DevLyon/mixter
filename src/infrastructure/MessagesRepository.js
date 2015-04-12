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
        var creationEvent = events[0];

        if(creationEvent instanceof Message.MessagePublished){
            return new Message.MessageDescription(creationEvent.author, creationEvent.content);
        } else if(creationEvent instanceof Message.ReplyMessagePublished){
            return new Message.MessageDescription(creationEvent.replier, creationEvent.replyContent);
        } else {
            throw new Error('Unknown creation event of message ' + messageId);
        }
    };
};

exports.create = function create(eventsStore){
    return new MessageRepository(eventsStore);
};