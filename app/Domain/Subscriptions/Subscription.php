<?php

namespace App\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;
use App\Domain\IEventPublisher;

class Subscription
{
    /** @var DecisionProjection */
    private $decisionProjection;

    /**
     * Subscription constructor.
     * @param IDomainEvent[] $events
     */
    public function __construct(array $events)
    {
        $this->decisionProjection = new DecisionProjection($events);
    }

    public static function followUser(IEventPublisher $eventPublisher, UserId $followerId, UserId $followeeId): void
    {
        $eventPublisher->publish(
            new UserFollowed(
                new SubscriptionId($followerId, $followeeId)
            )
        );
    }

    public function unfollow(IEventPublisher $eventPublisher)
    {
        $eventPublisher->publish(
            new UserUnfollowed(
                $this->decisionProjection->subscriptionId()
            )
        );
    }
}
