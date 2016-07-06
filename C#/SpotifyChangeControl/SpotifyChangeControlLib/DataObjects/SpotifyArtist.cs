using SpotifyChangeControlLib.Types.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyArtist : SpotifyObjectBase
    {
        
        public SpotifyArtist(SimpleArtist oSpotifySimpleArtist) : base(oSpotifySimpleArtist.Id, oSpotifySimpleArtist.Name)
        {

        }
    }
}
