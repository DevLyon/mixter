<?php

namespace App\Domain;

interface IEventPublisher {

    public function publish(IDomainEvent $event);
}