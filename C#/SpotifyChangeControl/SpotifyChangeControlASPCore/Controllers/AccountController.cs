/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Extensions;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNet.Security.OAuth.Spotify;
using System.Security.Claims;

namespace Mvc.Client.Controllers {
    public class AccountController : Controller {

        [Route("/Account/Login/")]
        [AllowAnonymous]
        [HttpGet]
        public  IActionResult Login()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = Url.Action("Index", "Home") }, "Spotify");
        }

        [Route("/Account/Logout/")]
        [Authorize]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            
            return View();
        }


        [Authorize]
        [HttpGet]
        public IActionResult Forbidden()
        {
            return View();
        }


    }
}