<?php

namespace App\Infrastructure\Subscriptions;

use App\Domain\Subscriptions\ISubscriptionRepository;
use App\Domain\Subscriptions\Subscription;
use App\Domain\Subscriptions\SubscriptionId;
use App\Infrastructure\IEventStore;

class SubscriptionRepository implements ISubscriptionRepository
{
    /**
     * @var IEventStore
     */
    private $eventStore;

    public function __construct(IEventStore $eventStore)
    {
        $this->eventStore = $eventStore;
    }

    /**
     * @param SubscriptionId $subscriptionId
     * @return Subscription
     */
    public function get(SubscriptionId $subscriptionId)
    {
        return new Subscription($this->eventStore->getEvents($subscriptionId->getId()));
    }
}