Mixter
======
Mixter is a project to discover CQRS/Event sourcing through koans in multiple languages.

At this point the koans have been ported to 5 languages: C#, Java 8, Scala, PHP and Javascript.

Starting
-------

1. Checkout `master` branch
2. Execute `./run` script
3. You will have to make tests pass green, then to go to next test, execute `./next`

See few slides on http://devlyon.fr/mixter, it explains the main steps and goals for each.

You can view one solution for each language with a small Web API on branches [lang]-solution.

Feedback is required
--------------------

Feel free to use issues in this repo to give your feedback, to propose some improvements,
to ask for other languages...and even better to submit pull requests.

Explanations of some implementation details
-------------------------------------------

We have done some choices that we consider implementation details, but that can hurt
some people. So we try to explain them here.

### About events publication mecanism in CQRS/ES, there are two main well known options :

1) use an AggregateRoot base class that accumulate uncommitted events that are picked by Repository on Save of the aggregate.

2) use DomainEvents.Raise(event) static call from AggregateRoot protected Apply method

We chose a third way that consists of passing an IEventPublisher (with Publish method) to each aggregate method to raise events.
There is no more need to call Repository.Save and it avoids static method call.

### We use a DecisionProjection concept to keep track of "transient state" of aggregates.

We thought this "transient state" as a special projection (like Read model ones) to take further decision in the aggregate,
that's why we call it DecisionProjection. We kept this class private inside the aggregate.

### Commands and command handlers are not shown here for now, for simplicity, it has been left implicit through method of aggregates.

Perhaps something to introduce in further version.

Any questions ?
---------------

You can contact us through GitHub or on Twitter : @clem_bouillier, @florentpellet, @jeanhelou, @ouarzy.


Add new languages
---------
If you want to fork with your preferred language, you only need to follow some rules.

The KoanCli script is based on a naming convention in commit message.
 * Failing tests should contain `[Test KO]`
 * Resolving tests should contain `[Test OK]`

Currently the KoanCli script isn't dynamic and tests number must be static to know the correspondence between a test and a step.
Here number of tests per step:
 * Step 1, delete command: 4 tests
 * Step 2, timeline messages: 1 tests
 * Step 3, subscription aggregate: 4 tests
 * Step 4, aggregates interaction: 4 tests
 * Step 5, command handler: 2 tests


Changelog
---------

### V2
 * Create KoanCli: no git knowledge required. You simply call run script to starting, and next to jump at next step. (#3)
 * Remove reply message (#7)
 * Rename publish to quack (#16)
 * Fix several bug specific at languages (#15, #5, #4)
 * Make PHP version work with PHP 7.2 (#36)

### V1 (obsolete)
First version, use during a Mix-IT workshop 2015 : http://www.mix-it.fr/session/1041/agilite-par-le-code-grace-a-cqrs-et-eventsourcing

