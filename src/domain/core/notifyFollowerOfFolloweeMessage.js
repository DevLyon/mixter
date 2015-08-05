var message = require('./message');

var NotifyFollowerOfFolloweeMessage = function NotifyFollowerOfFolloweeMessage(subscriptionsRepository){
    var self = this;

    var notifyFollowers = function notifyFollowers(eventPublisher, author, messageId){
        subscriptionsRepository.getSubscriptionsOfUser(author).forEach(function(subscription) {
            subscription.notifyFollower(eventPublisher.publish, messageId);
        });
    };

    self.register = function register(eventPublisher) {
        eventPublisher.on(message.MessageQuacked, function(event){
            notifyFollowers(eventPublisher, event.author, event.messageId);
        }).on(message.MessageRequacked, function(event){
            notifyFollowers(eventPublisher, event.requacker, event.messageId);
        });
    };
};

exports.create = function create(subscriptionsRepository){
    return new NotifyFollowerOfFolloweeMessage(subscriptionsRepository);
};