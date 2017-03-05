package mixter.domain.identity

import mixter.domain.AggregateId

case class UserId(email: String) extends AggregateId

object UserId {
  def of(email: String): Option[UserId] =
    Option(email).filter(_.nonEmpty).map(s=>UserId(s))
}
