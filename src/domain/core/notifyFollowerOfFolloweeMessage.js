var NotifyFollowerOfFolloweeMessage = function NotifyFollowerOfFolloweeMessage(subscriptionsRepository){
    var self = this;

    self.register = function register(eventPublisher) {
    };
};

exports.create = function create(subscriptionsRepository){
    return new NotifyFollowerOfFolloweeMessage(subscriptionsRepository);
};