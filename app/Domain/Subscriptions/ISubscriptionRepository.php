<?php

namespace App\Domain\Subscriptions;

interface ISubscriptionRepository
{
    /**
     * @param SubscriptionId $subscriptionId
     * @return Subscription
     */
    public function get(SubscriptionId $subscriptionId);
}