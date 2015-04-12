var EventsStore = require('../../src/infrastructure/EventsStore');
var SessionsRepository = require('../../src/infrastructure/SessionsRepository');
var SessionProjection = require('../../src/domain/Identity/SessionProjection');
var Session = require('../../src/domain/Identity/Session');
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Sessions Repository', function() {
    var sessionId = new Session.SessionId('SessionA');
    var userId = new UserId('user1@mix-it.fr');

    var eventsStore;
    var repository;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        repository = SessionsRepository.create(eventsStore);
    });

    it('Given no projections When getUserIdOfSession Then return empty', function() {
        var userId = repository.getUserIdOfSession(sessionId);

        expect(userId).to.be.null;
    });

    it('Given several user connected When getUserIdOfSession Then userId of this session', function() {
        repository.save(new SessionProjection.create(sessionId, userId, SessionProjection.SessionEnabled));
        repository.save(new SessionProjection.create(
            new Session.SessionId('SessionB'),
            new UserId('user2@mix-it.fr'),
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

    it('Given UserConnected When getSession Then return Session aggregate', function() {
        var userConnected = new Session.UserConnected(sessionId, userId, new Date());
        eventsStore.store(userConnected);

        var session = repository.getSession(userConnected.sessionId);

        expect(session).not.to.empty;
    });

    it('Given no events When getSession Then throw UnknownSession', function() {
        expect(function () {
            repository.getSession(sessionId);
        }).to.throw(SessionsRepository.UnknownSession);
    });
});