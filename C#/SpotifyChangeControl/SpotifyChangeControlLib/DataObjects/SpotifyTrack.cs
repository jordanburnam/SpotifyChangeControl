using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyTrack : SpotifyObjectBase
    {
        private IEnumerable<SpotifyArtist> _Artists; 


        public IEnumerable<SpotifyArtist> Artists
        {
            get { return this._Artists; }
            
        }


        protected SpotifyTrack(long iID, string sSpotifyID, string sName, IEnumerable<SpotifyArtist> oArtists) :base(iID, sName)
        {
            this.SpotifyID = sSpotifyID;
            this._Artists = oArtists;
        }
        public SpotifyTrack(PlaylistTrack oPlaylistTrack, IEnumerable<SpotifyArtist> oArtists):base(oPlaylistTrack.Track.Id, oPlaylistTrack.Track.Name)
        {
            this._Artists = oArtists;
        }

       
    }
}
