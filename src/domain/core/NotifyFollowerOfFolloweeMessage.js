var Message = require('./Message');

var NotifyFollowerOfFolloweeMessage = function NotifyFollowerOfFolloweeMessage(subscriptionsRepository){
    var self = this;

    var notifyFollowers = function notifyFollowers(eventPublisher, author, messageId){
        subscriptionsRepository.getSubscriptionsOfUser(author).forEach(function(subscription) {
            subscription.notifyFollower(eventPublisher.publish, messageId);
        });
    };

    self.register = function register(eventPublisher) {
        eventPublisher.on(Message.MessagePublished, function(event){
            notifyFollowers(eventPublisher, event.author, event.messageId);
        }).on(Message.ReplyMessagePublished, function(event){
            notifyFollowers(eventPublisher, event.replier, event.replyId);
        });
    };
};

exports.create = function create(subscriptionsRepository){
    return new NotifyFollowerOfFolloweeMessage(subscriptionsRepository);
};