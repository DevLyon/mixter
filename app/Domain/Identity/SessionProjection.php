<?php

namespace App\Domain\Identity;

class SessionProjection implements \JsonSerializable
{

    /**
     * @var SessionId
     */
    private $sessionId;

    /**
     * @var UserId
     */
    private $userId;

    public function __construct(UserId $userId, SessionId $sessionId)
    {
        $this->sessionId = $sessionId;
        $this->userId = $userId;
    }

    /**
     * @return SessionId
     */
    public function getSessionId()
    {
        return $this->sessionId;
    }

    /**
     * @return UserId
     */
    public function getUserId()
    {
        return $this->userId;
    }

    /**
     * (PHP 5 &gt;= 5.4.0)<br/>
     * Specify data which should be serialized to JSON
     * @link http://php.net/manual/en/jsonserializable.jsonserialize.php
     * @return mixed data which can be serialized by <b>json_encode</b>,
     * which is a value of any type other than a resource.
     */
    function jsonSerialize()
    {
        return [
            'userId' => $this->userId->getId(),
            'sessionId' => $this->sessionId->getId()
        ];
    }
}