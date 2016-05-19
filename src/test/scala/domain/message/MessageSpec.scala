package domain.message

import domain.message.event.MessageQuacked
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
  }
}







