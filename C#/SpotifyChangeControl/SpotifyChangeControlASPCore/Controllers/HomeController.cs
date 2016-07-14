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
    }
}