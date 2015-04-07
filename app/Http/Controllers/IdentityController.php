<?php namespace App\Http\Controllers;

use App\Domain\Identity\ISessionProjectionRepository;
use App\Domain\Identity\ISessionRepository;
use App\Domain\Identity\SessionId;
use App\Domain\Identity\UserId;
use App\Domain\Identity\UserIdentity;
use App\Domain\IEventPublisher;
use App\Domain\UnknownAggregate;
use App\Infrastructure\Identity\UserIdentityRepository;
use Illuminate\Support\Facades\Input;
use Illuminate\Support\Facades\Response;

class IdentityController extends Controller
{
    /** @var IEventPublisher */
    private $eventPublisher;

    /** @var UserIdentityRepository */
    private $userIdentityRepository;

    /**
     * Create a new controller instance.
     * @param IEventPublisher $eventPublisher
     * @param UserIdentityRepository $userIdentityRepository
     */
    public function __construct(
        IEventPublisher $eventPublisher,
        UserIdentityRepository $userIdentityRepository)
    {
        $this->eventPublisher = $eventPublisher;
        $this->userIdentityRepository = $userIdentityRepository;
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

    public function logIn()
    {
        $email = Input::get('email');
        try {
            $userIdentity = $this->userIdentityRepository->get(new UserId($email));
            $userIdentity->logIn($this->eventPublisher);
            return response('Logged in', 200);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('User ' . $email . ' not authenticated', 401);
        }
    }

    public function logOut(ISessionRepository $sessionRepository)
    {
        $sessionId = new SessionId(Input::get('sessionId'));
        try {
            $session = $sessionRepository->get($sessionId);
            $session->logOut($this->eventPublisher);
            return response('Logged out', 200);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('Session unknown', 401);
        }
    }

    public function getSessions(ISessionProjectionRepository $sessionRepository)
    {
        return $sessionRepository->getAll();
    }
}
