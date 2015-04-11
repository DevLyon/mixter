var EventsStore = require('../../src/infrastructure/EventsStore');
var MessagesRepository = require('../../src/infrastructure/MessagesRepository');
var Message = require('../../src/domain/core/Message');
var MessageId = Message.MessageId;
var UserId = require('../../src/domain/UserId').UserId;
var expect = require('chai').expect;

describe('Messages Repository', function() {
    var repository;
    var eventsStore;
    beforeEach(function(){
        eventsStore = EventsStore.create();
        repository = MessagesRepository.create(eventsStore);
    });

    it('Given MessagePublished When GetMessage Then return Message aggregate', function(done) {
        var messageId = new MessageId('M1');
        eventsStore.store(new Message.MessagePublished(messageId, new UserId('author@mix-it.fr'), 'Hello'));

        var message = repository.getMessage(messageId);

        message.republish(function(event){
            expect(event.messageId).to.equal(messageId);

            done();
        }, new UserId('republisher@mix-it.fr'));
    });
});