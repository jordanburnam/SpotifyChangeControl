using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;
using SpotifyChangeControlLib.AccessLayer;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyPlaylist : SpotifyObjectBase 
    {
        private Dictionary<int, SpotifyTrack> _Tracks;

       public Dictionary<int, SpotifyTrack> Tracks
        {
            get { return this._Tracks; }
        }

        public SpotifyPlaylist(long iID, string sSpotifyID, string sName, Dictionary<int, SpotifyTrack> oTracks) : base(iID, sName)
        {
            this.SpotifyID = sSpotifyID;
            this._Tracks = oTracks;
        }

        public SpotifyPlaylist(SimplePlaylist oSpotifySimplePlaylist, Dictionary<int, SpotifyTrack> oTracks) : base(oSpotifySimplePlaylist.Id, oSpotifySimplePlaylist.Name)
        {
            this._Tracks = oTracks;
        }
       
       

     

    }
}
