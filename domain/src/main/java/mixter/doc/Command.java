package mixter.doc;

/**
 * A Command is the main concept of the write-side in CQRS.
 *
 * It represents a demand of a user or an external system that our system performs some action. A command doesn't return
 * any data.
 * It maps to the concept of command in Object oriented programming, which is defined as :
 * <quote>A command is any method that mutates state and a query
 * is any method that returns a value.</quote>
 *
 * @link http://cqrs.nu/Faq
 * @link http://culttt.com/2015/01/14/command-query-responsibility-segregation-cqrs/
 * @link http://martinfowler.com/bliki/CQRS.html
 * @link https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_Query_Responsibility_Segregation
 */
public @interface Command {
}
