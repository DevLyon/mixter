<?php

namespace Tests\Infrastructure;

use App\Infrastructure\IProjectionStore;

class InMemoryProjectionStore implements IProjectionStore {

    /** @var array */
    private $projections;

    public function get($id, $projectionType)
    {
        return $this->projections[$id];
    }

    public function store($id, $projection)
    {
        $this->projections[$id] = $projection;
    }

    public function getAll($projectionType)
    {
        return array_values($this->projections);
    }
}