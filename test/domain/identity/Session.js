var Session = require('../../../src/domain/identity/Session');
var UserIdentity = require('../../../src/domain/identity/UserIdentity');
var expect = require('chai').expect;

describe('Session Aggregate', function() {
    var userIdentityId = new UserIdentity.UserIdentityId('user@mix-it.fr');
    var sessionId = new Session.SessionId('SessionA');

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
        var session = Session.create(new Session.UserConnected(sessionId, userIdentityId, new Date()));

        session.logOut(publishEvent);

        var expectedEvent = new Session.UserDisconnected(sessionId, userIdentityId);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('Given user disconnected When user log out Then nothing', function() {
        var session = Session.create([
            new Session.UserConnected(sessionId, userIdentityId, new Date()),
            new Session.UserDisconnected(sessionId, userIdentityId)
        ]);

        session.logOut(publishEvent);

        expect(eventsRaised).to.be.empty;
    });

    it('When create UserConnected Then aggregateId is sessionId', function() {
        var event = new Session.UserConnected(sessionId, userIdentityId, new Date());

        expect(event.getAggregateId()).to.equal(sessionId);
    });

    it('When create UserDisconnected Then aggregateId is sessionId', function() {
        var event = new Session.UserDisconnected(sessionId, userIdentityId, new Date());

        expect(event.getAggregateId()).to.equal(sessionId);
    });
});