using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyChangeControlLib;
using System.Text;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyChangeControlLib.DataObjects;


namespace SpotifyChangeControl.Controllers
{
    
    public class AccountController : Controller
    {
        private  SCCManager _oSCCManager;

        public AccountController()
        {
            this._oSCCManager = new SCCManager();   
        }

        //
        // GET: /Account/Login
        [HttpGet]
        public ActionResult Authorize()
        {
            StringBuilder builder = new StringBuilder("https://accounts.spotify.com/authorize/?");
            builder.Append("client_id=" + this._oSCCManager.SCC_PUBLIC_ID);
            builder.Append("&response_type=code");
            builder.Append("&redirect_uri=" + this._oSCCManager.RedirectUrl);
            builder.Append("&state=" + "");
            builder.Append("&scope=" + "playlist-read-private user-read-private user-read-email user-library-read user-follow-read");
            builder.Append("&show_dialog=" + "false");
            ViewData["AuthUrl"] = builder.ToString();
            return View();
        }

        [HttpGet]
        public RedirectResult Logout()
        { 
            if (AuthenticateUser())
            {
                this._oSCCManager.DeleteUserSession(this.ControllerContext.HttpContext.Request.Cookies.Get("UserGuid").Value);
            }
            return new RedirectResult("/SpotifyChangeControl/Home/Index");
        }
    
    
        public RedirectResult Authorized(string code, string error, string state)
        {

            if (error != null)
            {
                throw new Exception(error);
            }
            AutorizationCodeAuth oAutorizationCodeAuth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = this._oSCCManager.SCC_PUBLIC_ID,
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = this._oSCCManager.RedirectUrl,
                //How many permissions we need?
                Scope = Scope.UserReadPrivate | Scope.UserReadPrivate | Scope.PlaylistReadPrivate | Scope.UserLibraryRead | Scope.UserReadPrivate | Scope.UserFollowRead
            };
            
            
            Token oToken;
            oToken = oAutorizationCodeAuth.ExchangeAuthCode(code, this._oSCCManager.SCC_PRIVATE_ID);
            //oToken = oAutorizationCodeAuth.RefreshToken(response.Code, SCC_PRIVATE_ID);


            SpotifyWebAPI oSpotifyWebApi = new SpotifyWebAPI()
            {
                AccessToken = oToken.AccessToken,
                TokenType = oToken.TokenType,
                UseAuth = true
            };


            PrivateProfile oPrivateProfile = oSpotifyWebApi.GetPrivateProfile();

            
           
            SpotifyUser oSpotifyUser = this._oSCCManager.AddSpotifyUser(oPrivateProfile, code, oToken);
            // ViewBag.RedirectUrl = string.Format("/SpotifyChangeControl/Change/Index?UserGuid={0}&SessionGuid={1}", oSpotifyUser.UserGuid, oSpotifyUser.SessionGuid);
           
            if (!this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserGuid"))
            {
                HttpCookie hcUserGuid = new HttpCookie("UserGuid");
                hcUserGuid.Value = oSpotifyUser.UserGuid;
                this.ControllerContext.HttpContext.Response.Cookies.Add(hcUserGuid);
                this.ControllerContext.HttpContext.Response.Cookies.Add(hcUserGuid);
            }
            else
            {
                this.ControllerContext.HttpContext.Request.Cookies.Get("UserGuid").Value = oSpotifyUser.UserGuid;
            }

            if (!this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("SessionGuid"))
            {
                HttpCookie hcSessionGuid = new HttpCookie("SessionGuid");
                hcSessionGuid.Value = oSpotifyUser.SessionGuid;
                this.ControllerContext.HttpContext.Response.Cookies.Add(hcSessionGuid);
                
            }
            else
            {
                this.ControllerContext.HttpContext.Request.Cookies.Get("SessionGuid").Value = oSpotifyUser.UserGuid;
            }

                
            return new RedirectResult(string.Format("/SpotifyChangeControl/Change/Index", oSpotifyUser.UserGuid, oSpotifyUser.SessionGuid));
        }


        public bool AuthenticateUser()
        {
            string sUserGuid;
            string sSessionGuid;
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserGuid"))
            {
                sUserGuid = this.ControllerContext.HttpContext.Request.Cookies.Get("UserGuid").Value;
            }
            else
            {
                return false;
            }

            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("SessionGuid"))
            {
                sSessionGuid = this.ControllerContext.HttpContext.Request.Cookies.Get("SessionGuid").Value;
            }
            else
            {
                return false;
            }



            if (sUserGuid != null && sSessionGuid != null)
            {
                return this._oSCCManager.AuthenticateUserSession(sUserGuid, sSessionGuid);
            }
            else
            {
                return false;
            }
        }





    }
}