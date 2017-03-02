package domain

trait EventPublisher {
  def publish(messageQuacked:Event): Unit
}
