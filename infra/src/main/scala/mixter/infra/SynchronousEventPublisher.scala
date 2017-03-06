package mixter.infra

import scala.reflect.ClassTag

import mixter.domain.{Event, EventPublisher}

class SynchronousEventPublisher extends EventPublisher {
  private var handler: Event => Unit = _

  def register[T <: Event](handler: T => Unit)(implicit ct: ClassTag[T]) {
    this.handler = handler.asInstanceOf[Event=>Unit]
  }

  override def publish[T<:Event](event: T): Unit = {
    handler.asInstanceOf[T=>Unit](event)
  }
}
