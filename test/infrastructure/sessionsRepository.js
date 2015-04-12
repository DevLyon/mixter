var sessionsRepository = require('../../src/infrastructure/sessionsRepository');
var sessionProjection = require('../../src/domain/identity/sessionProjection');
var session = require('../../src/domain/identity/session');
var userIdentity = require('../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var repository = sessionsRepository.create();

        var userId = repository.getUserIdOfSession("SessionA");

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        var repository = sessionsRepository.create();
        var sessionId = new session.SessionId('SessionA');
        var userId = new userIdentity.UserIdentityId('user1@mix-it.fr');
        repository.save(new sessionProjection.create(sessionId, userId, sessionsRepository.SessionEnabled));
        repository.save(new sessionProjection.create(
            new session.SessionId('SessionB'),
            new userIdentity.UserIdentityId('user2@mix-it.fr'),
            sessionsRepository.SessionEnabled));

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });
});