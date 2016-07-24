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
        private string _Owner;
        private IEnumerable<SpotifyTrackChange> _Changes;

        public IEnumerable<SpotifyTrackChange> Changes
        {
            get { return this._Changes; }
        }

        public string Owner
        {
            get { return this._Owner; }
        }
        public SpotifyPlaylistChange(long iID, string sSpotifyID, string sName, string sOwner, IEnumerable<SpotifyTrackChange> oTracks) : base(iID, sName)
        {
            this.SpotifyID = sSpotifyID;
            this._Changes = oTracks;
            this._Owner = sOwner;
        }
    }
}
