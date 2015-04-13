<?php

namespace Tests\Domain\Timeline;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessageProjection;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Messages\ReplyMessagePublished;
use App\Domain\Subscriptions\FolloweeMessagePublished;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\Timeline\ITimelineMessageRepository;
use App\Domain\Timeline\TimelineMessage;
use App\Domain\Timeline\TimelineMessageId;
use App\Domain\Timeline\UpdateTimeline;
use App\Infrastructure\IProjectionStore;
use App\Infrastructure\Messages\MessageProjectionRepository;
use App\Infrastructure\Timeline\TimelineMessageRepository;
use Tests\Infrastructure\InMemoryProjectionStore;

class UpdateTimelineTest extends \PHPUnit_Framework_TestCase
{
    /** @var IProjectionStore */
    private $projectionStore;

    /** @var ITimelineMessageRepository */
    private $timelineMessageRepository;

    /** @var UpdateTimeline */
    private $updateTimeline;

    /** @var MessageId */
    private $messageId;

    /** @var string */
    private $messageContent;

    public function setup()
    {
        $this->projectionStore = new InMemoryProjectionStore();
        $this->timelineMessageRepository = new TimelineMessageRepository($this->projectionStore);
        $this->messageId = MessageId::generate();
        $this->messageContent = 'Hello';
        $messageProjection = new MessageProjection($this->messageId, $this->messageContent);
        $projectionStore = new InMemoryProjectionStore(array($this->messageId->getId() => $messageProjection));
        $messageProjectionRepository = new MessageProjectionRepository($projectionStore);
        $this->updateTimeline = new UpdateTimeline($this->timelineMessageRepository, $messageProjectionRepository);
    }

    public function testWhenHandleMessagePublished_ThenTimelineMessageIsSavedForAuthor()
    {
        $messagePublished = new MessagePublished(MessageId::generate(), 'hello', new UserId('clem@mix-it.fr'));

        $this->updateTimeline->handleMessagePublished($messagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessageId = new TimelineMessageId($messagePublished->getMessageId(), $messagePublished->getAuthorId());
        $timelineMessage = $this->projectionStore->get($timelineMessageId->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($messagePublished->getMessageId());
        \Assert\that($timelineMessage->getOwnerId())->eq($messagePublished->getAuthorId());
        \Assert\that($timelineMessage->getContent())->eq($messagePublished->getContent());
    }

    public function testWhenHandleReplyMessagePublished_ThenTimelineMessageIsSavedForReplier()
    {
        $replyMessagePublished = new ReplyMessagePublished(MessageId::generate(), 'hello too', new UserId('clem@mix-it.fr'), MessageId::generate());

        $this->updateTimeline->handleReplyMessagePublished($replyMessagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessageId = new TimelineMessageId($replyMessagePublished->getReplyId(), $replyMessagePublished->getReplierId());
        $timelineMessage = $this->projectionStore->get($timelineMessageId->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($replyMessagePublished->getReplyId());
        \Assert\that($timelineMessage->getOwnerId())->eq($replyMessagePublished->getReplierId());
        \Assert\that($timelineMessage->getContent())->eq($replyMessagePublished->getReplyContent());
    }

    public function testWhenHandleMessageRepublished_ThenTimelineMessageNbRepublishIsUpdated()
    {
        $ownerId = new UserId('clem@mix-it.fr');
        $existingTimelineMessage = new TimelineMessage(MessageId::generate(), 'hello', $ownerId);
        $timelineMessageId = new TimelineMessageId($existingTimelineMessage->getMessageId(), $existingTimelineMessage->getOwnerId());
        $this->projectionStore->store($timelineMessageId->getId(), $existingTimelineMessage);
        $messageRepublished = new MessageRepublished($existingTimelineMessage->getMessageId(), new UserId('florent@mix-it.fr'));

        $this->updateTimeline->handleMessageRepublished($messageRepublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessageId = new TimelineMessageId($messageRepublished->getMessageId(), $ownerId);
        $timelineMessage = $this->projectionStore->get($timelineMessageId->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getNbRepublish())->eq(1);
    }

    public function testWhenHandleFolloweeMessagePublished_ThenTimelineMessageIsSavedForFollower()
    {
        $followeeMessagePublished = new FolloweeMessagePublished($this->messageId, new SubscriptionId(new UserId('clem@mix-it.fr'), new UserId('florent@mix-it.fr')));

        $this->updateTimeline->handleFolloweeMessagePublished($followeeMessagePublished);

        /** @var TimelineMessage $timelineMessage */
        $timelineMessageId = new TimelineMessageId($followeeMessagePublished->getMessageId(), $followeeMessagePublished->getSubscriptionId()->getFollowerId());
        $timelineMessage = $this->projectionStore->get($timelineMessageId->getId(), 'App\Domain\Timeline\TimelineMessage');
        \Assert\that($timelineMessage->getMessageId())->eq($this->messageId);
        \Assert\that($timelineMessage->getOwnerId())->eq($followeeMessagePublished->getSubscriptionId()->getFollowerId());
        \Assert\that($timelineMessage->getContent())->eq($this->messageContent);
    }
}