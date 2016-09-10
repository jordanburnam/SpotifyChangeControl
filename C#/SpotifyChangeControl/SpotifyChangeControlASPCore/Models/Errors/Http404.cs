using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models.Errors
{
    public class Http404Message
    {
        public string PageMessage;

        public Http404Message(string sPageMessage)
        {
            this.PageMessage = sPageMessage;
        }
    }
}
