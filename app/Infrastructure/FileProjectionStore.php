<?php

namespace App\Infrastructure;

use Illuminate\Contracts\Filesystem\Filesystem;

class FileProjectionStore implements IProjectionStore
{

    /**
     * @var Filesystem
     */
    private $filesystem;

    public function __construct(Filesystem $filesystem)
    {
        $this->filesystem = $filesystem;
    }

    public function get($id, $projectionType)
    {
        $path = (new \ReflectionClass($projectionType))->getShortName() . DIRECTORY_SEPARATOR . $id;
        if ($this->filesystem->exists($path)) {
            return unserialize($this->filesystem->get($path));
        }
        return null;
    }

    public function store($id, $projection)
    {
        $this->filesystem->put((new \ReflectionClass($projection))->getShortName() . DIRECTORY_SEPARATOR . $id, serialize($projection));
    }

    public function getAll($projectionType)
    {
        $allProjections = array();
        $path = (new \ReflectionClass($projectionType))->getShortName();
        foreach ($this->filesystem->allFiles($path) as $file) {
            $allProjections[] = unserialize($this->filesystem->get($file));
        }
        return $allProjections;
    }
}