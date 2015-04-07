<?php

namespace App\Http\Controllers;

use App\Domain\IEventPublisher;
use App\Domain\Messages\Message;
use Illuminate\Support\Facades\Input;

class MessageController extends Controller
{
    /** @var IEventPublisher */
    private $eventPublisher;

    public function __construct(IEventPublisher $eventPublisher)
    {
        $this->eventPublisher = $eventPublisher;
    }

    public function publish()
    {
        $messageContent = Input::get('content');
        Message::publish($this->eventPublisher, $messageContent);
    }
}