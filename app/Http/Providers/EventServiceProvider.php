<?php namespace App\Http\Providers;

use Illuminate\Contracts\Events\Dispatcher as DispatcherContract;
use Illuminate\Foundation\Support\Providers\EventServiceProvider as ServiceProvider;

class EventServiceProvider extends ServiceProvider {

	/**
	 * The event handler mappings for the application.
	 *
	 * @var array
	 */
	protected $listen = [
		'App\Domain\*' => [
			'App\Infrastructure\IEventStore@storeEvent',
		],
        'App\Domain\Identity\UserConnected' => [
            'App\Domain\Identity\SessionHandler@handleUserConnected'
        ],
        'App\Domain\Identity\UserDisconnected' => [
            'App\Domain\Identity\SessionHandler@handleUserDisconnected'
        ],
        'App\Domain\Messages\MessagePublished' => [
            'App\Domain\Timeline\UpdateTimeline@handleMessagePublished'
        ],
        'App\Domain\Messages\ReplyMessagePublished' => [
            'App\Domain\Timeline\UpdateTimeline@handleReplyMessagePublished'
        ],
        'App\Domain\Messages\MessageRepublished' => [
            'App\Domain\Timeline\UpdateTimeline@handleMessageRepublished'
        ]
	];

	/**
	 * Register any other events for your application.
	 *
	 * @param  \Illuminate\Contracts\Events\Dispatcher  $events
	 * @return void
	 */
	public function boot(DispatcherContract $events)
	{
		parent::boot($events);

		//
	}

}
