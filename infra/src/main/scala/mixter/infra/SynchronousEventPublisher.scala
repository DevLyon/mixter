package mixter.infra

import scala.reflect.ClassTag

import mixter.domain.{Event, EventPublisher}

class SynchronousEventPublisher extends EventPublisher {
  private var handlers: Map[Class[_],Seq[Class[_] => Unit]]= Map.empty.withDefaultValue(Seq.empty)

  def register[T <: Event](handler: T => Unit)(implicit ct: ClassTag[T]) {
    val ctHandlers = this.handlers(ct.runtimeClass)
    this.handlers = this.handlers + (ct.runtimeClass -> (ctHandlers :+ handler.asInstanceOf[Class[_]=>Unit]))
  }

  override def publish[T<:Event](event: T): Unit = {
    handlers(event.getClass).foreach(_.asInstanceOf[T=>Unit](event))
  }
}
