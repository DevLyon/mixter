package mixter.doc

import scala.annotation.StaticAnnotation

/**
  * A repository mediates between the domain and data mapping layers using a collection-like interface for accessing
  * domain objects.
  *
  * In the exercise there are multiple repositories, they can be backed by different stores. Some are backed by event
  * stores, other may be backed by an SQL database. The purpose of the repository is to protect the domain model from
  * the dirty details of persistence as much as possible.
  *
  * ==Further references:==
  * - http://martinfowler.com/eaaCatalog/repository.html
  * - https://msdn.microsoft.com/en-us/library/ff649690.aspx
  * - http://codebetter.com/gregyoung/2009/01/16/ddd-the-generic-repository/
  */
class Repository extends StaticAnnotation
