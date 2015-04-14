package mixter.doc;

/**
 * A collection of objects that are bound together by a root entity, otherwise known as an aggregate root.
 *
 * The aggregate root guarantees the consistency of changes being made within the aggregate by forbidding external
 * objects from holding references to its members. It will receive commands, make decisions based on these commands and
 * their current decision projection. If the result of the decision needs to be persisted, it will be encoded in an
 * Event which will be saved to a store and published to the rest of the system.
 *
 * @see mixter.doc.Command
 * @see mixter.doc.Projection
 * @see mixter.domain.Event
 *
 * @link https://vaughnvernon.co/?p=838
 * @link https://en.wikipedia.org/wiki/Domain-driven_design
 * @link http://martinfowler.com/bliki/DDD_Aggregate.html
 *
 */
public @interface Aggregate {
}
