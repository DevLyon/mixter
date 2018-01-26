package mixter.domain

object Message {
  def quack(message: String, author: UserId): MessageQuacked =
    MessageQuacked(message, author)

  private[domain] def from(event: MessageQuacked, messageRequacked: MessageRequacked*) =
    new Message(event, messageRequacked: _*)
}

class Message private[domain](event: MessageQuacked, messageRequacked: MessageRequacked*) {

  private val projection =
    messageRequacked.foldLeft(DecisionProjection(event))((projection, event) =>
      projection(event)
    )

  def requack(requacker: UserId): Option[MessageRequacked] = requacker match {
    case projection.author => None
    case _ if projection.hasRequacked(requacker) => None
    case _ => Some(MessageRequacked(requacker))
  }

  private object DecisionProjection {
    def apply(initialEvent: MessageQuacked): DecisionProjection =
      DecisionProjection(initialEvent.author)
  }

  case class DecisionProjection(author: UserId, requackers: Set[UserId] = Set.empty) {
    def apply(messageRequacked: MessageRequacked): DecisionProjection = {
      copy(requackers = requackers + messageRequacked.requacker)
    }

    def hasRequacked(requacker: UserId): Boolean = {
      requackers.contains(requacker)
    }
  }

}