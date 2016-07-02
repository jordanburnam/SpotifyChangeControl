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
    internal class UserManager: SpotifyUser
    {
        
       

        private SpotifyUser[] _Users;
        private Dictionary<Int64, List<SpotifyPlaylist>> _UserPlaylists;

        
        private int _iCurrentIndex;
           





        public UserManager():base()
        {
            this._Users = SpotifyAccessLayer.GetSpotifyUsersAndTokens().ToArray<SpotifyUser>();
            this._UserPlaylists = new Dictionary<Int64, List<SpotifyPlaylist>>();
            this._iCurrentIndex = 0; 
            
            
        }

        private void GetPlaylistsFromUsers()
        {
            foreach (SpotifyUser oSpotifyUser in this._Users)
            {
                List<SpotifyPlaylist> Playlists = new List<SpotifyPlaylist>();
                Playlists.AddRange(oSpotifyUser.Playlist);
                    
                if (Playlists != null)
                {
                    if (Playlists.ToArray().Length > 0)
                    {
                        
                        this._UserPlaylists.Add(oSpotifyUser.ID, Playlists);
                        
                    }
                }

            }
        }

        public bool Read()
        {
          
            if (_iCurrentIndex > this._Users.Length - 1)
            {
                this._iCurrentIndex = 0; //All users returned reset this before we return
                return false;
            }
            SpotifyUser oSpotifyUser = this._Users[this._iCurrentIndex];
            this._iCurrentIndex++;


            this._ID = oSpotifyUser.ID;
            this._SpotifyID = oSpotifyUser.SpotifyID;
            this._Name = oSpotifyUser.Name;
            this._UserAuth = oSpotifyUser.UserAuth;
            this._RefreshToken = oSpotifyUser.RefreshToken;
            this._AccessToken = oSpotifyUser.AccessToken;
            this._Playlists = oSpotifyUser.Playlist;


            return true;
        }

    }
}
