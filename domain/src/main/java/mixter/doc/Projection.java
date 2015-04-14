package mixter.doc;

/**
 * A projection is a view of the domain which is used to query the system, aka "the read model".
 *
 * For the purpose of this exercise, we have introduced internal projections). These projections are not
 * strictly-speaking used by the query side but they help enforce a very important concept which is that aggregates do
 * not store state.
 * The decision projections derive the state from the event history, deriving only what is absolutely necessary for the
 * Aggregate to take its decisions when it receives a command.
 * By putting the derived state in an internal project we hope we made it obvious that this state is never persisted
 * directly.
 *
 * @see mixter.domain.DecisionProjectionBase and inherited classes
 * @see mixter.doc.Aggregate
 *
 * @link https://abdullin.com/post/event-sourcing-projections/
 * @link http://cqrs.wikidot.com/doc:projection
 */
public @interface Projection {
}
