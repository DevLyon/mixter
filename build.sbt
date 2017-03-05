name := "mixter"

ThisBuild / scalaVersion := "2.13.12"

libraryDependencies ++= Common.libraryDependencies

lazy val domain = (project in file("domain"))

lazy val infra = (project in file("infra"))
	.dependsOn(domain)

lazy val mixter = (project in file("."))
	.aggregate(domain, infra)
