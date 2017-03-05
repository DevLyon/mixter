package mixter.domain.identity

sealed trait SessionStatus
object SessionStatus {
  case object CONNECTED extends SessionStatus
  case object DISCONNECTED extends SessionStatus
}
