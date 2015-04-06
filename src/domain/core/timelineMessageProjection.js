var TimelineMessageProjection = function TimelineMessageProjection(ownerId, authorId, content, messageId){
    this.ownerId = ownerId;
    this.authorId = authorId;
    this.content = content;
    this.messageId = messageId;
};

exports.create = function(ownerId, authorId, content, messageId){
    return new TimelineMessageProjection(ownerId, authorId, content, messageId);
};
