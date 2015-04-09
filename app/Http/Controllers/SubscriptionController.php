<?php

namespace App\Http\Controllers;

use App\Domain\Identity\UserId;
use App\Domain\IEventPublisher;
use App\Domain\Subscriptions\ISubscriptionRepository;
use App\Domain\Subscriptions\Subscription;
use App\Domain\Subscriptions\SubscriptionId;
use App\Domain\UnknownAggregate;
use Illuminate\Support\Facades\Input;

class SubscriptionController extends Controller
{
    /**
     * @var IEventPublisher
     */
    private $eventPublisher;

    public function __construct(IEventPublisher $eventPublisher)
    {
        $this->eventPublisher = $eventPublisher;
    }

    public function follow()
    {
        $followeeId = new UserId(Input::get('followeeId'));
        $followerId = new UserId(Input::get('followerId'));

        Subscription::followUser($this->eventPublisher, $followerId, $followeeId);
        return response('User followed', 201);
    }

    public function unfollow(ISubscriptionRepository $subscriptionRepository)
    {
        $followeeId = new UserId(Input::get('followeeId'));
        $followerId = new UserId(Input::get('followerId'));
        $subscriptionId = new SubscriptionId($followerId, $followeeId);

        try {
            $subscription = $subscriptionRepository->get($subscriptionId);
            $subscription->unfollow($this->eventPublisher);
            return response('User unfollowed', 200);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('Subscription does not exist', 401);
        }
    }
}