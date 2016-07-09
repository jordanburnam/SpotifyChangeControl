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



        public SpotifyPlaylist(SimplePlaylist oSpotifySimplePlaylist, Dictionary<int, SpotifyTrack> oTracks) : base(oSpotifySimplePlaylist.Id, oSpotifySimplePlaylist.Name)
        {
            this._Tracks = oTracks;
        }
       
       

     

    }
}
