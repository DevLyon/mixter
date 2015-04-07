<?php

namespace App\Domain\Messages;

use App\Domain\Identity\UserId;
use App\Domain\IDomainEvent;

class ReplyMessagePublished implements IDomainEvent
{
    /**
     * @var MessageId
     */
    private $replyId;

    /**
     * @var string
     */
    private $replyContent;

    /**
     * @var UserId
     */
    private $replierId;

    /**
     * @var MessageId
     */
    private $parentMessageId;

    public function __construct(MessageId $replyId, $replyContent, UserId $replierId, MessageId $parentMessageId)
    {
        $this->replyId = $replyId;
        $this->replyContent = $replyContent;
        $this->replierId = $replierId;
        $this->parentMessageId = $parentMessageId;
    }

    /**
     * @return MessageId
     */
    public function getReplyId()
    {
        return $this->replyId;
    }

    /**
     * @return string
     */
    public function getReplyContent()
    {
        return $this->replyContent;
    }

    /**
     * @return UserId
     */
    public function getReplierId()
    {
        return $this->replierId;
    }

    /**
     * @return MessageId
     */
    public function getParentMessageId()
    {
        return $this->parentMessageId;
    }

    /**
     * @return string
     */
    public function getAggregateId()
    {
        return $this->getReplyId();
    }
}