var SessionsRepository = require('../../src/infra/SessionsRepository');
var SessionProjection = require('../../src/domain/Identity/SessionProjection');
var Session = require('../../src/domain/Identity/Session');
var UserIdentity = require('../../src/domain/Identity/UserIdentity');
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var repository = SessionsRepository.create();

        var userId = repository.getUserIdOfSession("SessionA");

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        var repository = SessionsRepository.create();
        var sessionId = new Session.SessionId('SessionA');
        var userId = new UserIdentity.UserIdentityId('user1@mix-it.fr');
        repository.save(new SessionProjection.create(sessionId, userId, SessionsRepository.SessionEnabled));
        repository.save(new SessionProjection.create(
            new Session.SessionId('SessionB'),
            new UserIdentity.UserIdentityId('user2@mix-it.fr'),
            SessionsRepository.SessionEnabled));

        expect(repository.getUserIdOfSession(sessionId)).to.equal(userId);
    });
});