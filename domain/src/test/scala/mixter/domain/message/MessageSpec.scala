package mixter.domain.message


import mixter.domain.SpyEventPublisherFixture
import mixter.domain.identity.UserId
import mixter.domain.message.event.{MessageDeleted, MessageEvent, MessageQuacked, MessageRequacked}
import org.scalatest.{Matchers, WordSpec}

class MessageSpec extends WordSpec with Matchers with SpyEventPublisherFixture{
  "Message" should{
    "raise MessageQuacked when quacked" in withSpyEventPublisher { implicit eventPublisher=>
      val author=USERID_JOHN
      val messageIdGen = () => MESSAGE_ID

      Message.quack(A_MESSAGE, author)(messageIdGen, eventPublisher)

      val expected = MessageQuacked(MESSAGE_ID, A_MESSAGE, author)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
    "raise MessageRequacked when requacked" in withSpyEventPublisher { implicit eventPublisher=>
      val requacker=USERID_JANE
      val history = A_MESSAGE_BY_JOHN

      Message(history, List.empty).requack(requacker, USERID_JOHN, A_MESSAGE)

      val expected = MessageRequacked(MESSAGE_ID, requacker, USERID_JOHN, A_MESSAGE)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }

    "not raise MessageRequacked when requacked by its author" in withSpyEventPublisher { implicit eventPublisher=>
      val messageQuacked = A_MESSAGE_BY_JOHN
      val history = List.empty[MessageEvent]

      Message(messageQuacked, history).requack(USERID_JOHN,USERID_JOHN, A_MESSAGE)

      eventPublisher.publishedEvents shouldBe empty
    }
    "not raise MessageRequacked when already requacked by requacker" in withSpyEventPublisher { implicit eventPublisher=>
      val requacker=USERID_JANE

      val messageQuacked = A_MESSAGE_BY_JOHN
      val history:List[MessageEvent] = List(
        MessageRequacked(MESSAGE_ID,requacker, USERID_JOHN, A_MESSAGE)
      )

      Message(messageQuacked, history).requack(requacker, USERID_JOHN, A_MESSAGE)

      eventPublisher.publishedEvents shouldBe empty
    }
    "raise a MessageDeleted event when it is deleted by its author" in withSpyEventPublisher{ implicit eventPublisher=>
      // Given
      val history = A_MESSAGE_BY_JOHN
      val message = Message(history, List.empty)

      // When
      message.delete(USERID_JOHN)

      // Then
      val expected = MessageDeleted(MESSAGE_ID)
      eventPublisher.publishedEvents should contain theSameElementsAs Seq(expected)
    }
    "not raise a MessageDeleted event when it is deleted by someone who isn't its author" in withSpyEventPublisher { implicit eventPublisher =>
      // Given
      val history = A_MESSAGE_BY_JOHN
      val message = Message(history, List.empty)

      // When
      message.delete(USERID_JANE)

      // Then
      eventPublisher.publishedEvents shouldBe empty
    }
  }
  private val MESSAGE_ID = MessageId("id")
  private val USERID_JOHN = UserId("john@example.com")
  private val USERID_JANE = UserId("jane@example.com")
  private val A_MESSAGE = "a message"
  private val A_MESSAGE_BY_JOHN = MessageQuacked(MESSAGE_ID, A_MESSAGE, USERID_JOHN)
}







