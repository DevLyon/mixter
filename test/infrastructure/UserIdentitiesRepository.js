var EventsStore = require('../../src/infrastructure/EventsStore');
var UserIdentitiesRepository = require('../../src/infrastructure/UserIdentitiesRepository');
var UserIdentity = require('../../src/domain/Identity/UserIdentity');
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('UserIdentities Repository', function() {
    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        repository = UserIdentitiesRepository.create(eventsStore);
    });

    it('Given UserRegistered When getUserIdentity Then return UserIdentity aggregate', function() {
        var userRegistered = new UserIdentity.UserRegistered(new UserId('user@mix-it.fr'));
        eventsStore.store(userRegistered);

        var userIdentity = repository.getUserIdentity(userRegistered.userId);

        expect(userIdentity).not.to.empty;
    });

    it('Given no events When getUserIdentity Then throw UnknownUserIdentity', function() {
        expect(function () {
            repository.getUserIdentity(new UserId('badUser@d.com'));
        }).to.throw(UserIdentitiesRepository.UnknownUserIdentity);
    });
});