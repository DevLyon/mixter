<?php

namespace App\Domain\Subscriptions;

use App\Domain\IDomainEvent;

class UserFollowed implements IDomainEvent
{
    /**
     * @var SubscriptionId
     */
    private $subscriptionId;

    public function __construct(SubscriptionId $subscriptionId)
    {
        $this->subscriptionId = $subscriptionId;
    }

    /**
     * @return SubscriptionId
     */
    public function getSubscriptionId()
    {
        return $this->subscriptionId;
    }

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return $this->getSubscriptionId()->getFolloweeId()->getId().'_'.$this->getSubscriptionId()->getFollowerId()->getId();
    }
}