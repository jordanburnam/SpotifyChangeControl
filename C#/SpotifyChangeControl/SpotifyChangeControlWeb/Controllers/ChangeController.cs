using SpotifyChangeControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyChangeControl.Controllers
{
    public class ChangeController : Controller
    {

        private SCCManager _oSCCManager;
        public ChangeController()
        {
            this._oSCCManager = new SCCManager();
        }

        // GET: Change
        public ActionResult Index()
        {
            if (!AuthenticateUser())
            {
                return new RedirectResult("/SpotifyChangeControl/Account/Authorize");
            }
            return View();
        }

        public ActionResult Changes()
        {
            if (!AuthenticateUser())
            {
                return new RedirectResult("/SpotifyChangeControl/Account/Authorize");
            }

            return View();
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