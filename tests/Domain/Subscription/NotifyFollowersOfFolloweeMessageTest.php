<?php

namespace Tests\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Subscriptions\FollowerProjection;
use App\Domain\Subscriptions\NotifyFollowersOfFolloweeMessage;
use App\Domain\Subscriptions\UserFollowed;
use App\Infrastructure\Subscriptions\FollowerProjectionRepository;
use App\Infrastructure\Subscriptions\SubscriptionRepository;
use Tests\Domain\FakeEventPublisher;
use Tests\Infrastructure\InMemoryEventStore;
use Tests\Infrastructure\InMemoryProjectionStore;

class NotifyFollowersOfFolloweeMessageTest extends \PHPUnit_Framework_TestCase
{
    public function testGivenSeveralFollowers_WhenHandleMessagePublished_ThenNotifyFollowers()
    {
        $eventPublisher = new FakeEventPublisher();
        $followeeId = new UserId('florent@mix-it.fr');
        $follower = new FollowerProjection(new UserId('clem@mix-it.fr'), $followeeId);
        $anotherFollower = new FollowerProjection(new UserId('emilien@mix-it.fr'), $followeeId);

        $followerProjectionRepository = $this->getFollowerProjectionRepository($follower, $anotherFollower);
        $subscriptionRepository = $this->getSubscriptionRepository($follower, $anotherFollower);
        $notifyFollowers = new NotifyFollowersOfFolloweeMessage($eventPublisher, $followerProjectionRepository, $subscriptionRepository);
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $followeeId);

        $notifyFollowers->handleMessagePublished($messagePublished);

        \Assert\that($eventPublisher->events)->count(2);
        /** @var FolloweeMessagePublished $followeeMessagePublished */
        $followeeMessagePublished = $eventPublisher->events[0];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($follower->getFollowerId());
        $followeeMessagePublished = $eventPublisher->events[1];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($anotherFollower->getFollowerId());
    }

    /**
     * @param FollowerProjection $follower
     * @param FollowerProjection $anotherFollower
     * @return FollowerProjectionRepository
     */
    private function getFollowerProjectionRepository(FollowerProjection $follower, FollowerProjection $anotherFollower)
    {
        $projectionStore = new InMemoryProjectionStore(
            array($follower->getSubscriptionId()->getId() => $follower,
                $anotherFollower->getSubscriptionId()->getId() => $anotherFollower));
        $followerProjectionRepository = new FollowerProjectionRepository($projectionStore);
        return $followerProjectionRepository;
    }

    /**
     * @param FollowerProjection $follower
     * @param FollowerProjection $anotherFollower
     * @return SubscriptionRepository
     */
    private function getSubscriptionRepository(FollowerProjection $follower, FollowerProjection $anotherFollower)
    {
        $userFollowed = new UserFollowed($follower->getSubscriptionId());
        $anotherUserFollowed = new UserFollowed($anotherFollower->getSubscriptionId());
        $eventStore = new InMemoryEventStore(array($userFollowed, $anotherUserFollowed));
        $subscriptionRepository = new SubscriptionRepository($eventStore);
        return $subscriptionRepository;
    }
}