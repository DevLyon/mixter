var TimelineMessageRepository = require('../../src/infrastructure/TimelineMessageRepository');
var TimelineMessageProjection = require('../../src/domain/core/TimelineMessageProjection');
var UserId = require('../../src/domain/UserId').UserId;
var MessageId = require('../../src/domain/core/Message').MessageId;
var expect = require('chai').expect;

describe('TimelineMessage Repository', function() {
    var ownerId = new UserId('owner@mix-it.fr');
    var authorId = new UserId('author@mix-it.fr');
    var messageContent = 'hello';
    var messageId = new MessageId('M1');

    var repository;
    beforeEach(function(){
        repository = TimelineMessageRepository.create();
    });

    it('Given a message saved When getMessageOfUser Then message is returned', function() {
        var message = TimelineMessageProjection.create(ownerId, authorId, messageContent, messageId);

        repository.save(message);

        expect(repository.getMessageOfUser(ownerId)).to.contains(message);
    });

    it('When save two messages of different owners Then GetMessagesOfUser return only messages of user', function() {
        var message = TimelineMessageProjection.create(ownerId, authorId, messageContent, messageId);
        repository.save(message);
        repository.save(TimelineMessageProjection.create(
            new UserId('owner2@mix-it.fr'),
            authorId,
            messageContent,
            new MessageId('M2')));

        var messages = repository.getMessageOfUser(ownerId);

        expect(messages).to.have.length(1);
        expect(messages).to.contains(message);
    });

    it('When save two same messages Then only one is saved', function() {
        var message = TimelineMessageProjection.create(ownerId, authorId, messageContent, messageId);
        repository.save(message);
        repository.save(message);

        var messages = repository.getMessageOfUser(ownerId);

        expect(messages).to.have.length(1);
    });
});