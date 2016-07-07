using SpotifyChangeControlLib.AccessLayer;
using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace SpotifyChangeControlLib.DataManagers
{
    internal class StateManager
    {
        private Dictionary<Int64, SpotifyArtist> _Artists;
        private Dictionary<Int64, SpotifyTrack> _Tracks;
        private Dictionary<Int64, SpotifyPlaylist> _Playlists;

        private List<SpotifyState> _SpotifyStates;

        private UserManager _oUserManager;

        private WorkTableState _WKAtrtist;
        private WorkTableState _WKTracks;
        private WorkTableState _WKPlaylists;
        private WorkTableState _WKState;

        internal StateManager()
        {
            this._Artists = new Dictionary<long, SpotifyArtist>();
            this._Tracks = new Dictionary<long, SpotifyTrack>();
            this._Playlists = new Dictionary<long, SpotifyPlaylist>();
            this._SpotifyStates = new List<SpotifyState>();
            this._oUserManager = new UserManager();
            this._WKAtrtist = new WorkTableState("Spotify_WK_Artist", "dbo");
            this._WKTracks = new WorkTableState("Spotify_WK_Track", "dbo");
            this._WKPlaylists = new WorkTableState("Spotify_WK_Playlist", "dbo");
            this._WKState = new WorkTableState("Spotify_WK_State", "dbo");
  
        }
        private void GetStates()
        {
            while (this._oUserManager.Read())
            {
                foreach (SpotifyPlaylist oSpotifyPlaylist in this._oUserManager.Playlist)
                {
                    
                    if (!this._Playlists.ContainsKey(oSpotifyPlaylist.ID))
                    {
                        this._Playlists.Add(oSpotifyPlaylist.ID, oSpotifyPlaylist);
                    }
                    foreach (KeyValuePair<int, SpotifyTrack> oKVP in oSpotifyPlaylist.Tracks)
                    {
                        if (!this._Tracks.ContainsKey(oKVP.Value.ID))
                        {
                            this._Tracks.Add(oKVP.Value.ID, oKVP.Value);
                        }
                        foreach (SpotifyArtist oSpotifyArtist in oKVP.Value.Artists)
                        {
                            if (!this._Artists.ContainsKey(oSpotifyArtist.ID))
                            {
                                this._Artists.Add(oSpotifyArtist.ID, oSpotifyArtist);
                            }
                            this._SpotifyStates.Add(new SpotifyState(this._oUserManager.ID, oSpotifyPlaylist.ID, oKVP.Value.ID, oKVP.Key, oSpotifyArtist.ID));
                        }
                    }
                }
            }
        }

        private void TruncateWorkTables()
        {
            this._WKAtrtist.TruncateTable();
            this._WKTracks.TruncateTable();
            this._WKPlaylists.TruncateTable();
            this._WKState.TruncateTable();
            
        }

        private void SaveStates()
        {
            SpotifyAccessLayer.SaveArtistsToDatabase(this._Artists, this._WKAtrtist);
            SpotifyAccessLayer.SaveTracksToDatabase(this._Tracks, this._WKTracks);
            SpotifyAccessLayer.SavePlaylistsToDatabase(this._Playlists, this._WKPlaylists);
            SpotifyAccessLayer.SaveStatesToDatabase(this._SpotifyStates, this._WKState);
        }


        public void SendNotification()
        {

        }

        public void Run()
        {

            this.TruncateWorkTables();
            this.GetStates();
            this.SaveStates();
        }
    }
}
