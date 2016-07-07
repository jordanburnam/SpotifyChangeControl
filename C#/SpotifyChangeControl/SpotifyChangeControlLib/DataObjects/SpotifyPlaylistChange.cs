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
        protected long _UserID;
        protected string _UserGuid;
        protected string _UserName;
        protected long _PlaylistID;
        protected string _PlaylistSpotifyID;
        protected string _PlaylistName;
        protected long _TrackID;
        protected string _TrackSpotifyID;
        protected string _TrackName;
        protected string[] _ArtistNames;
        protected char _ChangeType;
        protected DateTime _ChangedDate;

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
        public long PlaylistID
        {
            get { return this._PlaylistID; }
        }
        public string PlaylistSpotifyID
        {
            get { return this._PlaylistSpotifyID; }
        }
        public string PlaylistName
        {
            get { return this._PlaylistName; }
        }
        public long TrackID
        {
            get { return this._TrackID; }
        }
        public string TrackSpotifyID
        {
            get { return this._TrackSpotifyID; }
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

        public DateTime ChangedDate
        {
            get { return this._ChangedDate; }
        }

        protected SpotifyPlaylistChange()
        {

        }

        public SpotifyPlaylistChange(long iUserID, string sUserGuid, string sUserName, long iPlaylistID, string sPlaylistSpotifyID, string sPlaylistName, long iTrackID, string sTrackSpotifyID, string sTrackName, char cChangeType, string[] ArtistsNames, DateTime dtChangedDate)
        {
            //The database will return multiple rows because the only difference are the artist when there are more than one artsit on the track
            this._UserID = iUserID;
            this._UserGuid = sUserGuid;
            this._UserName = sUserName;
            this._PlaylistID = iPlaylistID;
            this._PlaylistSpotifyID = sPlaylistSpotifyID;
            this._PlaylistName = sPlaylistName;
            this._TrackID = iTrackID;
            this._TrackSpotifyID = sTrackSpotifyID;
            this._TrackName = sTrackName;
            this._ChangeType = cChangeType;
            this._ArtistNames = ArtistsNames;
            this._ChangedDate = dtChangedDate;

        }


    }
}
