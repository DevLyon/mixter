<?php

namespace App\Domain\Identity;

class SessionId {
    private $id;

    public static function generate() {
        return new SessionId(uniqid());
    }

    private function __construct($id) {
        $this->id = $id;
    }
}