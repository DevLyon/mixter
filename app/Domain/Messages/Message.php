<?php

namespace App\Domain\Messages;

use App\Domain\IEventPublisher;

class Message {
    public static function publish(IEventPublisher $eventPublisher, $messageContent) {
        $eventPublisher->publish(new MessagePublished($messageContent));
    }
}