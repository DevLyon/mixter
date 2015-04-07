<?php

namespace App\Domain\Identity;

class UserId {
    /** @var string */
    private $id;

    /**
     * @param string $id
     */
    public function __construct($id) {
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