var idGenerator = require('../../idGenerator');
var valueType = require('../../valueType');
var DecisionProjection = require('../DecisionProjection');

var MessageId = exports.MessageId = valueType.extends(function MessageId(id){
    this.id = id;
}, function toString() {
    return 'Message:' + this.id;
});

var MessagePublished = exports.MessagePublished = function MessagePublished(messageId, author, content){
    this.messageId = messageId;
    this.author = author;
    this.content = content;

    Object.freeze(this);
};

MessagePublished.prototype.getAggregateId = function getAggregateId(){
    return this.messageId;
};

var MessageRepublished = exports.MessageRepublished = function MessageRepublished(messageId, republisher){
    this.messageId = messageId;
    this.republisher = republisher;

    Object.freeze(this);
};

MessageRepublished.prototype.getAggregateId = function getAggregateId(){
    return this.messageId;
};

var Message = function Message(events){
    var self = this;

    var projection = DecisionProjection.create().register(MessagePublished, function(event) {
        this.messageId = event.messageId;
        this.author = event.author;
    }).apply(events);

    self.republish = function republish(publishEvent, republisher) {
        if(projection.author.equals(republisher)){
            return;
        }

        publishEvent(new MessageRepublished(projection.messageId, republisher))
    };
};

exports.publish = function publish(publishEvent, author, content){
    var messageId = new MessageId(idGenerator.generate());

    publishEvent(new MessagePublished(messageId, author, content));

    return messageId;
};

exports.create = function create(events){
    return new Message(events);
};