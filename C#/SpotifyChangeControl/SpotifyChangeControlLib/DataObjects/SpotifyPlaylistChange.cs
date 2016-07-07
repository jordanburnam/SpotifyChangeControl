﻿using System;
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
        protected string _PlaylistName;
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

        public DateTime ChangedDate
        {
            get { return this._ChangedDate; }
        }

        protected SpotifyPlaylistChange()
        {

        }

        public SpotifyPlaylistChange(long iUserID, string sUserGuid, string sUserName, string sPlaylistName, string sTrackName, char cChangeType, string[] ArtistsNames, DateTime dtChangedDate)
        {
            //The database will return multiple rows because the only difference are the artist when there are more than one artsit on the track
            this._UserID = iUserID;
            this._UserGuid = sUserGuid;
            this._UserName = sUserName;
            this._PlaylistName = sPlaylistName;
            this._TrackName = sTrackName;
            this._ChangeType = cChangeType;
            this._ArtistNames = ArtistsNames;
            this._ChangedDate = dtChangedDate;

        }


    }
}
