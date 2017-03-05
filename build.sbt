name := "mixter"

scalaVersion in ThisBuild := "2.12.1"

libraryDependencies ++= Common.libraryDependencies

lazy val domain = (project in file("domain"))

lazy val infra = (project in file("infra"))
	.dependsOn(domain)

lazy val mixter = (project in file("."))
	.aggregate(domain, infra)
