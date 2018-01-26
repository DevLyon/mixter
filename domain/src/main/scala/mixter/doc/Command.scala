package mixter.doc

import scala.annotation.StaticAnnotation


/**
  * A Command is the main concept of the write-side in CQRS.
  *
  * It represents a demand of a user or an external system that our system performs some action. A command doesn't return
  * any data.
  * It maps to the concept of command in Object oriented programming, which is defined as :
  * <q>A command is any method that mutates state and a query
  * is any method that returns a value.</q>
  *
  * ==Further references:==
  *
  * - [[http://cqrs.nu/Faq]]
  * - [[http://culttt.com/2015/01/14/command-query-responsibility-segregation-cqrs/]]
  * - [[http://martinfowler.com/bliki/CQRS.html]]
  * - [[https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_Query_Responsibility_Segregation]]
  */
class Command extends StaticAnnotation
