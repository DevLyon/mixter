Mixter
======
Mixter is a project to discover CQRS/Event sourcing through koans in multiple
languages.

At this point the koans have been ported to 4 languages: C#, Java 8, PHP and
Javascript.

Starting
-------

Checkout `slide` branch, where you can find slides used for Mix-IT workshop.
It explains the main steps and goals for each (only the title is in French ;)).

1. Choose a language and checkout `{language}-workshop`, for `java` that would be
`java-workshop`.
2. Implement the failing test
3. merge and implement the next test : `{language}-test-{step}.{test number}` [1]

[1] to see the list of available tags you can use `git tag -l | grep {language}`

If you somehow get lost or stuck, you can `git merge {language}-solution-{step}`
or `git checkout {language}-solution-{step}` and see what our solution looks like.

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
