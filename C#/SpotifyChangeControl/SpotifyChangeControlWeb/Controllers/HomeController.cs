﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyChangeControl.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Who()
        {
            return View();
        }

        public ActionResult What()
        {
            return View();
        }

        public ActionResult How()
        {
            return View();
        }
    }
}