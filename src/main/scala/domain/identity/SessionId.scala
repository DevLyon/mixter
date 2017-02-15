package domain.identity

import java.util.UUID

case class SessionId(value:String=UUID.randomUUID().toString){
  require(value != null, "A session id cannot be null")
  require(value.nonEmpty, "A session id cannot be empty")

  override val toString: String = value
}