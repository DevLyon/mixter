var idGenerator = require('../../idGenerator');
var valueType = require('../../valueType');
var DecisionProjection = require('../DecisionProjection');
var _ = require('lodash');

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

var ReplyMessagePublished = exports.ReplyMessagePublished = function ReplyMessagePublished(replyId, replier, replyContent, parentId){
    this.replyId = replyId;
    this.replier = replier;
    this.replyContent = replyContent;
    this.parentId = parentId;

    Object.freeze(this);
};

ReplyMessagePublished.prototype.getAggregateId = function getAggregateId(){
    return this.replyId;
};

var MessageDeleted = exports.MessageDeleted = function MessageDeleted(messageId){
    this.messageId = messageId;

    Object.freeze(this);
};

MessageDeleted.prototype.getAggregateId = function getAggregateId(){
    return this.messageId;
};

var Message = function Message(events){
    var self = this;

    var projection = DecisionProjection.create().register(MessagePublished, function(event) {
        this.messageId = event.messageId;
        this.author = event.author;
    }).register(ReplyMessagePublished, function(event) {
        this.messageId = event.replyId;
        this.author = event.replier;
    }).register(MessageRepublished, function(event) {
        if(!this.republishers){
            this.republishers = [];
        }

        this.republishers.push(event.republisher);
    }).register(MessageDeleted, function(event) {
        this.isDeleted = true;
    }).apply(events);

    self.republish = function republish(publishEvent, republisher) {
        if(projection.author.equals(republisher) || _.includes(projection.republishers, republisher)){
            return;
        }

        publishEvent(new MessageRepublished(projection.messageId, republisher));
    };

    self.reply = function reply(publishEvent, replier, replyContent){
        if(projection.isDeleted){
            return;
        }

        var replyId = new MessageId(idGenerator.generate());
        publishEvent(new ReplyMessagePublished(replyId, replier, replyContent, projection.messageId));
    };

    self.delete = function(publishEvent, deleter){
        if(!deleter.equals(projection.author) || projection.isDeleted){
            return;
        }

        publishEvent(new MessageDeleted(projection.messageId));
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