package mixter.domain.message


import mixter.domain.SpyEventPublisher
import mixter.domain.identity.UserId
import mixter.domain.message.event.{MessageEvent, MessageQuacked, MessageRequacked}
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

      Message(history, List.empty).requack(requacker, author, message)

      val expected = MessageRequacked(messageId, requacker, author, message)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }

    "not raise MessageRequacked when requacked by its author" in {
      val message="a message"
      val author=UserId("john@example.com")
      val requacker=author
      val messageId = MessageId("id")
      implicit val eventPublisher=new SpyEventPublisher()

      val messageQuacked = MessageQuacked(messageId, message, author)
      val history = List.empty[MessageEvent]


      val requacked = Message(messageQuacked, history).requack(requacker,author, message)

      eventPublisher.publishedEvents shouldBe empty
    }
    "not raise MessageRequacked when already requacked by requacker" in {
      val message="a message"
      val messageId = MessageId("id")
      val author=UserId("john@example.com")
      val requacker=UserId("jane@example.com")
      implicit val eventPublisher=new SpyEventPublisher()

      val messageQuacked = MessageQuacked(messageId, message, author)
      val history:List[MessageEvent] = List(
        MessageRequacked(messageId,requacker, author, message)
      )

      val requacked = Message(messageQuacked, history).requack(requacker, author, message)

      eventPublisher.publishedEvents shouldBe empty
    }
  }
}







