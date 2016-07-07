using SpotifyAPI.Web.Auth;
using SpotifyChangeControlLib;
using SpotifyChangeControlLib.DataManagers;
using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyChangeControl.Controllers
{
    public class EmailController : Controller
    {

        private string SCC_PRIVATE_ID;
        private string SCC_PUBLIC_ID;
        private string SCC_SQL_CON;
        private string SCC_REDIS_HOST;
        private string SCC_REDIS_PASS;
        private int SCC_REDIS_PORT;
        private SCCManager oSCCManager;
        private AutorizationCodeAuth _oAuthorizationCodeAuth;

        public EmailController()
        {
            SCC_PRIVATE_ID = System.Environment.GetEnvironmentVariable("SCC_PRIVATE_ID");
            SCC_PUBLIC_ID = System.Environment.GetEnvironmentVariable("SCC_PUBLIC_ID");

            SCC_SQL_CON = System.Environment.GetEnvironmentVariable("SCC_SQL_CON");

            SCC_REDIS_HOST = System.Environment.GetEnvironmentVariable("SCC_REDIS_HOST");
            SCC_REDIS_PASS = System.Environment.GetEnvironmentVariable("SCC_REDIS_PASS");
            string sPort = System.Environment.GetEnvironmentVariable("SCC_REDIS_PORT");
            if (!int.TryParse(sPort, out SCC_REDIS_PORT))
            {
                SCC_REDIS_PORT = 6379;
            }


            oSCCManager = new SCCManager(SCC_PRIVATE_ID, SCC_PUBLIC_ID, "", SCC_SQL_CON, SCC_REDIS_HOST, SCC_REDIS_PORT, SCC_REDIS_PASS);

        }

        // GET: Email
        public ActionResult Changes(string UserGuid, DateTime? LastSeen)
        {
            SpotifyPlaylistChangeManager oSpotifyPlaylistChangeManager = oSCCManager.GetUserForUserGuid(UserGuid, LastSeen);
            ViewBag.Message = LastSeen == null ? "you joined" : LastSeen.Value.ToShortDateString();
            return View(oSpotifyPlaylistChangeManager);
        }
    }

    
}