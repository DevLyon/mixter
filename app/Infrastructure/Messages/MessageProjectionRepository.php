<?php

namespace App\Infrastructure\Messages;

use App\Domain\Messages\IMessageProjectionRepository;
use App\Domain\Messages\MessageId;
use App\Infrastructure\IProjectionStore;

class MessageProjectionRepository implements IMessageProjectionRepository
{
    /**
     * @var IProjectionStore
     */
    private $projectionStore;

    public function __construct(IProjectionStore $projectionStore)
    {
        $this->projectionStore = $projectionStore;
    }

    public function getById(MessageId $messageId)
    {
        return $this->projectionStore->get($messageId->getId(), 'App\Domain\Messages\MessageProjection');
    }
}