package mixter.domain

import org.scalatest.{Matchers, WordSpec}

class MessageSpec extends WordSpec with Matchers {

  val John = UserId("john@example.com")
  val Jane = UserId("jane@example.com")

  val MessageContent = "a message"
  "A Message" should {
    "raise MessageQuacked when quacked" in {
      val message = "a message"
      val author = UserId("john@example.com")

      val quacked = Message.quack(message, author)

      quacked should equal(MessageQuacked(message, author))
    }
    "raise MessageRequacked when requacked" in {
      val requacker=UserId("jane@example.com")
      val message = "a message"
      val author = UserId("john@example.com")
      val history =  MessageQuacked(message, author)

      val requacked =  Message.from(history).requack(requacker)

      val expected = Some(MessageRequacked(requacker))
      requacked should equal(expected)
    }
  }
}
