<?php

namespace App\Domain\Identity;

class SessionId {
    /** @var string  */
    private $id;

    /** @return SessionId */
    public static function generate() {
        return new SessionId(uniqid());
    }

    /** @param string $id */
    private function __construct($id) {
        $this->id = $id;
    }

    /** @return string */
    public function getId()
    {
        return $this->id;
    }
}