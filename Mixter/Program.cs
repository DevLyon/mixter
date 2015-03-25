using System;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Infrastructure;

namespace Mixter
{
    public class Program
    {
        private static readonly IEventPublisher EventPublisher;
        private static readonly TimelineMessagesRepository _timelineMessagesRepository;

        static Program()
        {
            _timelineMessagesRepository = new TimelineMessagesRepository();
            EventPublisher = new EventPublisher(
                new MessagePublishedHandler(), 
                new TimelineMessageHandler(_timelineMessagesRepository));
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Connexion...");

            var email = AskEmail();

            StartMixter(new UserId(email));
        }

        private static void StartMixter(UserId userId)
        {
            do
            {
                Console.WriteLine("Menu :");
                Console.WriteLine("1 - Timeline");
                Console.WriteLine("2 - Publish new message");

                var selectedMenu = Console.ReadKey().KeyChar;
                switch (selectedMenu)
                {
                    case '1':
                        DisplayTimeline(userId);
                        break;
                    case '2':
                        PublishNewMessage(userId);
                        break;
                }
            } while (true);
        }

        private static void PublishNewMessage(UserId author)
        {
            Console.WriteLine("Content :");
            var content = Console.ReadLine();

            Message.PublishMessage(EventPublisher, author, content);
        }

        private static void DisplayTimeline(UserId userId)
        {
            Console.WriteLine();
            foreach (var message in _timelineMessagesRepository.GetMessagesOfUser(userId))
            {
                Console.WriteLine(message.AuthorId + " :");
                Console.WriteLine(message.Content);
                Console.WriteLine();

                Console.WriteLine("1 - Republish");
                Console.WriteLine("2 - Reply");
                Console.WriteLine("other - Next");
                var selectedMenu = Console.ReadKey().KeyChar;
                switch (selectedMenu)
                {
                    case '1':
                        Republish(userId, message.MessageId);
                        break;
                    case '2':
                        Reply(userId, message.MessageId);
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("--------------------------------------");
                Console.WriteLine();
            }


        }

        private static void Reply(UserId userId, MessageId messageId)
        {
            throw new NotImplementedException();
        }

        private static void Republish(UserId userId, MessageId messageId)
        {
            throw new NotImplementedException();
        }

        private static string AskEmail()
        {
            do
            {
                Console.WriteLine("Email :");

                var email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    return email;
                }
                Console.WriteLine("On a dit un email! Reessaye encore une fois...");
            } while (true);
        }
    }

    internal class MessagePublishedHandler : IEventHandler<MessagePublished>
    {
        public void Handle(MessagePublished evt)
        {
            Console.WriteLine("\nMessage published !\n");
        }
    }
}
