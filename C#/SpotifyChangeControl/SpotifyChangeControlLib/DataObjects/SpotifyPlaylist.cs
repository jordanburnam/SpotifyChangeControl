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
        private string _Owner;

        private Dictionary<int, SpotifyTrack> _Tracks;

       public Dictionary<int, SpotifyTrack> Tracks
        {
            get { return this._Tracks; }
        }

         public string Owner
        {
            get { return this._Owner; }
        }


        public SpotifyPlaylist(long iID, string sSpotifyID, string sName, string sOwner, Dictionary<int, SpotifyTrack> oTracks) : base(iID, sName)
        {
            this.SpotifyID = sSpotifyID;
            this._Tracks = oTracks;
            this._Owner = sOwner;
        }

        public SpotifyPlaylist(SimplePlaylist oSpotifySimplePlaylist, Dictionary<int, SpotifyTrack> oTracks) : base(oSpotifySimplePlaylist.Id, oSpotifySimplePlaylist.Name)
        {
            this._Tracks = oTracks;
            this._Owner = oSpotifySimplePlaylist.Owner.Id;
        }
       
       

     

    }
}
