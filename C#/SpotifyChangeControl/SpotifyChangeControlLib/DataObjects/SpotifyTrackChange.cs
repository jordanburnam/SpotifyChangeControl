using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;
using static SpotifyChangeControlLib.Types.Enums;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyTrackChange : SpotifyTrack
    {


        private SpotifyChangeType _ChangeType;
        private DateTime _ChangedDate;

        public SpotifyChangeType ChangeType
        {
            get { return this._ChangeType; }
        }

        public DateTime ChangedDate
        {
            get { return this._ChangedDate; }
        }
        
        public SpotifyTrackChange(long iID, string sSpotifyID, string sName, IEnumerable<SpotifyArtist> oArtists, SpotifyChangeType eChangeType, DateTime dtChanged):base(iID, sSpotifyID, sName, oArtists)
        {
            this._ChangeType = eChangeType;
            this._ChangedDate = dtChanged;
        }
    }
}
