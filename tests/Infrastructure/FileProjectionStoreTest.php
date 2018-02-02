<?php

namespace Tests\Infrastructure;

use App\Infrastructure\FileProjectionStore;
use Illuminate\Contracts\Filesystem\Filesystem;
use Illuminate\Filesystem\FilesystemAdapter;
use League\Flysystem\Adapter\Local;

class FileProjectionStoreTest extends \PHPUnit_Framework_TestCase
{

    /** @var Filesystem */
    private $filesystem;

    /** @var FileProjectionStore */
    private $projectionStore;

    public function setUp()
    {
        $this->filesystem = new FilesystemAdapter(new \League\Flysystem\Filesystem(new Local(__DIR__ . '/../../storage/tests')));
        $this->filesystem->delete($this->filesystem->allFiles());
        $this->projectionStore = new FileProjectionStore($this->filesystem);
    }

    public function tearDown()
    {
        $this->filesystem->delete($this->filesystem->allFiles());
    }

    public function testGivenProjectionDoesNotExist_WhenStore_ThenFileIsCreatedWithProjectionSerialized()
    {
        $fakeProjection = new FakeProjection('someId');

        $this->projectionStore->store($fakeProjection->getId(), $fakeProjection);

        $path = (new \ReflectionClass($fakeProjection))->getShortName() . DIRECTORY_SEPARATOR . $fakeProjection->getId();
        \Assert\that($this->filesystem->exists($path))->true();
    }

    public function testGivenProjectionExists_WhenGet_ThenReturnProjection()
    {
        $fakeProjection = new FakeProjection('someId');
        $this->projectionStore->store($fakeProjection->getId(), $fakeProjection);

        $projection = $this->projectionStore->get($fakeProjection->getId(), get_class($fakeProjection));

        \Assert\that($projection)->eq($fakeProjection);
    }

    public function testGivenProjectionDoesNotExist_WhenGet_ThenReturnNull()
    {
        $projection = $this->projectionStore->get('doesNotExist', 'Tests\Infrastructure\FakeProjection');

        \Assert\that($projection)->nullOr();
    }

    public function testGivenTwoProjectionsExist_WhenGetAll_ThenReturnTwoProjections()
    {
        $this->projectionStore->store('1', new FakeProjection('1'));
        $this->projectionStore->store('2', new FakeProjection('2'));

        $projections = $this->projectionStore->getAll('Tests\Infrastructure\FakeProjection');

        \Assert\that($projections)->count(2);
    }

    public function testGivenAProjectionExists_WhenRemoveIt_ThenFileIsRemoved() {
        $fakeProjection = new FakeProjection('someId');
        $this->projectionStore->store($fakeProjection->getId(), $fakeProjection);

        $this->projectionStore->remove($fakeProjection->getId(), 'Tests\Infrastructure\FakeProjection');

        $path = (new \ReflectionClass($fakeProjection))->getShortName() . DIRECTORY_SEPARATOR . $fakeProjection->getId();
        \Assert\that($this->filesystem->exists($path))->false();
    }
}

class FakeProjection
{
    private $id;

    public function __construct($id)
    {
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