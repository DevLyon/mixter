<?php namespace App\Http\Providers;

use Illuminate\Support\ServiceProvider;

class AppServiceProvider extends ServiceProvider {

	/**
	 * Bootstrap any application services.
	 *
	 * @return void
	 */
	public function boot()
	{
        $this->app->bind('App\Domain\IEventPublisher', 'App\Infrastructure\EventPublisher');
		$this->app->bind('App\Infrastructure\IEventStore', 'App\Infrastructure\FileEventStore');
        $this->app->bind('App\Infrastructure\IProjectionStore', 'App\Infrastructure\FileProjectionStore');
        $this->app->bind('App\Domain\Identity\ISessionProjectionRepository', 'App\Infrastructure\Identity\SessionProjectionRepository');
        $this->app->bind('App\Domain\Identity\ISessionRepository', 'App\Infrastructure\Identity\SessionRepository');
        $this->app->bind('App\Domain\Messages\IMessageRepository', 'App\Infrastructure\Messages\MessageRepository');
        $this->app->bind('App\Domain\Timeline\ITimelineMessageRepository', 'App\Infrastructure\Timeline\TimelineMessageRepository');
        $this->app->bind('App\Domain\Subscriptions\ISubscriptionRepository', 'App\Infrastructure\Subscriptions\SubscriptionRepository');
	}

	/**
	 * Register any application services.
	 *
	 * @return void
	 */
	public function register()
	{
		//
	}

}
