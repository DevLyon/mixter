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
}