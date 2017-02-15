package domain.identity

case class SessionId(value:String){
  require(value != null, "A session id cannot be null")
  require(value.nonEmpty, "A session id cannot be empty")
}