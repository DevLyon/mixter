package mixter.domain

import org.scalatest.{Matchers, WordSpec}

class MessageSpec extends WordSpec with Matchers {
  "A Message" should {
    "raise MessageQuacked when quacked" in {
      val message = "a message"
      val author = UserId("john@example.com")

      val quacked = Message.quack(message, author)

      quacked should equal(MessageQuacked(message, author))
    }
  }
}
