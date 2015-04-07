<?php

namespace App\Http\Controllers;

use App\Domain\Identity\UserId;
use App\Domain\IEventPublisher;
use App\Domain\Messages\IMessageRepository;
use App\Domain\Messages\Message;
use App\Domain\Messages\MessageId;
use App\Domain\UnknownAggregate;
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
        // DEBT : should be found through session -> later
        $authorId = new UserId(Input::get('userId'));

        $messageContent = Input::get('content');
        Message::publish($this->eventPublisher, $messageContent, $authorId);
        return response('Message published', 201);
    }

    public function republish(IMessageRepository $messageRepository)
    {
        // DEBT : should be found through session -> later
        $republisherId = new UserId(Input::get('userId'));

        $messageToRepublishId = new MessageId(Input::get('messageId'));
        try {
            $message = $messageRepository->get($messageToRepublishId);
            $message->republish($this->eventPublisher, $republisherId);
            return response('Message '.$messageToRepublishId->getId().' republished', 201);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('Message to republish does not exist', 401);
        }
    }
}