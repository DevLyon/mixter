package domain.identity

import org.scalatest.{Matchers, WordSpec}

class SessionIdSpec extends WordSpec with Matchers {
  "Creating 2 SessionIds from the same string" should {
    "make them equal" in {
      val string="id"
      val id1 = SessionId(string)
      val id2 = SessionId(string)
      id1 should equal(id2)
    }
  }
  "A SessionId" should {
    "raise an IllegalArgumentException when created from an empty string" in {
      an[IllegalArgumentException] should be thrownBy{
        SessionId("")
      }
    }
    "raise an IllegalArgumentException when created from null" in {
      an[IllegalArgumentException] should be thrownBy{
        SessionId(null)
      }
    }
    "be serialized to its value" in {
      val string="id"
      val id1 = SessionId(string)
      id1.toString should be(string)
    }
  }
  "Creating a session id" should{
    "have a random default value" in {
      val id1 = SessionId()
      val id2 = SessionId()
      id1 should not equal(id2)
    }
  }
}
