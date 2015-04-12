var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var userIdentitiesRepository = require('../../src/infrastructure/userIdentitiesRepository');
var userIdentity = require('../../src/domain/identity/userIdentity');
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('UserIdentities Repository', function() {
    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = createEventsStore();
        repository = userIdentitiesRepository.create(eventsStore);
    });

    it('Given UserRegistered When getUserIdentity Then return UserIdentity aggregate', function() {
        var userRegistered = new userIdentity.UserRegistered(new UserId('user@mix-it.fr'));
        eventsStore.store(userRegistered);

        var user = repository.getUserIdentity(userRegistered.userId);

        expect(user).not.to.empty;
    });

    it('Given no events When getUserIdentity Then throw UnknownUserIdentity', function() {
        expect(function () {
            repository.getUserIdentity(new UserId('badUser@d.com'));
        }).to.throw(userIdentitiesRepository.UnknownUserIdentity);
    });
});