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
    public class UserManager
    {
        
       

        private SpotifyUser[] _Users;
        private Dictionary<Int64, List<SpotifyPlaylist>> _dicUserPlaylists;






        public UserManager()
        {
            this._Users = SpotifyAccessLayer.GetSpotifyUsersAndTokens().ToArray<SpotifyUser>();
            this._dicUserPlaylists = new Dictionary<Int64, List<SpotifyPlaylist>>();
            
        }

        public void GetPlaylistsFromUsers()
        {
            foreach (SpotifyUser oSpotifyUser in this._Users)
            {
                List<SpotifyPlaylist> Playlists = new List<SpotifyPlaylist>();
                Playlists.AddRange(oSpotifyUser.Playlist);
                    
                if (Playlists != null)
                {
                    if (Playlists.ToArray().Length > 0)
                    {
                        
                        this._dicUserPlaylists.Add(oSpotifyUser.ID, Playlists);
                        
                    }
                }

            }
        }

        public void GetTracksFromPlaylists()
        {
            foreach (Int64 iUserID in this._dicUserPlaylists.Keys)
            {

            }
        }

    }
}
