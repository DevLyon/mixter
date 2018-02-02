<?php

namespace App\Domain\Subscriptions;

use App\Domain\DecisionProjectionBase;
use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class DecisionProjection extends DecisionProjectionBase
{
    /** @var SubscriptionId */
    private $subscriptionId;

    /**
     * @param IDomainEvent[] $events
     */
    public function __construct(array $events)
    {
        $this->registerUserFollowed();

        parent::__construct($events);
    }

    private function registerUserFollowed()
    {
        $this->register(UserFollowed::class, function (UserFollowed $event) {
            $this->subscriptionId = $event->getSubscriptionId();
        });
    }

    public function subscriptionId(): SubscriptionId
    {
        return $this->subscriptionId;
    }
}
