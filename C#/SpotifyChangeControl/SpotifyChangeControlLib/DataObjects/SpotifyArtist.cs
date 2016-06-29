using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI.Models;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyArtist : SpotifyObjectBase
    {
        public SpotifyArtist(string sSpotifyID, string sName):base(sSpotifyID, sName)
        {
            ErrorResponse err;
            
        }
        public SpotifyArtist(FullArtist oSpotifyFullArtist): base(oSpotifyFullArtist.Id, oSpotifyFullArtist.Name)
        {

        }

        public SpotifyArtist(SimpleArtist oSpotifySimpleArtist) : base(oSpotifySimpleArtist.Id, oSpotifySimpleArtist.Name)
        {

        }
    }
}
