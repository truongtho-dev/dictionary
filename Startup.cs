using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Dictionary.Startup))]

namespace Dictionary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
