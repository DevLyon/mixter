var idGenerator = require('../../idGenerator');
var valueType = require('../../valueType');

var MessageId = exports.MessageId = valueType.extends(function MessageId(id){
    this.id = id;
}, function toString() {
    return 'Message:' + this.id;
});

var MessageQuacked = exports.MessageQuacked = function MessageQuacked(messageId, author, content){
    this.messageId = messageId;
    this.author = author;
    this.content = content;

    Object.freeze(this);
};

MessageQuacked.prototype.getAggregateId = function getAggregateId(){
    return this.messageId;
};

var Message = function Message(){

};

exports.quack = function quack(publishEvent, author, content){
    var messageId = new MessageId(idGenerator.generate());

    publishEvent(new MessageQuacked(messageId, author, content));

    return messageId;
};