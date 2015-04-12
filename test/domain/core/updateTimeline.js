var timelineMessageRepository = require('../../../src/infrastructure/timelineMessageRepository');
var createEventPublisher = require('../../../src/infrastructure/eventPublisher').create;
var updateTimeline = require('../../../src/domain/core/updateTimeline');
var message = require('../../../src/domain/core/message');
var UserId = require('../../../src/domain/userId').UserId;
var timelineMessageProjection = require('../../../src/domain/core/timelineMessageProjection');
var expect = require('chai').expect;

describe('UpdateTimeline Handler', function() {
    var messageId = new message.MessageId('M1');
    var author = new UserId('author@mix-it.fr');
    var messageContent = 'Hello';

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = timelineMessageRepository.create();
        handler = updateTimeline.create(repository);
        eventPublisher = createEventPublisher();
        handler.register(eventPublisher);
    });

    it('When handle MessageQuacked Then save TimelineMessageProjection for author', function() {
        var messageQuacked = new message.MessageQuacked(messageId, author, messageContent);

        eventPublisher.publish(messageQuacked);

        var expectedProjection = timelineMessageProjection.create(author, author, messageContent, messageId);
        expect(repository.getMessageOfUser(author)).to.contains(expectedProjection);
    });
});