var UserIdentity = require('../../../src/domain/identity/UserIdentity');
var Session = require('../../../src/domain/identity/Session');
var expect = require('chai').expect;

describe('User Identity Aggregate', function() {
    var email = 'user@mix-it.fr';

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt){
        eventsRaised.push(evt);
    };

    beforeEach(function(){
        eventsRaised = [];
    });

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
        expect(event).to.be.an.instanceof(Session.UserConnected);
        expect(event.userIdentityId).to.equal(id);
        expect(event.connectedAt - new Date()).to.within(-1, 1);
        expect(event.sessionId).not.to.be.empty;
    });

    it('When log in Then return sessionId', function(){
        var id = new UserIdentity.UserIdentityId(email);
        var userIdentity = UserIdentity.create([ new UserIdentity.UserRegistered(id) ]);

        var result = userIdentity.logIn(publishEvent);

        var event = eventsRaised[0];
        expect(result).to.equal(eventsRaised[0].sessionId);
    });
});