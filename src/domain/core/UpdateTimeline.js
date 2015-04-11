var Message = require('./Message');
var TimelineMessageProjection = require('./TimelineMessageProjection');

var UpdateTimeline = function UpdateTimeline(timelineMessageRepository){
    var self = this;

    var saveProjection = function(owner, author, content, messageId){
        var projection = TimelineMessageProjection.create(owner, author, content, messageId);
        timelineMessageRepository.save(projection);
    };

    self.register = function(eventPublisher) {
        eventPublisher.on(Message.MessagePublished, function(event){
            saveProjection(event.author, event.author, event.content, event.messageId);
        });
    };
};

exports.create = function(timelineMessageRepository){
    return new UpdateTimeline(timelineMessageRepository);
};