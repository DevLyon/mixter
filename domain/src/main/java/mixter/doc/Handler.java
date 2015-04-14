package mixter.doc;

/**
 * Within the scope of this code base, Handler stands for Event Handler.
 *
 * It's a part of the system which will react when it receives given events. The reaction can take any form :
 * <ul>
 *   <li>Using the event to update a view of the domain on the query side (updating a database, regenerating an HTML
 *   cache ...)</li>
 *   <li>Sending commands to other Aggregates </li>
 *   <li>Translating the event to an extenal protocol (sending an email, pushing a message in a queue, sending a
 *   notification)</li>
 * </ul>
 *
 * @see mixter.doc.Aggregate
 * @link http://cqrs.nu/Faq
 * @link http://thinkbeforecoding.com/post/2013/07/28/Event-Sourcing-vs-Command-Sourcing
 */
public @interface Handler {
}
