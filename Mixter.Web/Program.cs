using System;
using Microsoft.Owin.Hosting;

namespace Mixter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.WriteLine("Server web démarré: http://localhost:12345");
                Console.ReadLine();
            }
        }
    }
}
