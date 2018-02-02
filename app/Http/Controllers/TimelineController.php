<?php

namespace App\Http\Controllers;

use App\Domain\Identity\UserId;
use App\Domain\Timeline\ITimelineMessageRepository;

class TimelineController extends Controller
{
    public function getAllMessages(ITimelineMessageRepository $timelineMessageRepository)
    {
        return $timelineMessageRepository->getAll();
    }

    public function getUserMessages($userId, ITimelineMessageRepository $timelineMessageRepository)
    {
        return $timelineMessageRepository->getByOwnerId(new UserId($userId));
    }
}