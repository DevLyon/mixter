var UserIdentity = require('../../../src/domain/identity/UserIdentity');
var expect = require('chai').expect;

describe('User Identity Aggregate', function() {
    var email = 'user@mix-it.fr';

    var eventsRaised;
    var publishEvent = function publishEvent(evt){
        eventsRaised = [];
        eventsRaised.push(evt);
    };

    it('When create UserIdentityId Then toString return email', function() {
        var id = new UserIdentity.UserIdentityId(email);

        expect(id.toString()).to.eql(('UserIdentity:' + email));
    });

    it('When register user Then raise userRegistered event', function() {
        UserIdentity.register(publishEvent, email);

        var expectedEvent = new UserIdentity.UserRegistered(new UserIdentity.UserIdentityId(email));
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('Given UserRegistered When log in Then raise UserConnected event', function(){
        var id = new UserIdentity.UserIdentityId(email);
        var userIdentity = UserIdentity.create([ new UserIdentity.UserRegistered(id) ]);

        userIdentity.logIn(publishEvent);

        expect(eventsRaised).to.have.length(1);
        var event = eventsRaised[0];
        expect(event).to.be.an.instanceof(UserIdentity.UserConnected);
        expect(event.userIdentityId).to.equal(id);
        expect(event.connectedAt - new Date()).to.within(-1, 1);
        expect(event.sessionId).not.to.be.empty;
    });
});