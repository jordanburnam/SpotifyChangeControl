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
using Mvc.Client.Models.Account;
using System.Collections.Generic;
using System;

namespace Mvc.Client.Controllers {
    public class AccountController : Controller {


        private SpotifyChangeControlLib.SCCManager _oSCCManager;
        public AccountController()
        {
            this._oSCCManager = new SpotifyChangeControlLib.SCCManager();
        }



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

        [Route("Account/Info")]
        [HttpGet]
        public IActionResult Info()
        {
            UserInfoModel o = new UserInfoModel();
            IEnumerable<Claim> oClaims = User.FindAll("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            long iUserID = 0;
            string sSpotifyID = "";
            foreach (Claim oClaim in oClaims)
            {
                iUserID = this._oSCCManager.GetObjectIDForSpotifyID(oClaim.Value);
                sSpotifyID = oClaim.Value;
            }
            o.UserID = iUserID;
            o.SpotifyID = sSpotifyID;
            DateTime dt = DateTime.Now;
            o.TestDate_Local = dt.ToLocalTime();
            o.TestDate_UT = dt.ToUniversalTime();
            return View(o);
        }


    }
}