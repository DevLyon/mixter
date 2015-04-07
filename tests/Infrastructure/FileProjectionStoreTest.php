<?php

namespace Tests\Infrastructure;

use App\Infrastructure\FileProjectionStore;
use App\Infrastructure\IProjectionStore;
use Illuminate\Contracts\Filesystem\Filesystem;
use Illuminate\Filesystem\FilesystemAdapter;
use League\Flysystem\Adapter\Local;

class FileProjectionStoreTest extends \PHPUnit_Framework_TestCase {

    /** @var Filesystem */
    private $filesystem;

    /** @var IProjectionStore */
    private $projectionStore;

    public function setUp() {
        $this->filesystem = new FilesystemAdapter(new \League\Flysystem\Filesystem(new Local(__DIR__.'/../../storage/tests/fakeProjection')));
        $this->filesystem->delete($this->filesystem->allFiles());
        $this->projectionStore = new FileProjectionStore($this->filesystem);
    }

    public function testGivenProjectionDoesNotExist_WhenStore_ThenFileIsCreatedWithProjectionSerialized() {
        $fakeProjection = new FakeProjection('someId');

        $this->projectionStore->store($fakeProjection->getId(), $fakeProjection);

        \Assert\that($this->filesystem->exists($fakeProjection->getId()))->true();
    }

    public function testGivenProjectionExists_WhenGet_ThenReturnProjection() {
        $fakeProjection = new FakeProjection('someId');
        $this->projectionStore->store($fakeProjection->getId(), $fakeProjection);

        $projection = $this->projectionStore->get($fakeProjection->getId());

        \Assert\that($projection)->eq($fakeProjection);
    }

    public function testGivenProjectionDoesNotExist_WhenGet_ThenReturnNull() {
        $projection = $this->projectionStore->get('doesNotExist');

        \Assert\that($projection)->nullOr();
    }
}

class FakeProjection {
    private $id;

    public function __construct($id) {
        $this->id = $id;
    }

    /**
     * @return mixed
     */
    public function getId()
    {
        return $this->id;
    }
}