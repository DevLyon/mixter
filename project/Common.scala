import sbt._
import sbt.Keys._

object Common{
  val libraryDependencies: Seq[ModuleID] = Seq(
    "org.scalactic" %% "scalactic" % Versions.ScalaTest % "test"
    , "org.scalatest" %% "scalatest" % Versions.ScalaTest % "test"
   )
}