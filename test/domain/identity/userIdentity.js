var userIdentity = require('../../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('User Identity Aggregate', function() {
    var email = 'user@mix-it.fr';

    var eventsRaised;
    var publishEvent = function publishEvent(evt){
        eventsRaised = [];
        eventsRaised.push(evt);
    };

    it('When create UserIdentityId Then toString return email', function() {
        var id = new userIdentity.UserIdentityId(email);

        expect(id.toString()).to.eql(('UserIdentity:' + email));
    });

    it('When register user Then raise userRegistered event', function() {
        userIdentity.register(publishEvent, email);

        var expectedEvent = new userIdentity.UserRegistered(new userIdentity.UserIdentityId(email));
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('Given UserRegistered When log in Then raise UserConnected event', function(){
        var id = new userIdentity.UserIdentityId(email);
        var user = userIdentity.create([ new userIdentity.UserRegistered(id) ]);

        user.logIn(publishEvent);

        expect(eventsRaised).to.have.length(1);
        var event = eventsRaised[0];
        expect(event).to.be.an.instanceof(userIdentity.UserConnected);
        expect(event.userIdentityId).to.equal(id);
        expect(event.connectedAt - new Date()).to.within(-1, 1);
        expect(event.sessionId).not.to.be.empty;
    });
});