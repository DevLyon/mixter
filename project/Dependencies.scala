import sbt._

object Dependencies {
  private val scalactic = "org.scalactic" %% "scalactic" % Versions.ScalaTest % "test"
  private val scalatest = "org.scalatest" %% "scalatest" % Versions.ScalaTest % "test"
  private val scalaCheck = "org.scalacheck" %% "scalacheck" % Versions.ScalaCheck % "test"

  val commonDependencies = List(
    scalactic
    , scalatest
    , scalaCheck
  )
}