package domain.identity

trait SessionRepository {
  def save(sessionProjection: SessionProjection):Unit
  def replaceBy(sessionProjection: SessionProjection):Unit
}
