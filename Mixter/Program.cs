using System;
using Mixter.Domain;
using Mixter.Domain.Messages;
using Mixter.Infrastructure;

namespace Mixter
{
    public class Program
    {
        private static readonly IEventPublisher EventPublisher = new EventPublisher();

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

                var selectedMenuFormatted = Console.ReadLine();
                int selectedMenu;
                if (!int.TryParse(selectedMenuFormatted, out selectedMenu))
                {
                    continue;
                }

                switch (selectedMenu)
                {
                    case 1:
                        DisplayTimeline(userId);
                        break;
                    case 2:
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

            Console.WriteLine("Message published !");
        }

        private static void DisplayTimeline(UserId userId)
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
}
