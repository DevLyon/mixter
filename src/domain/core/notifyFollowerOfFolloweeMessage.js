var message = require('./message');

var NotifyFollowerOfFolloweeMessage = function NotifyFollowerOfFolloweeMessage(subscriptionsRepository){
    var self = this;

    self.register = function register(eventPublisher) {
        eventPublisher.on(message.MessageQuacked, function(event){
            subscriptionsRepository.getSubscriptionsOfUser(event.author).forEach(function(subscription) {
                subscription.notifyFollower(eventPublisher.publish, event.messageId);
            });
        });
    };
};

exports.create = function create(subscriptionsRepository){
    return new NotifyFollowerOfFolloweeMessage(subscriptionsRepository);
};