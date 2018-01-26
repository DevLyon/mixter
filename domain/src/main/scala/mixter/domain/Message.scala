package mixter.domain

object Message {
  def quack(message: String, author: UserId): MessageQuacked =
    MessageQuacked(message, author)

  private[domain] def from(event: MessageQuacked, messageEvents: MessageEvent*) =
    new Message(event, messageEvents: _*)
}

class Message private[domain](initialEvent: MessageQuacked, messageEvents: MessageEvent*) {

  private val projection = DecisionProjection(initialEvent, messageEvents: _*)


  def requack(requacker: UserId): Option[MessageRequacked] = {
    projection match {
      case QuackedMessage(author, _) if author == requacker => None
      case QuackedMessage(_, requackers) if requackers.contains(requacker) => None
      case _ => Some(MessageRequacked(requacker))
    }
  }

  def delete(): Option[MessageDeleted] = projection match {
    case DeletedMessage => None
    case _ => Some(MessageDeleted)
  }

  private object DecisionProjection {
    def apply(initialEvent: MessageQuacked, events: MessageEvent*): DecisionProjection = {
      events.foldLeft(DecisionProjection(initialEvent))((projection, event) =>
        projection(event)
      )
    }

    def apply(initialEvent: MessageQuacked): DecisionProjection = {
      QuackedMessage(initialEvent.author)
    }
  }

  sealed trait DecisionProjection {
    def apply(event: MessageEvent): DecisionProjection
  }

  case class QuackedMessage(author: UserId, requackers: Set[UserId] = Set.empty) extends DecisionProjection {
    def apply(event: MessageEvent): DecisionProjection = event match {
      case requacked: MessageRequacked => apply(requacked)
      case deleted: MessageDeleted => apply(deleted)
    }

    def apply(messageRequacked: MessageRequacked): DecisionProjection = {
      copy(requackers = requackers + messageRequacked.requacker)
    }

    def apply(deleted: MessageDeleted): DecisionProjection = {
      DeletedMessage
    }
  }

  case object DeletedMessage extends DecisionProjection {
    def apply(event: MessageEvent): DecisionProjection = this

    def hasRequacked(requacker: UserId): Boolean = {
      true
    }
  }

}