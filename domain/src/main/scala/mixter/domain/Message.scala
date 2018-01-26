package mixter.domain

object Message {
  def quack(message: String, author: UserId): MessageQuacked =
    MessageQuacked(message, author)

  private[domain] def from(event: MessageQuacked) =
    new Message(event)
}

class Message private[domain](event: MessageQuacked) {

  private val projection = DecisionProjection(event)

  def requack(requacker: UserId): Option[MessageRequacked] = {
    if (projection.author == requacker) {
      None
    } else {
      Some(MessageRequacked(requacker))
    }
  }

  private object DecisionProjection {
    def apply(initialEvent: MessageQuacked): DecisionProjection =
      DecisionProjection(initialEvent.author)
  }

  private case class DecisionProjection(author: UserId)

}