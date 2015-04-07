<?php

namespace App\Infrastructure;

interface IProjectionStore {

    public function get($id, $projectionType);

    public function store($id, $projection);

    public function getAll($projectionType);
}