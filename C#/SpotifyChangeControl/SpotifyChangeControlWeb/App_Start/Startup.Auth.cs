﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using AspNet.Security.OAuth.Spotify;


[assembly: OwinStartup(typeof(SpotifyChangeControl.App_Start.Startup))]

namespace SpotifyChangeControl.App_Start
{
    public class Startup
    {
        private SpotifyChangeControlLib.SCCManager _oSCCManager;

        public void Configuration(IAppBuilder app)
        {
            //// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            _oSCCManager = new SpotifyChangeControlLib.SCCManager();

          
        }
    }
}
