package mixter.domain

object Message {
  def quack(message: String, author: UserId): MessageQuacked =
    MessageQuacked(message, author)

}
