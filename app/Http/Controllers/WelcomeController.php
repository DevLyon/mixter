<?php namespace App\Http\Controllers;

use App\Domain\Identity\UserId;
use App\Domain\Identity\UserIdentity;
use App\Domain\IEventPublisher;
use App\Infrastructure\EventPublisher;
use Illuminate\Support\Facades\Input;
use Illuminate\Support\Facades\Response;

class WelcomeController extends Controller {
    /**
     * @var IEventPublisher
     */
    private $eventPublisher;

    /**
     * Create a new controller instance.
     * @param EventPublisher $eventPublisher
     */
	public function __construct(EventPublisher $eventPublisher)
	{
        $this->eventPublisher = $eventPublisher;
    }

	/**
	 * Show the application welcome screen to the user.
	 *
	 * @return Response
	 */
	public function index()
	{
		return view('welcome');
	}

    public function register()
    {
        $email = Input::get('email');
        UserIdentity::register($this->eventPublisher, new UserId($email));
        return response('', 201);
    }
}
