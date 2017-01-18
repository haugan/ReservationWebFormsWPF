using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(User_Application.Startup))]
namespace User_Application
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
