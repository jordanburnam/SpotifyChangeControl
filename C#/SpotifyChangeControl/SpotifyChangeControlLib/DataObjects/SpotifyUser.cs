using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyChangeControlLib.Utilities;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyUser
    {
        private int _UserID;
        private string _AccessToken;
        private string _RefreshToken;
        private bool _AccessExpired;
        private int _ExpiresIn;
        private DateTime _TokenUpdatedDate;
        private string _PropertyHash; //Used to know if the object has changed since it was instantiated!

        public bool HasChanged
        {
            get { return this.GetHasChanged(); }
        }


        public SpotifyUser(DataRow drUserAndTokenInfo)
        {
            int iUserID = Convert.ToInt32(drUserAndTokenInfo["UserID"].ToString());
            string sAccessToken = drUserAndTokenInfo["AccessToken"].ToString();
            string sRefreshToken = drUserAndTokenInfo["RefreshToken"].ToString();
            bool bAccessExpired = Convert.ToBoolean(drUserAndTokenInfo["AccessExpired"].ToString());
            int iExpiresIn = Convert.ToInt32(drUserAndTokenInfo["ExpiresIn"].ToString());
            DateTime dtTokenUpdatedDate = Convert.ToDateTime(drUserAndTokenInfo["TokenUpdatedDate"].ToString());

            this.Instantiate(iUserID, sAccessToken, sRefreshToken, bAccessExpired, iExpiresIn, dtTokenUpdatedDate);
        }
        public SpotifyUser(int iUserID, string sAccessToken, string sRefreshToken, bool bAccessExpired, int iExpiresIn, DateTime dtTokenUpdatedDate)
        {
            this.Instantiate(iUserID, sAccessToken, sRefreshToken, bAccessExpired, iExpiresIn, dtTokenUpdatedDate);
        }

        private void Instantiate(int iUserID, string sAccessToken, string sRefreshToken, bool bAccessExpired, int iExpiresIn, DateTime dtTokenUpdatedDate)
        {
            this._UserID = iUserID;
            this._AccessToken = sAccessToken;
            this._AccessExpired = bAccessExpired;
            this._ExpiresIn = iExpiresIn;
            this._TokenUpdatedDate = dtTokenUpdatedDate;

            string sPropertyHash = HashSlingingSlasher.HashObjects(this._UserID, this._AccessToken, this._RefreshToken, this._AccessExpired, this._ExpiresIn, this._TokenUpdatedDate);
            this._PropertyHash = sPropertyHash;
            

        }
        private bool GetHasChanged()
        {
            bool bChanged = false;
            string sPropertyHash = HashSlingingSlasher.HashObjects(this._UserID, this._AccessToken, this._RefreshToken, this._AccessExpired, this._ExpiresIn, this._TokenUpdatedDate);

            if (!this._PropertyHash.Equals(sPropertyHash)) //If the hashes do not match then this was changed
            {
                bChanged = true;
            }

            return bChanged;
        }
        public void SetAccessToken(string sAccessToken, string sRefreshToken, int iExpiresIn)
        {
            //Make sure the access token and the refresh token are not the same as they were before
            if (!sAccessToken.Equals(this._AccessToken))
            {
                if (!sAccessToken.Equals(this._AccessToken))
                {
                    //All things seem to check out update the access, refresh, and expires in then change the TokenUpdatedDate
                    this._AccessToken = sAccessToken;
                    this._RefreshToken = sRefreshToken;
                    this._ExpiresIn = iExpiresIn;
                    this._TokenUpdatedDate = DateTime.Now;
                }
                else //The refresh token is the same
                {
                    throw new Exception(string.Format("The SpotifyUser with id of '{0}' was attempted to update the refresh token with the same refresh token as before. This is prohibited!", this._UserID));
                }
            }
            else //The Access Token is the same
            {
                throw new Exception(string.Format("The SpotifyUser with id of '{0}' was attempted to update the access token with the same access token as before. This is prohibited!", this._UserID));
            }
        }
    }
}
