<?php

namespace App\Infrastructure;

interface IProjectionStore {

    public function get($id);

    public function store($id, $projection);
}