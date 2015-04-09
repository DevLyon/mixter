<?php

namespace Tests\Infrastructure;

use App\Infrastructure\IProjectionStore;

class InMemoryProjectionStore implements IProjectionStore
{
    /** @var array */
    private $projections = array();

    public function __construct($projections = array())
    {
        $this->projections = $projections;
    }

    public function get($id, $projectionType)
    {
        if (array_key_exists($id, $this->projections)) {
            return $this->projections[$id];
        }
        return null;
    }

    public function store($id, $projection)
    {
        $this->projections[$id] = $projection;
    }

    public function getAll($projectionType)
    {
        return array_values($this->projections);
    }

    public function remove($id, $projectionType)
    {
        unset($this->projections[$id]);
    }
}