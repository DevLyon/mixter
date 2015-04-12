var Message = require('../domain/core/Message');

var UnknownMessage = exports.UnknownMessage = function UnknownMessage(messageId){
    this.messageId = messageId;
};

var MessageRepository = function MessagesRepository(eventsStore){
    var getAllEvents = function getAllEvents(messageId){
        var events = eventsStore.getEventsOfAggregate(messageId);
        if(!events.length){
            throw new UnknownMessage(messageId);
        }

        return events;
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