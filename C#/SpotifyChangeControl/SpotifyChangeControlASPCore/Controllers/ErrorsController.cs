using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvc.Client.Models.Errors;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Mvc.Client.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: /<controller>/
        [Route("Errors/GetCustomErrorPageToMakeTheStupidHumanFeelGoodAboutThereTecnicalAbilitiesAndReassureThemItsNotTheirFault")]
        [HttpGet]
        public IActionResult GetCustomErrorPageToMakeTheStupidHumanFeelGoodAboutThereTecnicalAbilitiesAndReassureThemItsNotTheirFault(string sHttpStatus)
        {
            string sErrorMessage;
            HttpError oHttpCustomError;
            if (sHttpStatus != null)
            {
                if (sHttpStatus == "404")
                {
                    sErrorMessage = "The page you requested is not here right now. Please leave a message for this page at the beep....";
                  
                    return View("404");

                }
                else if (sHttpStatus == "500")
                {
                    sErrorMessage = "The server was unable to handle your request right now, please refrain from being so needy! Also stop causing my server to have errors and/or go learn how to Jordan and come back.";
                    oHttpCustomError = new HttpError(sHttpStatus, sErrorMessage);
                    return View("CustomError", oHttpCustomError);
                }
                else
                {
                    sHttpStatus = "WTF";
                    sErrorMessage = "I have no idea how you messed up this bad. Please go study up on how not to be a failure.";
                    oHttpCustomError = new HttpError(sHttpStatus, sErrorMessage);
                    return View("CustomError", oHttpCustomError);
                }
            } else {
                sHttpStatus = "WTF";
                sErrorMessage = "I have no idea how you messed up this bad. Please go study up on how not to be a failure.";
                oHttpCustomError = new HttpError(sHttpStatus, sErrorMessage);
                return View("CustomError", oHttpCustomError);
            }
       
 
        }

      
      


    }
}
