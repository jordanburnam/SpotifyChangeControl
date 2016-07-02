using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpotifyChangeControl.Startup))]
namespace SpotifyChangeControl
{
    
    public partial class Startup
    {


        static Startup()
        {

        }
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
