package mixter.domain

sealed trait MessageEvent
case class MessageRequacked(requacker: UserId) extends MessageEvent
sealed trait MessageDeleted  extends MessageEvent
case object MessageDeleted extends MessageDeleted