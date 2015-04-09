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
        'App\Domain\Messages\MessageQuacked' => [
            'App\Domain\Timeline\UpdateTimeline@handleMessageQuacked'
        ],
        'App\Domain\Messages\MessageRequacked' => [
            'App\Domain\Timeline\UpdateTimeline@handleMessageRequacked'
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
