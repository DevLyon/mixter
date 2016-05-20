package domain.identity

case class UserId(email: String)

object UserId {
  def of(email: String): Option[UserId] =
    Option(email).filter(_.nonEmpty).map(s=>UserId(s))
}
