package mixter.infra

import mixter.domain.Event

class SpyEventHandler[T<:Event] extends (T => Unit) {
  private var called: Boolean = false

  def apply(event: T): Unit = {
    called = true
  }

  def wasCalled: Boolean = called
}
