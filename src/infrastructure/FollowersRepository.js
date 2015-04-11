var _ = require('lodash');

var FollowersRepository = function FollowersRepository(){
    var projections = [];

    this.save = function(projection){
        projections.push(projection);
    };

    this.getFollowers = function(userId){
        return _.map(projections, function(projection){
            return projection.follower;
        });
    };
};

exports.create = function(){
    return new FollowersRepository();
};