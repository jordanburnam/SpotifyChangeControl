using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib.AccessLayer;

namespace SpotifyChangeControlLib.DataManagers
{
    /// <summary>
    /// This will be used mostly to get the next user
    /// and when that happens to automagically update the token
    /// if it has expired and to do that it would need to call the 
    /// spotify api and requqest a new token. 
    /// </summary>
    internal class UserManager
    {
        
        private string _ClientPublic;
        private string _ClientPrivate;

        public SpotifyUser[] _Users;
        private SpotifyUser _CurrentUser;
        private int _CurrentIndex;

        private AutorizationCodeAuth _oAuth;

        public SpotifyUser CurrentUser
        {
            get { return this._CurrentUser;  }
        }

   

        public UserManager(string sClientPrivate, string sClientPublic)
        {
            this._ClientPrivate = sClientPrivate;
            this._ClientPublic = sClientPublic;
            this._CurrentIndex = -1;
            this._Users = new SpotifyUser[0];
            this._oAuth = new AutorizationCodeAuth();
            this._oAuth.ClientId = this._ClientPublic;
            this._oAuth.Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_READ_PRIVATE | Scope.USER_FOLLOW_READ;
            this._oAuth.ShowDialog = false;
            
            this._Users = SpotifyAccessLayer.GetSpotifyUsersAndTokens().ToArray<SpotifyUser>();
        }

        

        public bool NextUser()
        {
            SpotifyUser oSpotifyUser = null;
            bool isNext = false;
            this._CurrentIndex++;
            if (this._CurrentIndex < this._Users.Length)
        {
                isNext = true;
                oSpotifyUser = this._Users[this._CurrentIndex];
                
                
                this._CurrentUser = oSpotifyUser;
            }
            else
            {
                this._CurrentIndex = -1;
                
            }


            return isNext;
        }


    }
}
