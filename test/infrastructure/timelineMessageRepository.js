var timelineMessageRepository = require('../../src/infrastructure/timelineMessageRepository');
var timelineMessageProjection = require('../../src/domain/core/timelineMessageProjection');
var UserId = require('../../src/domain/userId').UserId;
var MessageId = require('../../src/domain/core/message').MessageId;
var expect = require('chai').expect;

describe('TimelineMessage Repository', function() {
    var ownerId = new UserId('owner@mix-it.fr');
    var authorId = new UserId('author@mix-it.fr');
    var messageContent = 'hello';
    var messageId = new MessageId('M1');

    var repository;
    beforeEach(function(){
        repository = timelineMessageRepository.create();
    });

    it('Given a message saved When getMessageOfUser Then message is returned', function() {
        var message = timelineMessageProjection.create(ownerId, authorId, messageContent, messageId);

        repository.save(message);

        expect(repository.getMessageOfUser(ownerId)).to.contains(message);
    });

    it('When save two messages of different owners Then GetMessagesOfUser return only messages of user', function() {
        var message = timelineMessageProjection.create(ownerId, authorId, messageContent, messageId);
        repository.save(message);
        repository.save(timelineMessageProjection.create(
            new UserId('owner2@mix-it.fr'),
            authorId,
            messageContent,
            new MessageId('M2')));

        var messages = repository.getMessageOfUser(ownerId);

        expect(messages).to.have.length(1);
        expect(messages).to.contains(message);
    });

    it('When save two same messages Then only one is saved', function() {
        var message = timelineMessageProjection.create(ownerId, authorId, messageContent, messageId);
        repository.save(message);
        repository.save(message);

        var messages = repository.getMessageOfUser(ownerId);

        expect(messages).to.have.length(1);
    });

    it('Given a message saved for several users When remove this message Then remove this message of all users', function() {
        repository.save(timelineMessageProjection.create(ownerId, authorId, messageContent, messageId));
        var message2 = timelineMessageProjection.create(ownerId, authorId, messageContent, new MessageId('M2'));
        repository.save(message2);
        var ownerId2 = new UserId('owner2@mix-it.fr');
        repository.save(timelineMessageProjection.create(ownerId2, authorId, messageContent, messageId));

        repository.deleteMessage(messageId);

        var messagesOfOwner1 = repository.getMessageOfUser(ownerId);
        expect(messagesOfOwner1).to.have.length(1);
        expect(messagesOfOwner1).to.contains(message2);
        expect(repository.getMessageOfUser(ownerId2)).to.have.length(0);
    });
});