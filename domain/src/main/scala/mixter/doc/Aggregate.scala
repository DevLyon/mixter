package mixter.doc

import scala.annotation.StaticAnnotation

/**
  * A collection of objects that are bound together by a root entity, otherwise known as an aggregate root.
  *
  * The aggregate root guarantees the consistency of changes being made within the aggregate by forbidding external
  * objects from holding references to its members. It will receive [[mixter.doc.Command commands]], make decisions based on these commands and
  * their current decision [[mixter.doc.Projection projection]]. If the result of the decision needs to be persisted, it will be encoded in an
  * [[mixter.domain.Event event]] which will be saved to a store and published to the rest of the system.
  *
  * @see
  *
  * - [[mixter.doc.Command]]
  * - [[mixter.doc.Projection]]
  * - [[mixter.domain.Event]]
  *
  * ==Further references:==
  *
  * - [[https://vaughnvernon.co/?p=838]]
  * - [[https://en.wikipedia.org/wiki/Domain-driven_design]]
  * - [[http://martinfowler.com/bliki/DDD_Aggregate.html]]
  *
  */
class Aggregate extends StaticAnnotation
