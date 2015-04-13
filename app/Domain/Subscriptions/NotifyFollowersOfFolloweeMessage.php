<?php

namespace App\Domain\Subscriptions;

use App\Domain\IEventPublisher;
use App\Domain\Messages\MessagePublished;

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
        /** @var FollowerProjection $follower */
        foreach ($this->followerProjectionRepository->getFollowersOf($messagePublished->getAuthorId()) as $follower) {
            $subscription = $this->subscriptionRepository->get($follower->getSubscriptionId());
            $subscription->notifyFollower($this->eventPublisher, $messagePublished->getMessageId());
        }
    }
}