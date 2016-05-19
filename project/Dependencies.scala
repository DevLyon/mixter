import sbt._

object Dependencies {
  val scalactic = "org.scalactic" %% "scalactic" % Versions.ScalaTest % "test"
  val scalatest = "org.scalatest" %% "scalatest" % Versions.ScalaTest % "test"

  val commonDependencies = List(
    scalactic
    , scalatest
  )
}