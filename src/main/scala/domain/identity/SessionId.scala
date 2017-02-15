package domain.identity

case class SessionId(value:String){
  require(value.nonEmpty, "A session id cannot be empty")
}
