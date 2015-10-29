using Owin;

namespace Mixter.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(options => options.Bootstrapper = new Bootstrapper());
        }
    }
}