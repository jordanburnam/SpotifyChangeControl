using SpotifyChangeControlLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            
            return View();
        }

        public  async Task<RedirectResult> Get()
        {
           
            return new RedirectResult("/Change/Look");
        }

        public  ViewResult Look()
        {


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