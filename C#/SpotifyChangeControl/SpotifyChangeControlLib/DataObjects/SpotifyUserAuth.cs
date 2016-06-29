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
        private bool _HasBeenTokenized;

        public string Code
        {
            get { return this._Code; }
        }

        public DateTime AuthDate
        {
            get { return this._AuthDate; }
        }

        public bool HasBeenTokenized
        {
            get { return this._HasBeenTokenized; }
        }

        public SpotifyUserAuth(string sAuthCode, bool bHasBeenTokenized, DateTime dtAuth)
        {
            this._Code = sAuthCode;

            this._AuthDate = AuthDate;

            this._HasBeenTokenized = bHasBeenTokenized;
        }
      




    }
}
