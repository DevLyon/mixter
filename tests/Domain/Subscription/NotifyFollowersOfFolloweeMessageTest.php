<?php

namespace Tests\Domain\Subscriptions;

use App\Domain\Identity\UserId;
use App\Domain\Messages\MessageId;
use App\Domain\Messages\MessagePublished;
use App\Domain\Messages\MessageRepublished;
use App\Domain\Subscriptions\FolloweeMessagePublished;
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
    /** @var NotifyFollowersOfFolloweeMessage */
    private $notifyFollowers;

    /** @var FakeEventPublisher */
    private $eventPublisher;

    /** @var FollowerProjection */
    private $follower;

    /** @var FollowerProjection */
    private $anotherFollower;

    /** @var UserId */
    private $followeeId;

    public function setUp()
    {
        $this->eventPublisher = new FakeEventPublisher();
        $this->followeeId = new UserId('florent@mix-it.fr');
        $this->follower = new FollowerProjection(new UserId('clem@mix-it.fr'), $this->followeeId);
        $this->anotherFollower = new FollowerProjection(new UserId('emilien@mix-it.fr'), $this->followeeId);

        $followerProjectionRepository = $this->getFollowerProjectionRepository($this->follower, $this->anotherFollower);
        $subscriptionRepository = $this->getSubscriptionRepository($this->follower, $this->anotherFollower);
        $this->notifyFollowers = new NotifyFollowersOfFolloweeMessage($this->eventPublisher, $followerProjectionRepository, $subscriptionRepository);
    }

    public function testGivenSeveralFollowers_WhenHandleMessagePublished_ThenNotifyFollowersOfAuthor()
    {
        $messagePublished = new MessagePublished(MessageId::generate(), 'Hello', $this->followeeId);

        $this->notifyFollowers->handleMessagePublished($messagePublished);

        \Assert\that($this->eventPublisher->events)->count(2);
        /** @var FolloweeMessagePublished $followeeMessagePublished */
        $followeeMessagePublished = $this->eventPublisher->events[0];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($this->follower->getFollowerId());
        $followeeMessagePublished = $this->eventPublisher->events[1];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($this->anotherFollower->getFollowerId());
    }

    public function testGivenSeveralFollowers_WhenHandleMessageRepublished_ThenNotifyFollowersOfRepublisher()
    {
        $messagePublished = new MessageRepublished(MessageId::generate(), $this->followeeId);

        $this->notifyFollowers->handleMessageRepublished($messagePublished);

        \Assert\that($this->eventPublisher->events)->count(2);
        /** @var FolloweeMessagePublished $followeeMessagePublished */
        $followeeMessagePublished = $this->eventPublisher->events[0];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($this->follower->getFollowerId());
        $followeeMessagePublished = $this->eventPublisher->events[1];
        \Assert\that($followeeMessagePublished)->isInstanceOf('App\Domain\Subscriptions\FolloweeMessagePublished');
        \Assert\that($followeeMessagePublished->getSubscriptionId()->getFollowerId())->eq($this->anotherFollower->getFollowerId());
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