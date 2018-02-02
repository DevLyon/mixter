<?php

namespace App\Domain\Subscriptions;

use App\Domain\IDomainEvent;

class UserUnfollowed implements IDomainEvent
{
    private $id;

    public function __construct(SubscriptionId $id)
    {
        $this->id = $id;
    }

    public function getAggregateId()
    {
        // TODO: Implement getAggregateId() method.
    }

    public function getSubscriptionId(): SubscriptionId
    {
        return $this->id;
    }
}
