using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MatomeAutomation.Startup))]
namespace MatomeAutomation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
