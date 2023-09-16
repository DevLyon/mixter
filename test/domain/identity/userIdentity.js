var userIdentity = require('../../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('User Identity Aggregate', function() {
    var email = 'user@mix-it.fr';

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt){
        eventsRaised.push(evt);
    };

    it('When create UserIdentityId Then toString return email', function() {
        var id = new userIdentity.UserIdentityId(email);

        expect(id.toString()).to.eql(('UserIdentity:' + email));
    });

    it('When register user Then raise userRegistered event', function() {
        userIdentity.register(publishEvent, email);

        var expectedEvent = new userIdentity.UserRegistered(new userIdentity.UserIdentityId(email));
        expect(eventsRaised).to.deep.contains(expectedEvent);
    });
});