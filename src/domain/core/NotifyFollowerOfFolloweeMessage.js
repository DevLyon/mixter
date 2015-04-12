var Message = require('./Message');

var NotifyFollowerOfFolloweeMessage = function NotifyFollowerOfFolloweeMessage(subscriptionsRepository){
    var self = this;

    self.register = function register(eventPublisher) {
        eventPublisher.on(Message.MessagePublished, function(event){
            subscriptionsRepository.getSubscriptionsOfUser(event.author).forEach(function(subscription) {
                subscription.notifyFollower(eventPublisher.publish, event.messageId);
            });
        });
    };
};

exports.create = function create(subscriptionsRepository){
    return new NotifyFollowerOfFolloweeMessage(subscriptionsRepository);
};