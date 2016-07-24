using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Mvc.Client.Models.Changes;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
using System.Security.Claims;
using SpotifyChangeControlLib.DataObjects;

namespace Mvc.Client.Controllers
{
    
    public class ChangesController : Controller
    {
        private SpotifyChangeControlLib.SCCManager _oSCCManager;
        public ChangesController()
        {
            this._oSCCManager = new SpotifyChangeControlLib.SCCManager();
        }


        [Route("Changes/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("~/Changes/Search")]
        [Route("Changes/Search")]
        [HttpGet]
        public IActionResult Search()
        {
            return View(new SearchModel());
        }

        [Route("Changes/Results")]
        [HttpGet]
        public IActionResult Results([Bind]SearchModel oSearchModel)
        {
            //ResultModel oResultModel = new ResultModel();
            //oResultModel.Start = oSearchModel.Start;
            //oResultModel.End = oSearchModel.End;
            IEnumerable<Claim> oClaims = User.FindAll("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            long iUserID = 0;
            foreach (Claim oClaim in oClaims)
            {
                iUserID = this._oSCCManager.GetObjectIDForSpotifyID(oClaim.Value);
            }

            IEnumerable<SpotifyPlaylistChange> oPlaylistChanges = this._oSCCManager.GetPlaylistChangesForUser(iUserID, oSearchModel.GetStartUTC(), oSearchModel.GetEndUTC());
            return View("Results", new ResultModel(oSearchModel, oPlaylistChanges));
        }

        
    }
}
