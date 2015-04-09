<?php

namespace App\Infrastructure\Timeline;

use App\Domain\Messages\MessageId;
use App\Domain\Timeline\ITimelineMessageRepository;
use App\Domain\Timeline\TimelineMessage;
use App\Infrastructure\IProjectionStore;

class TimelineMessageRepository implements ITimelineMessageRepository
{
    const PROJECTION_TYPE = 'App\Domain\Timeline\TimelineMessage';

    /**
     * @var IProjectionStore
     */
    private $projectionStore;

    public function __construct(IProjectionStore $projectionStore)
    {
        $this->projectionStore = $projectionStore;
    }

    public function getByMessageId(MessageId $messageId)
    {
        return $this->projectionStore->get($messageId->getId(), self::PROJECTION_TYPE);
    }

    public function save(TimelineMessage $timelineMessage)
    {
        $this->projectionStore->store($timelineMessage->getMessageId()->getId(), $timelineMessage);
    }

    public function getAll()
    {
        return $this->projectionStore->getAll(self::PROJECTION_TYPE);
    }

    public function getByOwnerId($ownerId)
    {
        return array_filter($this->getAll(), function(TimelineMessage $timelineMessage) use($ownerId) {
            return $timelineMessage->getOwnerId() == $ownerId;
        });
    }
}