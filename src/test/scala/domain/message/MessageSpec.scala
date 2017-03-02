package domain.message

import domain.SpyEventPublisher
import domain.message.event.{MessageQuacked, MessageRequacked}
import org.scalatest.{Matchers, WordSpec}

class MessageSpec extends WordSpec with Matchers {
  "Message" should{
    "raise MessageQuacked when quacked" in {
      val message="a message"
      val author=UserId("john@example.com")
      val eventPublisher=new SpyEventPublisher()
      val expectedId = MessageId("id")
      val messageIdGen = () => expectedId

      Message.quack(message, author)(messageIdGen, eventPublisher)

      val expected = MessageQuacked(expectedId, message, author)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
    "raise MessageRequacked when requacked" in {
      val message="a message"
      val author=UserId("john@example.com")
      val requacker=UserId("jane@example.com")
      val messageId = MessageId("id")
      val history = MessageQuacked(messageId, message, author)
      implicit val eventPublisher=new SpyEventPublisher()

      Message(history).requack(requacker)

      val expected = MessageRequacked(messageId, requacker)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
  }
}







