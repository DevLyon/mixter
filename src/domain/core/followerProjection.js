var FollowerProjection = function FollowerProjection(followee, follower){
    this.followee = followee;
    this.follower = follower;
};

exports.create = function(followee, follower){
    return new FollowerProjection(followee, follower);
};