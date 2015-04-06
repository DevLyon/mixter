<?php

namespace App\Domain\Identity;

class UserId {
    private $id;

    public function __construct($id) {
        $this->id = $id;
    }
}