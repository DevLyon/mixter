<?php

namespace App\Domain\Messages;

class MessageId
{
    /** @var string */
    private $id;

    /**
     * @return MessageId
     */
    public static function generate()
    {
        return new MessageId(uniqid());
    }

    /**
     * @param string $id
     */
    public function __construct($id)
    {
        $this->id = $id;
    }

    /**
     * @return string
     */
    public function getId()
    {
        return $this->id;
    }
}