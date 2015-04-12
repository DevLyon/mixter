var Subscription = require('./Subscription');
var FollowerProjection = require('./FollowerProjection');

var UpdateFollowers = function UpdateFollowers(followersRepository){
    var self = this;

    self.register = function register(eventPublisher) {
        eventPublisher.on(Subscription.UserFollowed, function(event){
            followersRepository.save(FollowerProjection.create(event.subscriptionId.followee, event.subscriptionId.follower));
        });
    };
};

exports.create = function create(followersRepository){
    return new UpdateFollowers(followersRepository);
};