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
      val requacker = Jane
      val author = John
      val message = quackedMessageBy(John)

      val requacked = message.requack(requacker)

      val expected = Some(MessageRequacked(requacker))
      requacked should equal(expected)
    }

    "raise nothing when author requacks" in {
      val author = John
      val requacker = John
      val message = quackedMessageBy(John)

      val requacked = message.requack(requacker)

      requacked should be(None)
    }

    "raise nothing when requacked twice by the same user" in {
      val requacker = Jane
      val message = Message.from(
        MessageQuacked(MessageContent, John),
        MessageRequacked(requacker)
      )

      val requacked = message.requack(requacker)
      requacked should be(None)
    }

    "raise message deleted when message delete" in {
      val message = quackedMessageBy(John)

      val deleted = message.delete()
      deleted should be(Some(MessageDeleted))
    }
  }

  def quackedMessageBy(author: UserId, message: String = MessageContent): Message = {
    val history = MessageQuacked(MessageContent, author)
    Message.from(history)
  }
}
