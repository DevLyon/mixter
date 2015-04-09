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

    public function quack()
    {
        // DEBT : should be found through session -> later
        $authorId = new UserId(Input::get('userId'));

        $messageContent = Input::get('content');
        Message::quack($this->eventPublisher, $messageContent, $authorId);
        return response('Message published', 201);
    }

    public function requack(IMessageRepository $messageRepository)
    {
        // DEBT : should be found through session -> later
        $requackerId = new UserId(Input::get('userId'));

        $messageToRequackId = new MessageId(Input::get('messageId'));
        try {
            $message = $messageRepository->get($messageToRequackId);
            $message->requack($this->eventPublisher, $requackerId);
            return response('Message '.$messageToRequackId->getId().' requacked', 201);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('Message to requack does not exist', 401);
        }
    }

    public function delete(IMessageRepository $messageRepository)
    {
        // DEBT : should be found through session -> later
        $replierId = new UserId(Input::get('userId'));

        $messageToReplyId = new MessageId(Input::get('messageId'));
        try {
            $message = $messageRepository->get($messageToReplyId);
            $message->delete($this->eventPublisher, $replierId);
            return response('Message '.$messageToReplyId->getId().' deleted', 200);
        } catch (UnknownAggregate $unknownAggregate) {
            return response('Message to republish does not exist', 401);
        }
    }
}