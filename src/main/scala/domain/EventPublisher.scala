package domain

import domain.message.event.MessageQuacked

trait EventPublisher {
  def publish(messageQuacked:MessageQuacked): Unit
}
