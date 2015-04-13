<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\IEventPublisher;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Messages\ReplyMessagePublished;

class NotifyFollowersOfFolloweeMessage
{
    /**
     * @var IEventPublisher
     */
    private $eventPublisher;

    /**
     * @var IFollowerProjectionRepository
     */
    private $followerProjectionRepository;

    /**
     * @var ISubscriptionRepository
     */
    private $subscriptionRepository;

    public function __construct(
        IEventPublisher $eventPublisher,
        IFollowerProjectionRepository $followerProjectionRepository,
        ISubscriptionRepository $subscriptionRepository)
    {
        $this->eventPublisher = $eventPublisher;
        $this->followerProjectionRepository = $followerProjectionRepository;
        $this->subscriptionRepository = $subscriptionRepository;
    }

    public function handleMessagePublished(MessagePublished $messagePublished)
    {
        $this->notifyFollowersOf($messagePublished->getAuthorId(), $messagePublished->getMessageId());
    }

    public function handleMessageRepublished(MessageRepublished $messageRepublished)
    {
        $this->notifyFollowersOf($messageRepublished->getRepublisherId(), $messageRepublished->getMessageId());
    }

    public function handleReplyMessagePublished(ReplyMessagePublished $replyMessagePublished)
    {
        $this->notifyFollowersOf($replyMessagePublished->getReplierId(), $replyMessagePublished->getReplyId());
    }

    private function notifyFollowersOf(UserId $followeeId, MessageId $messageId)
    {
        /** @var FollowerProjection $follower */
        foreach ($this->followerProjectionRepository->getFollowersOf($followeeId) as $follower) {
            $subscription = $this->subscriptionRepository->get($follower->getSubscriptionId());
            $subscription->notifyFollower($this->eventPublisher, $messageId);
        }
    }
}