using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI.Models;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyTrack : SpotifyObjectBase
    {
        private IEnumerable<SpotifyArtist> _Artists; 


        public IEnumerable<SpotifyArtist> Artists
        {
            get { return this._Artists; }
            
        }



        public SpotifyTrack(PlaylistTrack oPlaylistTrack, IEnumerable<SpotifyArtist> oArtists):base(oPlaylistTrack.Track.Id, oPlaylistTrack.Track.Name)
        {
            this._Artists = oArtists;
        }

       
    }
}
