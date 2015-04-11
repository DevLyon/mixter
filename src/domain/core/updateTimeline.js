var message = require('./message');
var timelineMessageProjection = require('./timelineMessageProjection');

var UpdateTimeline = function UpdateTimeline(timelineMessageRepository){
    var self = this;

    var saveProjection = function(owner, author, content, messageId){
        var projection = timelineMessageProjection.create(owner, author, content, messageId);
        timelineMessageRepository.save(projection);
    };

    self.register = function(eventPublisher) {
        eventPublisher.on(message.MessageQuacked, function(event){
            saveProjection(event.author, event.author, event.content, event.messageId);
        });
    };
};

exports.create = function(timelineMessageRepository){
    return new UpdateTimeline(timelineMessageRepository);
};