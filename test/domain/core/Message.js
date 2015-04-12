var Message = require('../../../src/domain/core/Message');
var UserId = require('../../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Message Aggregate', function() {
    var author = new UserId('author@mix-it.fr');
    var messageContent = 'Hello';
    var messageId = new Message.MessageId('MessageA');

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt) {
        eventsRaised.push(evt);
    };

    beforeEach(function () {
        eventsRaised = [];
    });

    it('When create MessageId Then toString return id', function() {
        var messageId = new Message.MessageId('M1');

        expect(messageId.toString()).to.equal(('Message:M1'));
    });

    it('When publish message Then raise UserMessagePublished', function () {
        Message.publish(publishEvent, author, messageContent);

        expect(eventsRaised).to.have.length(1);
        var event = eventsRaised[0];
        expect(event).to.be.an.instanceof(Message.MessagePublished);
        expect(event.author).to.equal(author);
        expect(event.content).to.equal(messageContent);
        expect(event.messageId).not.to.be.empty;
    });

    it('When publish several messages Then messageId is not same', function () {
        Message.publish(publishEvent, author, messageContent);
        Message.publish(publishEvent, author, messageContent);

        expect(eventsRaised[0].messageId).not.to.equal(eventsRaised[1].messageId);
    });

    it('When publish message Then return messageId', function () {
        var result = Message.publish(publishEvent, author, messageContent);

        expect(result).to.equal(eventsRaised[0].messageId);
    });

    it('When create MessagePublished Then getAggregateId return messageId', function() {
        var event = new Message.MessagePublished(new Message.MessageId('M1'), author, messageContent);

        expect(event.getAggregateId()).to.equal(event.messageId);
    });

    it('When republish message Then raise MessageRepublished', function () {
        var message = Message.create(new Message.MessagePublished(messageId, author, messageContent));
        var republisher = new UserId('republisher@mix-it.fr');

        message.republish(publishEvent, republisher);

        var expectedEvent = new Message.MessageRepublished(messageId, republisher);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('When create MessageRepublished Then aggregateId is messageId', function() {
        var event = new Message.MessageRepublished(messageId, author);

        expect(event.getAggregateId()).to.equal(event.messageId);
    });

    it('When republish my own message Then do not raise MessageRepublished', function () {
        var message = Message.create(new Message.MessagePublished(messageId, author, messageContent));

        message.republish(publishEvent, author);

        expect(eventsRaised).to.be.empty;
    });

    it('When republish two times same message Then do not raise MessageRepublished', function () {
        var republisher = new UserId('republisher@mix-it.fr');
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent),
            new Message.MessageRepublished(messageId, republisher)
        ]);

        message.republish(publishEvent, republisher);

        expect(eventsRaised).to.be.empty;
    });

    it('When reply Then raise ReplyMessagePublished', function () {
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent)
        ]);
        var replier = new UserId('replier@mix-it.fr');
        var replyContent = 'Hi';

        message.reply(publishEvent, replier, replyContent);

        expect(eventsRaised).to.have.length(1);
        var event = eventsRaised[0];
        expect(event).to.be.an.instanceof(Message.ReplyMessagePublished);
        expect(event.parentId).to.equal(messageId);
        expect(event.replyContent).to.equal(replyContent);
        expect(event.replier).to.equal(replier);
        expect(event.replyId).not.to.be.empty;
        expect(event.replyId).not.to.equal(messageId);
    });

    it('When create ReplyMessagePublished Then aggregateId is replyId', function() {
        var event = new Message.ReplyMessagePublished(
            new Message.MessageId('RA'),
            new UserId('replier@mix-it.fr'),
            'replyContent',
            messageId);

        expect(event.getAggregateId()).to.equal(event.replyId);
    });

    it('Given reply message published When use messageId Then is replyId', function () {
        var message = Message.create([
            new Message.ReplyMessagePublished(messageId, author, messageContent, new Message.MessageId('MessageId'))
        ]);
        var republisher = new UserId('republisher@mix-it.fr');

        message.republish(publishEvent, republisher);

        var expectedEvent = new Message.MessageRepublished(messageId, republisher);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('Given reply message published When replier republish Then nothing', function () {
        var message = Message.create([
            new Message.ReplyMessagePublished(messageId, author, messageContent, new Message.MessageId('MessageId'))
        ]);

        message.republish(publishEvent, author);

        expect(eventsRaised).to.be.empty;
    });

    it('When create MessageDeleted Then aggregateId is messageId', function() {
        var event = new Message.MessageDeleted(messageId);

        expect(event.getAggregateId()).to.equal(event.messageId);
    });

    it('When delete Then raise MessageDeleted', function () {
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent)
        ]);
        var deleter = author;

        message.delete(publishEvent, deleter);

        var expectedEvent = new Message.MessageDeleted(messageId);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('When delete by someone else than author Then do not raise MessageDeleted', function () {
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent)
        ]);
        var deleter = new UserId('baduser@mix-it.fr');

        message.delete(publishEvent, deleter);

        expect(eventsRaised).to.be.empty;
    });

    it('Given deleted message When delete Then nothing', function () {
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent),
            new Message.MessageDeleted(messageId)
        ]);

        message.delete(publishEvent, author);

        expect(eventsRaised).to.be.empty;
    });

    it('Given a deleted message When reply Then do not raise MessageDeleted', function () {
        var message = Message.create([
            new Message.MessagePublished(messageId, author, messageContent),
            new Message.MessageDeleted(messageId)
        ]);

        message.reply(publishEvent, new UserId('replier@mix-it.fr'), 'reply content');

        expect(eventsRaised).to.be.empty;
    });
});