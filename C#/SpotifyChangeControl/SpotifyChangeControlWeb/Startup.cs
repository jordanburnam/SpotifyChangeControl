using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpotifyChangeControl.Startup))]
namespace SpotifyChangeControl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
