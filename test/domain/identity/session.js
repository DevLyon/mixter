var session = require('../../../src/domain/identity/session');
var userIdentity = require('../../../src/domain/identity/userIdentity');
var UserId = require('../../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Session Aggregate', function() {
    var userId = new UserId('user@mix-it.fr');
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
        var userSession = session.create(new session.UserConnected(sessionId, userId, new Date()));

        userSession.logOut(publishEvent);

        var expectedEvent = new session.UserDisconnected(sessionId, userId);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('Given user disconnected When user log out Then nothing', function() {
        var userSession = session.create([
            new session.UserConnected(sessionId, userId, new Date()),
            new session.UserDisconnected(sessionId, userId)
        ]);

        userSession.logOut(publishEvent);

        expect(eventsRaised).to.be.empty;
    });

    it('When create UserConnected Then aggregateId is sessionId', function() {
        var event = new session.UserConnected(sessionId, userId, new Date());

        expect(event.getAggregateId()).to.equal(sessionId);
    });

    it('When create UserDisconnected Then aggregateId is sessionId', function() {
        var event = new session.UserDisconnected(sessionId, userId, new Date());

        expect(event.getAggregateId()).to.equal(sessionId);
    });
});