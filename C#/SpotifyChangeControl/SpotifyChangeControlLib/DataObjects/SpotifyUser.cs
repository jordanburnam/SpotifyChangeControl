using System;
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
    public class SpotifyUser : SpotifyObjectBase
    {

        private SpotifyUserAuth _UserAuth;
        private SpotifyUserRefreshToken _RefreshToken;
        private SpotifyUserAccessToken _AccessToken;

        private IEnumerable<SpotifyPlaylist> _Playlists;



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

        public SpotifyUser(DataRow drUser) : base(Convert.ToInt64(drUser["UserID"].ToString()), drUser["Name"].ToString())
        {
            this.Name = drUser["Name"].ToString();
            string sAuthCode = drUser["AuthCode"].ToString();
            DateTime dtAuth = Convert.ToDateTime(drUser["AuthDate"].ToString());
            string sAccessCode = drUser["AccessTokenCode"].ToString();
            string sRefreshCode = drUser["RefreshTokenCode"].ToString();
            string sAccessTokenType = drUser["AccessTokenTokenType"].ToString();
            int iAccessExpiresIn = Convert.ToInt32(drUser["AccessTokenExpiresIn"].ToString());
            DateTime dtToken = Convert.ToDateTime(drUser["AccessTokenCreatedDate"].ToString());
            bool bHasBeenTokenized = Convert.ToBoolean(drUser["AuthCodeTokenized"].ToString());
            this._UserAuth = new SpotifyUserAuth(sAuthCode, bHasBeenTokenized, dtAuth);
            this._RefreshToken = new SpotifyUserRefreshToken(sRefreshCode, dtToken);
            this._AccessToken = new SpotifyUserAccessToken(sAccessCode, sAccessTokenType, iAccessExpiresIn, dtToken);

        }

        public SpotifyUser(string sSpotifyID, string sUserName, string sAuthCode, bool bHasBeenTokenized, DateTime dtAuth, Token oToken): base(sSpotifyID, sUserName)
        {

            string sAccessCode = oToken.AccessToken;
            string sRefreshCode = oToken.RefreshToken;
            string sAccessTokenType = oToken.RefreshToken;
            int iAccessExpiresIn = oToken.ExpiresIn;
            DateTime dtToken = oToken.CreateDate;

            this._UserAuth = new SpotifyUserAuth(sAuthCode, bHasBeenTokenized, dtAuth);
            SaveAuthToDatabase();
            this._RefreshToken = new SpotifyUserRefreshToken(sRefreshCode, dtToken);
            SaveRefreshTokenToDatebase();
            this._AccessToken = new SpotifyUserAccessToken(sAccessCode, sAccessTokenType, iAccessExpiresIn, dtToken);
            SaveAccessTokenToDatabase();
        }

        private void SaveAuthToDatabase()
        {
            SpotifyAccessLayer.SaveAuthTokenForUserToDatabase(this.ID, this._UserAuth);
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
