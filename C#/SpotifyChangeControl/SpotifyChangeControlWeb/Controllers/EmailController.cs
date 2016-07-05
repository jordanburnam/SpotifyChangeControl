using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyChangeControl.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Chnages(long UserID)
        {
            return View();
        }
    }
}