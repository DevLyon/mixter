package domain.identity

trait SessionRepository {
  def save(sessionProjection: SessionProjection):Unit
}
