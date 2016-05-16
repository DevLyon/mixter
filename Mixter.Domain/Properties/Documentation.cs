using System;

// This package contains live documentation annotations. They describe the important parts of mixter to try and help
// the developer get a feel for the concepts he or she is manipulating through the exercises. 
// We annotated elements from DDD, CQRS and ES.
//
// The definitions in the annotations are the ones we chose.While some are commonly agreed upon, other can be subject
// to different interpretations.We are not pretending that we know better and encourage you to explore the concepts and
// build your own opinion on the matter.We try to provide some links to help you begin your exploration. We hope you
// will have as much fun as we do in the process !
namespace Mixter.Domain
{
    /// <summary>
    /// A collection of objects that are bound together by a root entity, otherwise known as an aggregate root.
    ///
    /// The aggregate root guarantees the consistency of changes being made within the aggregate by forbidding external
    /// objects from holding references to its members.It will receive commands, make decisions based on these commands and
    /// their current decision projection.If the result of the decision needs to be persisted, it will be encoded in an
    /// Event which will be saved to a store and published to the rest of the system.
    ///
    /// See :
    /// <see cref="CommandAttribute"/>
    /// <see cref="ProjectionAttribute"/>
    /// <see cref="EventAttribute"/>
    ///
    /// https://vaughnvernon.co/?p=838
    /// https://en.wikipedia.org/wiki/Domain-driven_design
    /// http://martinfowler.com/bliki/DDD_Aggregate.html
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AggregateAttribute : Attribute
    {
    }

    /// <summary>
    /// A Command is the main concept of the write-side in CQRS.
    ///
    /// It represents a demand of a user or an external system that our system performs some action.A command doesn't return
    /// any data.
    /// It maps to the concept of command in Object oriented programming, which is defined as :
    /// A command is any method that mutates state and a query is any method that returns a value.
    ///
    /// See :
    /// http://cqrs.nu/Faq
    /// http://culttt.com/2015/01/14/command-query-responsibility-segregation-cqrs/
    /// http://martinfowler.com/bliki/CQRS.html
    /// https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_Query_Responsibility_Segregation
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
    }

    /// <summary>
    /// A Query is the main concept of the read-side in CQRS.
    ///
    /// A query allows to get data directly from database, bypassing business logic
    ///
    /// See :
    /// http://cqrs.nu/Faq
    /// http://culttt.com/2015/01/14/command-query-responsibility-segregation-cqrs/
    /// http://martinfowler.com/bliki/CQRS.html
    /// https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_Query_Responsibility_Segregation
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface)]
    public class QueryAttribute : Attribute
    {
    }


    /// <summary>
    /// Within the scope of this code base, Handler stands for Event Handler.
    ///
    /// It's a part of the system which will react when it receives given events. The reaction can take any form :
    ///  * Using the event to update a view of the domain on the query side(updating a database, regenerating an HTML cache ...)
    ///  * Sending commands to other Aggregates
    ///  * Translating the event to an extenal protocol(sending an email, pushing a message in a queue, sending a notification)
    ///
    /// See :
    /// <see cref="AggregateAttribute"/>
    /// http://cqrs.nu/Faq
    /// http://thinkbeforecoding.com/post/2013/07/28/Event-Sourcing-vs-Command-Sourcing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlerAttribute : Attribute
    {
    }


    /// <summary>
    /// A projection is a view of the domain which is used to query the system, aka "the read model".
    ///
    /// For the purpose of this exercise, we have introduced internal projections). These projections are not
    /// strictly-speaking used by the query side but they help enforce a very important concept which is that aggregates do
    /// not store state.
    /// The decision projections derive the state from the event history, deriving only what is absolutely necessary for the
    /// Aggregate to take its decisions when it receives a command.
    /// By putting the derived state in an internal project we hope we made it obvious that this state is never persisted
    /// directly.
    ///
    /// see DecisionProjection
    /// <see cref="AggregateAttribute"/>
    ///
    /// <a href="https://abdullin.com/post/event-sourcing-projections/" />
    /// <a href="http://cqrs.wikidot.com/doc:projection" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ProjectionAttribute : Attribute
    {
    }


    /// <summary>
    /// A repository mediates between the domain and data mapping layers using a collection-like interface for accessing
    /// domain objects.
    ///
    /// In the exercise there are multiple repositories, they can be backed by different stores.Some are backed by event
    /// stores, other may be backed by an SQL database. The purpose of the repository is to protect the domain model from
    /// the dirty details of persistence as much as possible.
    ///
    /// <a href="http://martinfowler.com/eaaCatalog/repository.html" />
    /// <a href="https://msdn.microsoft.com/en-us/library/ff649690.aspx" />
    /// <a href="http://codebetter.com/gregyoung/2009/01/16/ddd-the-generic-repository/" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RepositoryAttribute : Attribute
    {
    }


    /// <summary>
    /// A domain object that defines an event (something that happens). A domain event is an event that domain experts care about.
    ///
    /// <a href="https://en.wikipedia.org/wiki/Domain-driven_design" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class EventAttribute : Attribute
    {
    }
}
