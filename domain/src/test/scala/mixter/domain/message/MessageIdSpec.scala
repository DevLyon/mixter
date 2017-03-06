package mixter.domain.message

import org.scalatest.wordspec.AnyWordSpec
import org.scalatest.matchers.should.Matchers

class MessageIdSpec extends AnyWordSpec with Matchers {
  "Two generated MessageId" should{
    "not be equal" in {
      val lhs = MessageId.generate()
      val rhs = MessageId.generate()
      lhs should !==(rhs)
    }
  }
  "Two MessageId for the same value" should{
    "be equal" in {
      val lhs = MessageId(VALUE)
      val rhs = MessageId(VALUE)
      lhs should ===(rhs)
    }
  }

  "A MessageId" should {
    "should return its value when serialized to a string" in {
      val id = MessageId(VALUE)

      id.toString should ===(VALUE)
    }
  }
  private val VALUE ="value"
}
