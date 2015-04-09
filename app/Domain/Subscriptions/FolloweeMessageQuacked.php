<?php

namespace App\Domain\Subscriptions;

use App\Domain\IDomainEvent;
use App\Domain\Messages\MessageId;

class FolloweeMessageQuacked implements IDomainEvent
{
    /**
     * @var MessageId
     */
    private $messageId;

    /**
     * @var SubscriptionId
     */
    private $subscriptionId;

    public function __construct(MessageId $messageId, SubscriptionId $subscriptionId)
    {
        $this->messageId = $messageId;
        $this->subscriptionId = $subscriptionId;
    }

    /**
     * @return MessageId
     */
    public function getMessageId()
    {
        return $this->messageId;
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
        return $this->getSubscriptionId()->getId();
    }
}