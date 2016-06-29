using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI.Models;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyPlaylist : SpotifyObjectBase 
    {
        private IEnumerable<SpotifyTrack> _Tracks;
        public SpotifyPlaylist(string sSpotifyID, string sName):base(sSpotifyID, sName)
        {
           
        }
        public SpotifyPlaylist(string sSpotifyID, string sName, params SpotifyTrack[] oTracks): base(sSpotifyID, sName)
        {
            this._Tracks = oTracks;
        }

        public SpotifyPlaylist(SimplePlaylist oSpotifySimplePlaylist, params SpotifyTrack[] oTracks) : base(oSpotifySimplePlaylist.Id, oSpotifySimplePlaylist.Name)
        {
            this._Tracks = oTracks;
        }
        public SpotifyPlaylist(FullPlaylist oSpotifyFullPlaylist, params SpotifyTrack[] oTracks) : base(oSpotifyFullPlaylist.Id, oSpotifyFullPlaylist.Name)
        {
            this._Tracks = oTracks;
        }

        public void AddTracks(params SpotifyTrack[] oTracks)
        {
            List<SpotifyTrack> liTracks = new List<SpotifyTrack>();
            liTracks.AddRange(this._Tracks);
            liTracks.AddRange(oTracks);
            this._Tracks = liTracks;
        }

     

    }
}
