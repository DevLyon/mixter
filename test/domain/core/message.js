var message = require('../../../src/domain/core/message');
var UserId = require('../../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Message Aggregate', function() {
    var author = new UserId('author@mix-it.fr');
    var messageContent = 'Hello';
    var messageId = new message.MessageId('MessageA');

    var eventsRaised = [];
    var publishEvent = function publishEvent(evt) {
        eventsRaised.push(evt);
    };

    beforeEach(function () {
        eventsRaised = [];
    });

    it('When create MessageId Then toString return id', function() {
        var messageId = new message.MessageId('M1');

        expect(messageId.toString()).to.equal(('Message:M1'));
    });

    it('When quack message Then raise UserMessageQuacked', function () {
        message.quack(publishEvent, author, messageContent);

        expect(eventsRaised).to.have.length(1);
        var event = eventsRaised[0];
        expect(event).to.be.an.instanceof(message.MessageQuacked);
        expect(event.author).to.equal(author);
        expect(event.content).to.equal(messageContent);
        expect(event.messageId).not.to.be.empty;
    });

    it('When quack several messages Then messageId is not same', function () {
        message.quack(publishEvent, author, messageContent);
        message.quack(publishEvent, author, messageContent);

        expect(eventsRaised[0].messageId).not.to.equal(eventsRaised[1].messageId);
    });

    it('When quack message Then return messageId', function () {
        var result = message.quack(publishEvent, author, messageContent);

        expect(result).to.equal(eventsRaised[0].messageId);
    });

    it('When create MessageQuacked Then getAggregateId return messageId', function() {
        var event = new message.MessageQuacked(new message.MessageId('M1'), author, messageContent);

        expect(event.getAggregateId()).to.equal(event.messageId);
    });

    it('When requack message Then raise MessageRequacked', function () {
        var userMessage = message.create(new message.MessageQuacked(messageId, author, messageContent));
        var requacker = new UserId('requacker@mix-it.fr');

        userMessage.requack(publishEvent, requacker);

        var expectedEvent = new message.MessageRequacked(messageId, requacker);
        expect(eventsRaised).to.contains(expectedEvent);
    });

    it('When create MessageQuacked Then aggregateId is messageId', function() {
        var event = new message.MessageQuacked(messageId, author);

        expect(event.getAggregateId()).to.equal(event.messageId);
    });

    it('When requack my own message Then do not raise MessageRequacked', function () {
        var userMessage = message.create(new message.MessageQuacked(messageId, author, messageContent));

        userMessage.requack(publishEvent, author);

        expect(eventsRaised).to.be.empty;
    });

    it('When requack two times same message Then do not raise MessageRequacked', function () {
        var requacker = new UserId('requacker@mix-it.fr');
        var userMessage = message.create([
            new message.MessageQuacked(messageId, author, messageContent),
            new message.MessageRequacked(messageId, requacker)
        ]);

        userMessage.requack(publishEvent, requacker);

        expect(eventsRaised).to.be.empty;
    });
});