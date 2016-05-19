scalaVersion in ThisBuild := "2.12.1"
version in ThisBuild := "2.0.0"

lazy val mixter = (project in file(".")).aggregate(
  domain, infra, web
)
lazy val domain = project
lazy val infra = project.dependsOn(domain)
lazy val web = project.dependsOn(domain,infra)



