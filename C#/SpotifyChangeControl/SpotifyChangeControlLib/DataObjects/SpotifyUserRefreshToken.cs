﻿using SpotifyAPI.SpotifyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyUserRefreshToken
    {
        private string _Code;
        private DateTime _TokenDate;


        public string Code
        {
            get { return this._Code; }
        }

        public DateTime TokenDate
        {
            get { return this._TokenDate; }
        }

       

        public SpotifyUserRefreshToken(string sCode, DateTime dtToken)
        {
            this._Code = sCode;
           
            this._TokenDate = dtToken;


        }
        public SpotifyUserRefreshToken(Token oSpotifyToken)
        {
            this._Code = oSpotifyToken.RefreshToken;
            this._TokenDate = oSpotifyToken.CreateDate;
        }

      


       
    }
}
