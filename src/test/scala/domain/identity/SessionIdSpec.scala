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
}
