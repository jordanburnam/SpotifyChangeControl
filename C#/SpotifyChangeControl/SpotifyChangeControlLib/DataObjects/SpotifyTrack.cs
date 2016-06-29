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
        private SpotifyArtist _Artist; 


        public SpotifyArtist Artist
        {
            get { return this._Artist; }
            
        }

        public SpotifyTrack(string sSpotifyID, string sName, SpotifyArtist oArtist): base(sSpotifyID, sName)
        {
            this._Artist = oArtist;
        }

        public SpotifyTrack(SimpleTrack oSpotifySimpleTrack, SpotifyArtist oArtist) : base(oSpotifySimpleTrack.Id, oSpotifySimpleTrack.Name)
        {
            this._Artist = oArtist;
        }

        public SpotifyTrack(FullTrack oSpotifyFullTrack, SpotifyArtist oAritst) : base(oSpotifyFullTrack.Id, oSpotifyFullTrack.Name)
        {
            this._Artist = oAritst;
        }
    }
}
