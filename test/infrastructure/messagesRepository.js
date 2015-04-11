var createEventsStore = require('../../src/infrastructure/eventsStore').create;
var messagesRepository = require('../../src/infrastructure/messagesRepository');
var message = require('../../src/domain/core/message');
var MessageId = message.MessageId;
var UserId = require('../../src/domain/userId').UserId;
var expect = require('chai').expect;

describe('Messages Repository', function() {
    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = createEventsStore();
        repository = messagesRepository.create(eventsStore);
    });

    it('Given MessageQuacked When GetMessage Then return message aggregate', function(done) {
        var messageId = new MessageId('M1');
        eventsStore.store(new message.MessageQuacked(messageId, new UserId('author@mix-it.fr'), 'Hello'));

        var messageM1 = repository.getMessage(messageId);

        messageM1.requack(function(event){
            expect(event.messageId).to.equal(messageId);

            done();
        }, new UserId('requacker@mix-it.fr'));
    });
});