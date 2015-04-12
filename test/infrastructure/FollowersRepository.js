var FollowersRepository = require('../../src/infrastructure/FollowersRepository');
var Subscription = require('../../src/domain/core/Subscription');
var FollowerProjection = require('../../src/domain/core/FollowerProjection');
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Followers Repository', function() {
    var followee = new UserId('followee@mix-it.fr');
    var follower = new UserId('follower@mix-it.fr');

    var repository;
    beforeEach(function(){
        repository = FollowersRepository.create();
    });

    it('When save Then getFollowers Return followerId', function() {
        repository.save(FollowerProjection.create(followee, follower));

        var followers = repository.getFollowers(followee);

        expect(followers).to.eql([follower]);
    });

    it('When save several followers but not same followee Then getFollowers return only followers of followee', function() {
        repository.save(FollowerProjection.create(followee, follower));
        repository.save(FollowerProjection.create(new UserId('otherFollowee@mix-it.fr'), new UserId('follower2@mix-it.fr')));

        var followers = repository.getFollowers(followee);

        expect(followers).to.eql([follower]);
    });
});