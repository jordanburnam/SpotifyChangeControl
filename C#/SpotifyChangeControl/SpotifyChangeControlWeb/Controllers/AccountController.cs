using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib;

namespace SpotifyChangeControl.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private string SCC_PRIVATE_ID;
        private string SCC_PUBLIC_ID;
        private string SCC_SQL_CON;
        private string SCC_REDIS_HOST;
        private string SCC_REDIS_PASS;
        private int SCC_REDIS_PORT;
        private string RedirectUrl;
        private SCCManager oSCCManager;


        public AccountController()
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

            this.RedirectUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Account/Authorized";
            oSCCManager = new SCCManager(SCC_PRIVATE_ID, SCC_PUBLIC_ID, RedirectUrl, SCC_SQL_CON, SCC_REDIS_HOST, SCC_REDIS_PORT, SCC_REDIS_PASS);
        }






        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Authorize()
        {
            AutorizationCodeAuth oAutorizationCodeAuth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = SCC_PUBLIC_ID,
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = this.RedirectUrl,
                ShowDialog = false,
                //How many permissions we need?
                Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_LIBRARY_MODIFY | Scope.USER_READ_PRIVATE
                   | Scope.USER_FOLLOW_MODIFY | Scope.USER_FOLLOW_READ | Scope.PLAYLIST_MODIFY_PRIVATE | Scope.USER_READ_BIRTHDATE | Scope.PLAYLIST_MODIFY_PUBLIC
            };
            oAutorizationCodeAuth.DoAuth();
            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult Authorized(string code, string error, string state)
        {

            if (error != null)
            {

            }
            AutorizationCodeAuth oAutorizationCodeAuth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = SCC_PUBLIC_ID,
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = this.RedirectUrl,
                ShowDialog = false,
                //How many permissions we need?
                Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_READ_PRIVATE | Scope.USER_FOLLOW_READ
            };
            Token oToken;
            oToken = oAutorizationCodeAuth.ExchangeAuthCode(code, SCC_PRIVATE_ID);
            //oToken = oAutorizationCodeAuth.RefreshToken(response.Code, SCC_PRIVATE_ID);

            SpotifyWebAPIClient oSpotifyWebApi = new SpotifyWebAPIClient()
            {
                AccessToken = oToken.AccessToken,
                TokenType = oToken.TokenType,
                UseAuth = true
            };


            PrivateProfile oPrivateProfile = oSpotifyWebApi.GetPrivateProfile();

            Int64 iUserID = this.oSCCManager.AddSpotifyUser(oPrivateProfile, code, oToken);


            // If we got this far, something failed, redisplay form
            return new EmptyResult();
        }





    }
}