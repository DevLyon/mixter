var _ = require('lodash');

var TimelineMessageRepository = function TimelineMessageRepository(){
    var self = this;

    var projections = [];

    var remove = function(ownerId, messageId){
        _.remove(projections, {
            ownerId: ownerId,
            messageId: messageId
        });
    };

    self.save = function(projection){
        remove(projection.ownerId, projection.messageId);

        projections.push(projection);
    };

    self.getMessageOfUser = function(userId) {
        return _.filter(projections, function(projection){
            return projection.ownerId.equals(userId);
        });
    };
};

exports.create = function(){
    return new TimelineMessageRepository();
};