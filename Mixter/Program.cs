using System;
using System.Collections.Generic;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Domain.Subscriptions;
using Mixter.Infrastructure;

namespace Mixter
{
    public class Program
    {
        private static readonly IEventPublisher EventPublisher;
        private static readonly TimelineMessagesRepository TimelineMessagesRepository;

        static Program()
        {
            TimelineMessagesRepository = new TimelineMessagesRepository();
            EventPublisher = new EventPublisher(GenerateEventHandlers);
        }

        private static IEnumerable<IEventHandler> GenerateEventHandlers(IEventPublisher eventPublisher)
        {
            yield return new MessagePublishedHandler();
            yield return new TimelineMessageHandler(TimelineMessagesRepository, new SubscriptionRepository(new EventsDatabase()), eventPublisher);
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Connexion...");

                var email = AskEmail();

                StartMixter(new UserId(email));
            }
        }

        private static void StartMixter(UserId connectedUser)
        {
            do
            {
                Console.WriteLine("Menu :");
                Console.WriteLine("1 - Timeline");
                Console.WriteLine("2 - Publish new message");
                Console.WriteLine("3 - Follow user");
                Console.WriteLine("4 - Disconnect");

                var selectedMenu = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (selectedMenu)
                {
                    case '1':
                        DisplayTimeline(connectedUser);
                        break;
                    case '2':
                        PublishNewMessage(connectedUser);
                        break;
                    case '3':
                        FollowUser(connectedUser);
                        break;
                    case '4':
                        return;
                }
            } while (true);
        }

        private static void FollowUser(UserId connectedUser)
        {
            Console.WriteLine("User email :");
            var email = Console.ReadLine();

            Subscription.FollowUser(EventPublisher, connectedUser, new UserId(email));
        }

        private static void PublishNewMessage(UserId author)
        {
            Console.WriteLine("Content :");
            var content = Console.ReadLine();

            Message.PublishMessage(EventPublisher, author, content);
        }

        private static void DisplayTimeline(UserId connectedUser)
        {
            Console.WriteLine();
            foreach (var message in TimelineMessagesRepository.GetMessagesOfUser(connectedUser))
            {
                Console.WriteLine(message.AuthorId + " :");
                Console.WriteLine(message.Content);
                Console.WriteLine();

                Console.WriteLine("1 - Republish");
                Console.WriteLine("2 - Reply");
                Console.WriteLine("other - Next");
                var selectedMenu = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (selectedMenu)
                {
                    case '1':
                        Republish(connectedUser, message.MessageId);
                        break;
                    case '2':
                        Reply(connectedUser, message.MessageId);
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("--------------------------------------");
                Console.WriteLine();
            }
        }

        private static void Reply(UserId connectedUser, MessageId messageId)
        {
            throw new NotImplementedException();
        }

        private static void Republish(UserId connectedUser, MessageId messageId)
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
