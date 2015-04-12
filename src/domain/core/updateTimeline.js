var UpdateTimeline = function UpdateTimeline(timelineMessageRepository){
    var self = this;

    self.register = function(eventPublisher) {
    };
};

exports.create = function(timelineMessageRepository){
    return new UpdateTimeline(timelineMessageRepository);
};