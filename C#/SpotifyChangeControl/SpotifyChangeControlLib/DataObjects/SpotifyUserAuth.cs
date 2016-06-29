using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyUserAuth
    {
        private string _Code;
        private DateTime _AuthDate;


        public string Code
        {
            get { return this._Code; }
        }

        public DateTime AuthDate
        {
            get { return this._AuthDate; }
        }

        

        public SpotifyUserAuth(string sAuthCode, DateTime dtAuth)
        {
            this._Code = sAuthCode;

            this._AuthDate = dtAuth;
        }
      




    }
}
