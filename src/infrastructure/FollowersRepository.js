var _ = require('lodash');

var FollowersRepository = function FollowersRepository(){
    var projections = [];

    this.save = function save(projection){
        projections.push(projection);
    };

    this.getFollowers = function getFollowers(userId){
        return _.chain(projections).filter(function(projection){
            return projection.followee.equals(userId);
        }).map(function(projection){
            return projection.follower;
        }).value();
    };
};

exports.create = function create(){
    return new FollowersRepository();
};