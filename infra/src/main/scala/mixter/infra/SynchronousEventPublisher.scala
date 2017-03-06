package mixter.infra

import scala.reflect.ClassTag

import mixter.domain.{Event, EventPublisher}

class SynchronousEventPublisher extends EventPublisher {
  private var handlers: Seq[Event => Unit]= Seq.empty

  def register[T <: Event](handler: T => Unit)(implicit ct: ClassTag[T]) {
    this.handlers = handlers :+ handler.asInstanceOf[Event=>Unit]
  }

  override def publish[T<:Event](event: T): Unit = {
    handlers.foreach(_.asInstanceOf[T=>Unit](event))
  }
}
