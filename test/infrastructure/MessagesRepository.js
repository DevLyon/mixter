var EventsStore = require('../../src/infrastructure/EventsStore');
var MessagesRepository = require('../../src/infrastructure/MessagesRepository');
var Message = require('../../src/domain/core/Message');
var MessageId = Message.MessageId;
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Messages Repository', function() {
    var messagePublished = new Message.MessagePublished(new MessageId('M1'), new UserId('author@mix-it.fr'), 'Hello');

    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        repository = MessagesRepository.create(eventsStore);
    });

    it('Given MessagePublished When GetMessage Then return Message aggregate', function(done) {
        eventsStore.store(messagePublished);

        var message = repository.getMessage(messagePublished.messageId);

        message.republish(function(event){
            expect(event.messageId).to.equal(messagePublished.messageId);

            done();
        }, new UserId('republisher@mix-it.fr'));
    });

    it('Given MessagePublished Then getDescription Then return MessageDescription', function() {
        eventsStore.store(messagePublished);

        var description = repository.getDescription(messagePublished.messageId);

        expect(description).to.eql(new Message.MessageDescription(messagePublished.author, messagePublished.content));
    });

    it('Given ReplyMessagePublished Then getDescription Then return MessageDescription', function() {
        var replyMessagePublished = new Message.ReplyMessagePublished(
            new MessageId('R1'),
            new UserId('replier@mix-it.fr'),
            'Reply Hello',
            new MessageId('M1'));
        eventsStore.store(replyMessagePublished);

        var description = repository.getDescription(replyMessagePublished.replyId);

        expect(description).to.eql(new Message.MessageDescription(replyMessagePublished.replier, replyMessagePublished.replyContent));
    });

    it('Given no events When GetMessage Then throw UnknownMessage', function() {
        expect(function () {
            repository.getMessage(new MessageId('BadId'));
        }).to.throw(MessagesRepository.UnknownMessage);
    });
});