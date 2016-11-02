using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTBGD.Startup))]
namespace WebTBGD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
