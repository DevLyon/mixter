package mixter.infra

class SpyEventHandler extends (EventA => Unit) {
  private var called: Boolean = false

  def apply(event: EventA): Unit = {
    called = true
  }

  def wasCalled: Boolean = called
}
