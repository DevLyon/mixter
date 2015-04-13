<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\IEventPublisher;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessageQuacked;
use App\Domain\Messages\MessageRequacked;

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

    public function handleMessageQuacked(MessageQuacked $messageQuacked)
    {
        $this->notifyFollowersOf($messageQuacked->getAuthorId(), $messageQuacked->getMessageId());
    }

    public function handleMessageRequacked(MessageRequacked $messageRequacked)
    {
        $this->notifyFollowersOf($messageRequacked->getRequackerId(), $messageRequacked->getMessageId());
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