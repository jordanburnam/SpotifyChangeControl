﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Mvc.Client.Controllers {
    [AllowAnonymous]
    public class HomeController : Controller {
        [HttpGet]
        [Route("Home/Index")]
        public ActionResult Index() => View();

        [HttpGet("~/")]
        public ActionResult Default() => new RedirectResult(Url.Action("Index", "Home"));


        [HttpGet]
        [Route("Home/Who")]
        public ActionResult Who()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/What")]
        public ActionResult What()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/When")]
        public ActionResult When()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/Where")]
        public ActionResult Where()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/Why")]
        public ActionResult Why()
        {
            return View();
        }
        [HttpGet]
        [Route("Home/How")]
        public ActionResult How()
        {
            return View();
        }

        [HttpGet]
        [Route("Home/FAQ")]
        public ActionResult FAQ()
        {
            return View();
        }


    }
}