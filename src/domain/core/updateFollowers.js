var subscription = require('./subscription');
var followerProjection = require('./followerProjection');

var UpdateFollowers = function UpdateFollowers(followersRepository){
    var self = this;

    self.register = function register(eventPublisher) {
        eventPublisher.on(subscription.UserFollowed, function(event){
            followersRepository.save(followerProjection.create(event.subscriptionId.followee, event.subscriptionId.follower));
        });
    };
};

exports.create = function create(followersRepository){
    return new UpdateFollowers(followersRepository);
};