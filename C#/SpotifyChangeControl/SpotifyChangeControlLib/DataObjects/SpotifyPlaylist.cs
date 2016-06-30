using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib.AccessLayer;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyPlaylist : SpotifyObjectBase 
    {
        private IEnumerable<SpotifyTrack> _Tracks;

       public IEnumerable<SpotifyTrack> Tracks
        {
            get { return this._Tracks; }
        }



        public SpotifyPlaylist(SimplePlaylist oSpotifySimplePlaylist, IEnumerable<SpotifyTrack> oTracks) : base(oSpotifySimplePlaylist.Id, oSpotifySimplePlaylist.Name)
        {
            this._Tracks = oTracks;
        }
       
       

     

    }
}
