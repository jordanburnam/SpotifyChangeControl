/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Extensions;

namespace Mvc.Client.Controllers {
    public class AuthenticationController : Controller {
        [HttpGet("~/signin")]
        public IActionResult SignIn() => View("SignIn");

        [HttpPost("~/Authorize")]
        public IActionResult Authorize() {
 
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Spotify");
        }

        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult SignOut() {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}