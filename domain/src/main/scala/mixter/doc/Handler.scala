package mixter.doc

import scala.annotation.StaticAnnotation


/**
  * Within the scope of this code base, Handler stands for Event Handler.
  *
  * It's a part of the system which will react when it receives given events. The reaction can take any form :
  *
  * - Using the event to update a view of the domain on the query side (updating a database, regenerating an HTML
  * cache ...)
  * - Sending commands to other [[mixter.doc.Aggregate Aggregates]]
  * - Translating the event to an extenal protocol (sending an email, pushing a message in a queue, sending a
  * notification)
  *
  * @see
  *      - [[mixter.doc.Aggregate]]
  *
  * ==Further references:==
  *
  * - http://cqrs.nu/Faq
  * - http://thinkbeforecoding.com/post/2013/07/28/Event-Sourcing-vs-Command-Sourcing
  */
class Handler extends StaticAnnotation
