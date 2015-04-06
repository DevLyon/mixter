var TimelineMessageRepository = function TimelineMessageRepository(){
    var self = this;

    var projections = [];

    self.save = function(projection){
        projections.push(projection);
    };

    self.getMessageOfUser = function(userId) {
        return projections;
    };
};

exports.create = function(){
    return new TimelineMessageRepository();
};