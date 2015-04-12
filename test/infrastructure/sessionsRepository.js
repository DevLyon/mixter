var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var sessionsRepository = require('../../src/infrastructure/sessionsRepository');
var sessionProjection = require('../../src/domain/identity/sessionProjection');
var session = require('../../src/domain/identity/session');
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    var sessionId = new session.SessionId('SessionA');
    var userId = new UserId('user1@mix-it.fr');

    var eventsStore;
    var repository;
    beforeEach(function(){
        eventsStore = createEventsStore();
        repository = sessionsRepository.create(eventsStore);
    });

    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var userId = repository.getUserIdOfSession(sessionId);

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        repository.save(new sessionProjection.create(sessionId, userId, sessionProjection.SessionEnabled));
        repository.save(new sessionProjection.create(
            new session.SessionId('SessionB'),
            new UserId('user2@mix-it.fr'),
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

    it('Given UserConnected When getSession Then return Session aggregate', function() {
        var userConnected = new session.UserConnected(sessionId, userId, new Date());
        eventsStore.store(userConnected);

        var userSession = repository.getSession(userConnected.sessionId);

        expect(userSession).not.to.empty;
    });

    it('Given no events When getSession Then throw UnknownSession', function() {
        expect(function () {
            repository.getSession(sessionId);
        }).to.throw(sessionsRepository.UnknownSession);
    });
});