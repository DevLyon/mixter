var SessionsRepository = require('../../src/infrastructure/SessionsRepository');
var SessionProjection = require('../../src/domain/Identity/SessionProjection');
var Session = require('../../src/domain/Identity/Session');
var UserIdentity = require('../../src/domain/Identity/UserIdentity');
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    var sessionId = new Session.SessionId('SessionA');
    var userId = new UserIdentity.UserIdentityId('user1@mix-it.fr');

    var repository;
    beforeEach(function(){
        repository = SessionsRepository.create();
    });

    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var userId = repository.getUserIdOfSession(sessionId);

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        repository.save(new SessionProjection.create(sessionId, userId, SessionProjection.SessionEnabled));
        repository.save(new SessionProjection.create(
            new Session.SessionId('SessionB'),
            new UserIdentity.UserIdentityId('user2@mix-it.fr'),
            SessionProjection.SessionEnabled));

        expect(repository.getUserIdOfSession(sessionId)).to.eql(userId);
    });

    it('Given user disconnected When getUserIdOfSession Then return empty', function() {
        repository.save(new SessionProjection.create(sessionId, userId, SessionProjection.SessionDisabled));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });

    it('Given already projection When save same projection Then update projection', function() {
        repository.save(new SessionProjection.create(sessionId, userId, SessionProjection.SessionEnabled));

        repository.save(new SessionProjection.create(sessionId, userId, SessionProjection.SessionDisabled));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});