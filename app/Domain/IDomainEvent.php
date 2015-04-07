<?php

namespace App\Domain;

interface IDomainEvent {

    /**
     * @return string
     */
    public function getAggregateId();
}