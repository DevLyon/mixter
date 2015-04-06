<?php

namespace App\Domain;

class Message {
    public static function publish(IEventPublisher $eventPublisher, $messageContent) {
        $eventPublisher->publish(new MessagePublished($messageContent));
    }
}