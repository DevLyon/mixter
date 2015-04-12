var sessionsRepository = require('../../src/infrastructure/sessionsRepository');
var sessionProjection = require('../../src/domain/identity/sessionProjection');
var session = require('../../src/domain/identity/session');
var userIdentity = require('../../src/domain/identity/userIdentity');
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    var sessionId = new session.SessionId('SessionA');
    var userId = new userIdentity.UserIdentityId('user1@mix-it.fr');

    var repository;
    beforeEach(function(){
        repository = sessionsRepository.create();
    });

    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var userId = repository.getUserIdOfSession(sessionId);

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        repository.save(new sessionProjection.create(sessionId, userId, sessionProjection.SessionEnabled));
        repository.save(new sessionProjection.create(
            new session.SessionId('SessionB'),
            new userIdentity.UserIdentityId('user2@mix-it.fr'),
            sessionProjection.SessionEnabled));

        expect(repository.getUserIdOfSession(sessionId)).to.eql(userId);
    });

    it('Given user disconnected When getUserIdOfSession Then return empty', function() {
        repository.save(new sessionProjection.create(sessionId, userId, sessionProjection.SessionDisabled));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });

    it('Given already projection When save same projection Then update projection', function() {
        repository.save(new sessionProjection.create(sessionId, userId, sessionProjection.SessionEnabled));

        repository.save(new sessionProjection.create(sessionId, userId, sessionProjection.SessionDisabled));

        expect(repository.getUserIdOfSession(sessionId)).to.be.null;
    });
});