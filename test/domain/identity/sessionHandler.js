var sessionsRepository = require('../../../src/infrastructure/sessionsRepository');
var sessionHandler = require('../../../src/domain/identity/sessionHandler');
var session = require('../../../src/domain/identity/session');
var userIdentity = require('../../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('Session Handler', function() {
    var sessionId = new session.SessionId('SessionA');
    var userId = new userIdentity.UserIdentityId('user1@mix-it.fr');

    var repository;
    var handler;
    beforeEach(function(){
        repository = sessionsRepository.create();
        handler = sessionHandler.create(repository);
    });

    it('When UserConnected Then store SessionProjection', function() {
        var userConnected = new session.UserConnected(sessionId, userId, new Date());

        handler.handleUserConnected(userConnected);

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });

    it('When UserDiconnected Then update SessionProjection and enable disconnected flag', function() {
        handler.handleUserConnected(new session.UserConnected(sessionId, userId, new Date()));

        handler.handleUserDisconnected(new session.UserDisconnected(sessionId, userId));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});