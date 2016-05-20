package domain.identity

case class UserId(email: String)

object UserId {
  def of(email: String): Option[UserId] = Some(UserId(email))
}
