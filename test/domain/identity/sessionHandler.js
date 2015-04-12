var sessionsRepository = require('../../../src/infrastructure/sessionsRepository');
var eventPublisherModule = require('../../../src/infrastructure/eventPublisher');
var sessionHandler = require('../../../src/domain/identity/sessionHandler');
var session = require('../../../src/domain/identity/session');
var userIdentity = require('../../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('Session Handler', function() {
    var sessionId = new session.SessionId('SessionA');
    var userId = new userIdentity.UserIdentityId('user1@mix-it.fr');

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = sessionsRepository.create();
        handler = sessionHandler.create(repository);
        eventPublisher = eventPublisherModule.create();
        handler.register(eventPublisher);
    });

    it('When UserConnected Then store SessionProjection', function() {
        var userConnected = new session.UserConnected(sessionId, userId, new Date());

        eventPublisher.publish(userConnected);

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });

    it('When UserDiconnected Then update SessionProjection and enable disconnected flag', function() {
        eventPublisher.publish(new session.UserConnected(sessionId, userId, new Date()));

        eventPublisher.publish(new session.UserDisconnected(sessionId, userId));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});