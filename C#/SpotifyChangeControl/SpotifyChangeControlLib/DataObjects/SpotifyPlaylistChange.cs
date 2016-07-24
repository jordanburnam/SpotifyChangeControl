using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SpotifyChangeControlLib.Types.Abstract;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyPlaylistChange : SpotifyObjectBase
    {
        protected IEnumerable<SpotifyTrackChange> _Changes;

        public IEnumerable<SpotifyTrackChange> Changes
        {
            get { return this._Changes; }
        }

        public SpotifyPlaylistChange(long iID, string sSpotifyID, string sName, IEnumerable<SpotifyTrackChange> oTracks) : base(iID, sName)
        {
            this.SpotifyID = sSpotifyID;
            this._Changes = oTracks;
        }
    }
}
