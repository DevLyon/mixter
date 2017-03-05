package mixter.domain

trait EventPublisher {
  def publish(messageQuacked:Event): Unit
}
