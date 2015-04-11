var TimelineMessageRepository = require('../../../src/infrastructure/TimelineMessageRepository');
var EventPublisher = require('../../../src/infrastructure/EventPublisher');
var UpdateTimeline = require('../../../src/domain/core/UpdateTimeline');
var Message = require('../../../src/domain/core/Message');
var UserId = require('../../../src/domain/UserId').UserId;
var TimelineMessageProjection = require('../../../src/domain/core/TimelineMessageProjection');
var expect = require('chai').expect;

describe('UpdateTimeline Handler', function() {
    var messageId = new Message.MessageId('M1');
    var author = new UserId('author@mix-it.fr');
    var messageContent = 'Hello';

    var repository;
    var handler;
    var eventPublisher;
    beforeEach(function(){
        repository = TimelineMessageRepository.create();
        handler = UpdateTimeline.create(repository);
        eventPublisher = EventPublisher.create();
        handler.register(eventPublisher);
    });

    it('When handle MessagePublished Then save TimelineMessageProjection for author', function() {
        var messagePublished = new Message.MessagePublished(messageId, author, messageContent);

        eventPublisher.publish(messagePublished);

        var expectedProjection = TimelineMessageProjection.create(author, author, messageContent, messageId);
        expect(repository.getMessageOfUser(author)).to.contains(expectedProjection);
    });

    it('When handle ReplyMessagePublished Then save TimelineMessageProjection for replier', function() {
        var replier = new UserId('replier@mix-it.fr');
        var replyContent = 'Reply Hello';
        var replyId = new Message.MessageId('R1');
        var replyMessagePublished = new Message.ReplyMessagePublished(replyId, replier, replyContent, messageId);

        eventPublisher.publish(replyMessagePublished);

        var expectedProjection = TimelineMessageProjection.create(replier, replier, replyContent, replyId);
        expect(repository.getMessageOfUser(replier)).to.contains(expectedProjection);
    });
});