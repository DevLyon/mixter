var SessionsRepository = require('../../../src/infrastructure/SessionsRepository');
var SessionHandler = require('../../../src/domain/identity/SessionHandler');
var Session = require('../../../src/domain/identity/Session');
var UserIdentity = require('../../../src/domain/identity/UserIdentity');
var expect = require('chai').expect;

describe('Session Handler', function() {
    var sessionId = new Session.SessionId('SessionA');
    var userId = new UserIdentity.UserIdentityId('user1@mix-it.fr');

    var repository;
    var handler;
    beforeEach(function(){
        repository = SessionsRepository.create();
        handler = SessionHandler.create(repository);
    });

    it('When UserConnected Then store SessionProjection', function() {
        var userConnected = new Session.UserConnected(sessionId, userId, new Date());

        handler.handleUserConnected(userConnected);

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });

    it('When UserDiconnected Then update SessionProjection and enable disconnected flag', function() {
        handler.handleUserConnected(new Session.UserConnected(sessionId, userId, new Date()));

        handler.handleUserDisconnected(new Session.UserDisconnected(sessionId, userId));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});