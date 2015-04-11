var followersRepository = require('../../src/infrastructure/followersRepository');
var subscription = require('../../src/domain/core/subscription');
var followerProjection = require('../../src/domain/core/followerProjection');
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Followers Repository', function() {
    var followee = new UserId('followee@mix-it.fr');
    var follower = new UserId('follower@mix-it.fr');

    var repository;
    beforeEach(function(){
        repository = followersRepository.create();
    });

    it('When save Then getFollowers Return followerId', function() {
        repository.save(followerProjection.create(followee, follower));

        var followers = repository.getFollowers(followee);

        expect(followers).to.eql([follower]);
    });
});