package domain.identity

sealed trait SessionStatus
object SessionStatus {
  case object CONNECTED extends SessionStatus
}
