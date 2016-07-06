using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyPlaylistChange
    {
        private long _UserID;
        private string _UserGuid;
        private string _UserName;
        private string _PlaylistName;
        private string _TrackName;
        private string[] _ArtistNames;
        private char _ChangeType;

        public long UserID
        {
            get { return this._UserID; }
        }
        public string UserGuid
        {
            get { return this._UserGuid; }
        }
        public string UserName
        {
            get { return this._UserName; }
        }
        public string PlaylistName
        {
            get { return this._PlaylistName; }
        }
        public string TrackName
        {
            get { return this._TrackName; }
        }
        public string[] ArtistNames
        {
            get { return this._ArtistNames; }
        }

        public char ChangeType
        {
            get { return this._ChangeType; }
        }
        public SpotifyPlaylistChange(long iUserID, string sUserGuid, string sUserName, string sPlaylistName, string sTrackName, char cChangeType, string[] ArtistsNames)
        {
            //The database will return multiple rows because the only difference are the artist when there are more than one artsit on the track
            this._UserID = iUserID;
            this._UserGuid = sUserGuid;
            this._UserName = sUserName;
            this._PlaylistName = sPlaylistName;
            this._TrackName = sTrackName;
            this._ChangeType = cChangeType;
            this._ArtistNames = ArtistsNames;

        }


    }
}
