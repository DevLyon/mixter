var idGenerator = require('../../idGenerator');
var valueType = require('../../valueType');

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

var Message = function Message(){

};

exports.publish = function publish(publishEvent, author, content){
    var messageId = new MessageId(idGenerator.generate());

    publishEvent(new MessagePublished(messageId, author, content));
};