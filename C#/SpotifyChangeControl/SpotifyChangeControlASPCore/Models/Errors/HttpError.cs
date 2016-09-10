using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models.Errors
{
    public class HttpError
    {
        private string _HttpStatus;

        public string HttpStatus
        {
            get { return this._HttpStatus; }

        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return this._ErrorMessage; }
        }

        public HttpError(string sHttpStatus, string sErrorMessage)
        {
            this._HttpStatus = sHttpStatus;
            this._ErrorMessage = sErrorMessage;
        }
    }
}
