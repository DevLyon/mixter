var SessionsRepository = require('../../../src/infrastructure/SessionsRepository');
var EventPublisher = require('../../../src/infrastructure/EventPublisher');
var SessionHandler = require('../../../src/domain/identity/SessionHandler');
var Session = require('../../../src/domain/identity/Session');
var UserIdentity = require('../../../src/domain/identity/UserIdentity');
var UserId = require('../../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Session Handler', function() {
    var sessionId = new Session.SessionId('SessionA');
    var userId = new UserId('user1@mix-it.fr');

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = SessionsRepository.create();
        handler = SessionHandler.create(repository);
        eventPublisher = EventPublisher.create();
        handler.register(eventPublisher);
    });

    it('When UserConnected Then store SessionProjection', function() {
        var userConnected = new Session.UserConnected(sessionId, userId, new Date());

        eventPublisher.publish(userConnected);

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });

    it('When UserDiconnected Then update SessionProjection and enable disconnected flag', function() {
        eventPublisher.publish(new Session.UserConnected(sessionId, userId, new Date()));

        eventPublisher.publish(new Session.UserDisconnected(sessionId, userId));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});