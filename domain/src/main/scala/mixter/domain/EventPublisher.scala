package mixter.domain

trait EventPublisher {
  def publish[T<:Event](event:T): Unit
}
