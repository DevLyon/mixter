<?php

namespace App\Domain\Subscriptions;

class UpdateFollowers
{
    /**
     * @var IFollowerProjectionRepository
     */
    private $followerProjectionRepository;

    public function __construct(IFollowerProjectionRepository $followerProjectionRepository)
    {
        $this->followerProjectionRepository = $followerProjectionRepository;
    }

    public function handleUserFollowed(UserFollowed $userFollowed)
    {
        $this->followerProjectionRepository->save(
            new FollowerProjection($userFollowed->getSubscriptionId()->getFollowerId(), $userFollowed->getSubscriptionId()->getFolloweeId()));
    }
}