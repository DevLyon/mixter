using Nancy;

namespace Mixter.Web
{
    public class Hello : NancyModule
    {
        public Hello()
        {
            Get("/", args => "Hello World, it's Mixter with Nancy on .NET Core");
        }
    }
}