<?php
namespace App\Domain\Timeline;

use App\Domain\Messages\MessageId;

interface ITimelineMessageRepository
{
    /**
     * @param MessageId $messageId
     * @return TimelineMessage
     */
    public function getByMessageId(MessageId $messageId);

    /**
     * @param TimelineMessage $timelineMessage
     */
    public function save(TimelineMessage $timelineMessage);

    /**
     * @return array of TimelineMessage
     */
    public function getAll();

    public function getByOwnerId($ownerId);
}