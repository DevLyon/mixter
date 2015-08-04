var session = require('../../../src/domain/identity/session');
var userIdentity = require('../../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('Session Aggregate', function() {
    var userIdentityId = new userIdentity.UserIdentityId('user@mix-it.fr');
    var sessionId = new session.SessionId('SessionA');

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt){
        eventsRaised.push(evt);
    };

    beforeEach(function(){
        eventsRaised = [];
    });

    it('When create SessionId Then toString return id', function() {
        expect(sessionId.toString()).to.eql(('Session:SessionA'));
    });

    it('When user logout Then raise UserDisconnected event', function() {
        var userSession = session.create(new session.UserConnected(sessionId, userIdentityId, new Date()));

        userSession.logOut(publishEvent);

        var expectedEvent = new session.UserDisconnected(sessionId, userIdentityId);
        expect(eventsRaised).to.contains(expectedEvent);
    });
});