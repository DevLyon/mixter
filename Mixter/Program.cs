using System;
using System.Linq;
using Mixter.Domain;
using Mixter.Domain.Core;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;

namespace Mixter
{
    public class Program
    {
        private static readonly IEventPublisher EventPublisher;
        private static readonly ITimelineMessagesRepository TimelineMessagesRepository;
        private static readonly IMessagesRepository MessagesRepository;

        static Program()
        {
            var eventsDatabase = new EventsStore();

            TimelineMessagesRepository = new TimelineMessagesRepository();
            MessagesRepository = new MessagesRepository(eventsDatabase);
            var handlersGenerator = new EventHandlersGenerator(eventsDatabase, TimelineMessagesRepository);
            EventPublisher = new EventPublisherWithStorage(eventsDatabase, new EventPublisher(handlersGenerator.Generate));
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

            Message.Publish(EventPublisher, author, content);
        }

        private static void DisplayTimeline(UserId connectedUser)
        {
            Console.WriteLine();
            foreach (var message in TimelineMessagesRepository.GetMessagesOfUser(connectedUser).ToArray())
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
            Console.WriteLine("Response :");
            var responseContent = Console.ReadLine();

            var message = MessagesRepository.Get(messageId);
            message.Reply(EventPublisher, connectedUser, responseContent);
        }

        private static void Republish(UserId connectedUser, MessageId messageId)
        {
            var message = MessagesRepository.Get(messageId);
            message.Republish(EventPublisher, connectedUser);
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

    public class MessagePublishedHandler : IEventHandler<MessagePublished>
    {
        public void Handle(MessagePublished evt)
        {
            Console.WriteLine("\nMessage published !\n");
        }
    }
}
