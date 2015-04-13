<?php

namespace App\Domain\Subscriptions;

use App\Domain\IEventPublisher;
use App\Domain\Messages\MessageQuacked;

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
        /** @var FollowerProjection $follower */
        foreach ($this->followerProjectionRepository->getFollowersOf($messageQuacked->getAuthorId()) as $follower) {
            $subscription = $this->subscriptionRepository->get($follower->getSubscriptionId());
            $subscription->notifyFollower($this->eventPublisher, $messageQuacked->getMessageId());
        }
    }
}