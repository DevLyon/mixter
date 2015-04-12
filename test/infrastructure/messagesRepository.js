var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var messagesRepository = require('../../src/infrastructure/messagesRepository');
var message = require('../../src/domain/core/message');
var MessageId = message.MessageId;
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Messages Repository', function() {
    var messageQuacked = new message.MessageQuacked(new MessageId('M1'), new UserId('author@mix-it.fr'), 'Hello');

    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = createEventsStore();
        repository = messagesRepository.create(eventsStore);
    });

    it('Given MessageQuacked When GetMessage Then return Message aggregate', function(done) {
        eventsStore.store(messageQuacked);

        var messageM1 = repository.getMessage(messageQuacked.messageId);

        messageM1.requack(function(event){
            expect(event.messageId).to.equal(messageQuacked.messageId);

            done();
        }, new UserId('requacker@mix-it.fr'));
    });

    it('Given MessageQuacked Then getDescription Then return MessageDescription', function() {
        eventsStore.store(messageQuacked);

        var description = repository.getDescription(messageQuacked.messageId);

        expect(description).to.eql(new message.MessageDescription(messageQuacked.author, messageQuacked.content));
    });
});