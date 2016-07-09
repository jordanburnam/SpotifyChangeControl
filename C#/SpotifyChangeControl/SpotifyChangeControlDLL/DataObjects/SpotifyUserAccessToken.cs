﻿using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.DataObjects
{
    internal class SpotifyUserAccessToken
    {
        private string _Code;

        private string _TokenType;

        private int _ExpiresIn;


        private DateTime _TokenDate;


        public string Code
        {
            get { return this._Code; }
        }

        public string TokenType
        {
            get { return this._TokenType; }
        }

        public int ExpiresIn
        {
            get { return this._ExpiresIn; }
        }

        public DateTime TokenDate
        {
            get { return this._TokenDate; }
        }

        public bool AccessExpired
        {
            get { return this.GetAccessExpired(); }
        }

        public SpotifyUserAccessToken(string sAccessCode, string sAccessTokenType, int iAccessExpiresIn, DateTime dtToken)
        {
            this._Code = sAccessCode;
            this._TokenType = sAccessTokenType;
            this._ExpiresIn = iAccessExpiresIn;
            this._TokenDate = dtToken;
        }

        public SpotifyUserAccessToken(Token oSpotifyToken)
        {
            this._Code = oSpotifyToken.AccessToken;
            this._TokenType = oSpotifyToken.TokenType;
            this._ExpiresIn = oSpotifyToken.ExpiresIn;
            this._TokenDate = oSpotifyToken.CreateDate;
        }

        


        private bool GetAccessExpired()
        {
            bool bAccessExpired = false;
            TimeSpan ts = DateTime.Now - this._TokenDate;
            if (ts.TotalSeconds > this._ExpiresIn)
            {
                bAccessExpired = true;
            }
            return bAccessExpired;
        }

    }
}
