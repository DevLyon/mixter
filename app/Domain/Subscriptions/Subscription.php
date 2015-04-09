<?php

namespace App\Domain\Subscriptions {

    use App\Domain\Identity\UserId;
    use App\Domain\IEventPublisher;
    use App\Domain\Messages\MessageId;
    use App\Domain\Subscriptions\Subscription\DecisionProjection;

    class Subscription
    {
        /** @var DecisionProjection */
        private $decisionProjection;

        public static function followUser(IEventPublisher $eventPublisher, UserId $followerId, UserId $followeeId)
        {
            $eventPublisher->publish(new UserFollowed(new SubscriptionId($followerId, $followeeId)));
        }

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->decisionProjection = new DecisionProjection($events);
        }

        public function unfollow(IEventPublisher $eventPublisher)
        {
            $eventPublisher->publish(new UserUnfollowed($this->decisionProjection->getSubscriptionId()));
        }

        public function notifyFollower(IEventPublisher $eventPublisher, MessageId $messageId)
        {
            $eventPublisher->publish(new FolloweeMessageQuacked($messageId, $this->decisionProjection->getSubscriptionId()));
        }
    }
}

namespace App\Domain\Subscriptions\Subscription {

    use App\Domain\DecisionProjectionBase;
    use App\Domain\Subscriptions\SubscriptionId;
    use App\Domain\Subscriptions\UserFollowed;

    class DecisionProjection extends DecisionProjectionBase
    {
        /** @var SubscriptionId */
        private $subscriptionId;

        /**
         * @param array $events
         */
        public function __construct($events)
        {
            $this->registerUserFollowed();
            parent::__construct($events);
        }

        /**
         * @return SubscriptionId
         */
        public function getSubscriptionId()
        {
            return $this->subscriptionId;
        }

        private function registerUserFollowed()
        {
            $this->register('App\Domain\Subscriptions\UserFollowed', function (UserFollowed $event) {
                $this->subscriptionId = $event->getSubscriptionId();
            });
        }
    }
}