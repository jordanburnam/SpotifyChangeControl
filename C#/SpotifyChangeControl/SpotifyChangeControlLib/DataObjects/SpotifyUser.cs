﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyChangeControlLib.Utilities;
using SpotifyChangeControlLib.Types.Interfaces;
using SpotifyChangeControlLib.Types.Abstract;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib.AccessLayer;


namespace SpotifyChangeControlLib.DataObjects
{/// <summary>
/// This class will represent the Spotify User and will be populated with
/// data from the database tables of Spotify_User and Spotify_User_Token
/// </summary>
    internal class SpotifyUser 
    {
        protected Int64 _ID;
        protected string _SpotifyID;
        protected string _Name;
        protected SpotifyUserAuth _UserAuth;
        protected SpotifyUserRefreshToken _RefreshToken;
        protected SpotifyUserAccessToken _AccessToken;

        protected IEnumerable<SpotifyPlaylist> _Playlists;

        public Int64 ID
        {
            get { return this._ID; }
        }

        public string SpotifyID
        {
            get { return this._SpotifyID; }
        }

        public string Name
        {
            get { return this._Name; }
        }

        public IEnumerable<SpotifyPlaylist> Playlist
        {
            get
            {
                if(this._Playlists == null)
                {
                    this._Playlists = SpotifyDatabase.GetUsersFollowedPlaylists(this);
                }
                return this._Playlists;

            }
            set { this._Playlists = value; }
        }


        public SpotifyUserAuth UserAuth
        {
            get { return this._UserAuth; }
        }

        public SpotifyUserRefreshToken RefreshToken
        {
            get { return this._RefreshToken; }
        }

      
        public SpotifyUserAccessToken AccessToken
        {
            get { return this._AccessToken;  }
        }

        protected SpotifyUser()
        {
            //THis is only used for the UserManger to not have to create an instance of an object unless it is needed!
        }


        public SpotifyUser(DataRow drUser)
        {
            this._ID = Convert.ToInt64(drUser["UserID"].ToString());
            this._Name = drUser["Name"].ToString();
            string sAuthCode = drUser["AuthCode"].ToString();
            DateTime dtAuth = Convert.ToDateTime(drUser["AuthDate"].ToString());
            string sAccessCode = drUser["AccessTokenCode"].ToString();
            string sRefreshCode = drUser["RefreshTokenCode"].ToString();
            string sAccessTokenType = drUser["AccessTokenTokenType"].ToString();
            int iAccessExpiresIn = Convert.ToInt32(drUser["AccessTokenExpiresIn"].ToString());
            DateTime dtToken = Convert.ToDateTime(drUser["AccessTokenCreatedDate"].ToString());
            this._UserAuth = new SpotifyUserAuth(sAuthCode, dtAuth);
            this._RefreshToken = new SpotifyUserRefreshToken(sRefreshCode, dtToken);
            this._AccessToken = new SpotifyUserAccessToken(sAccessCode, sAccessTokenType, iAccessExpiresIn, dtToken);

        }


        public SpotifyUser(PrivateProfile oPrivateProfile, string sAuthCode, Token oToken)
        {
            this._SpotifyID = oPrivateProfile.Id;
            this._Name = oPrivateProfile.Id;
            this._ID = SpotifyAccessLayer.GetObjectIDForSpotifyID(this._SpotifyID);

            string sAccessCode = oToken.AccessToken;
            string sRefreshCode = oToken.RefreshToken;
            string sAccessTokenType = oToken.TokenType;
            int iAccessExpiresIn = oToken.ExpiresIn;
            DateTime dtToken = oToken.CreateDate;
            DateTime dtAuth = oToken.CreateDate;
            this._UserAuth = new SpotifyUserAuth(sAuthCode, dtAuth);
            SaveUserAndAuthToDatabase();
            this._RefreshToken = new SpotifyUserRefreshToken(sRefreshCode, dtToken);
            SaveRefreshTokenToDatebase();
            this._AccessToken = new SpotifyUserAccessToken(sAccessCode, sAccessTokenType, iAccessExpiresIn, dtToken);
            SaveAccessTokenToDatabase();
        }
        
        private void SaveUserAndAuthToDatabase()
        {
            SpotifyAccessLayer.SaveUserAndAuthToDatabase(this);
        }
        
        private void SaveRefreshTokenToDatebase()
        {
            SpotifyAccessLayer.SaveRefreshTokenForUserToDatabase(this.ID, this._RefreshToken);
        }

        private void SaveAccessTokenToDatabase()
        {
            SpotifyAccessLayer.SaveAccessTokenForUserToDatabase(this.ID, this._AccessToken);
        }

       
        
        public void UpdateUserWithToken(Token oToken)
        {
            this._RefreshToken = new SpotifyUserRefreshToken(oToken);
            SaveRefreshTokenToDatebase();
            this._AccessToken = new SpotifyUserAccessToken(oToken);
            SaveAccessTokenToDatabase();
        }



    }
}
