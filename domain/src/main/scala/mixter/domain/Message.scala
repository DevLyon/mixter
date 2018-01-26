package mixter.domain

object Message {
  def quack(message: String, author: UserId): MessageQuacked =
    MessageQuacked(message, author)

  private[domain] def from(event: MessageQuacked, messageRequacked: MessageRequacked*) =
    new Message(event, messageRequacked: _*)
}

class Message private[domain](initialEvent: MessageQuacked, messageRequacked: MessageRequacked*) {

  private val projection = DecisionProjection(initialEvent, messageRequacked:_*)


  def requack(requacker: UserId): Option[MessageRequacked] = requacker match {
    case projection.author => None
    case _ if projection.hasRequacked(requacker) => None
    case _ => Some(MessageRequacked(requacker))
  }

  def delete(): Option[MessageDeleted] = {
    Some(MessageDeleted)
  }

  private object DecisionProjection {
    def apply(initialEvent: MessageQuacked, events: MessageRequacked*):DecisionProjection={
      events.foldLeft(DecisionProjection(initialEvent))((projection, event) =>
        projection(event)
      )
    }
    def apply(initialEvent: MessageQuacked):DecisionProjection={
      DecisionProjection(initialEvent.author)
    }
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