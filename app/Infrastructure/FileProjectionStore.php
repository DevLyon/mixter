<?php

namespace App\Infrastructure;

use Illuminate\Contracts\Filesystem\Filesystem;

class FileProjectionStore implements IProjectionStore {

    /**
     * @var Filesystem
     */
    private $filesystem;

    public function __construct(Filesystem $filesystem) {
        $this->filesystem = $filesystem;
    }

    public function get($id)
    {
        if($this->filesystem->exists($id)) {
            return unserialize($this->filesystem->get($id));
        }
        return null;
    }

    public function store($id, $projection)
    {
        $this->filesystem->put($id, serialize($projection));
    }
}