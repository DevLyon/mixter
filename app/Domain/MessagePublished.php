<?php

namespace App\Domain;

class MessagePublished implements IDomainEvent {

    /**
     * @var string
     */
    private $content;

    public function __construct($content) {
        $this->content = $content;
    }

    /**
     * @return string
     */
    public function getContent()
    {
        return $this->content;
    }
}