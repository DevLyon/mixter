<?php

namespace Tests\Infrastructure\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\Subscriptions\UserFollowed;
use App\Infrastructure\Subscriptions\SubscriptionRepository;
use Tests\Infrastructure\InMemoryEventStore;

class SubscriptionRepositoryTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenAUserHaveFollowedAnother_WhenGetBySubscriptionId_ThenReturnSubscription()
    {
        $userFollowed = new UserFollowed(new SubscriptionId(new UserId('clem@mix-it.fr'), new UserId('clem@mix-it.fr')));
        $eventStore = new InMemoryEventStore(array($userFollowed));
        $subscriptionRepository = new SubscriptionRepository($eventStore);

        $subscription = $subscriptionRepository->get($userFollowed->getSubscriptionId());

        \Assert\that($subscription)->notNull();
    }
}