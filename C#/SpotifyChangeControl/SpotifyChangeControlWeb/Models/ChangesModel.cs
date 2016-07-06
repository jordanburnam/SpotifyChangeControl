using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyChangeControl.Models
{
    public class ChangesModel
    {
        private long _UserID;
        private string _UserGuid;
        private SpotifyPlaylistChange[] _Changes;

        public long UserId
        {
            get { return this._UserID; }
        }

        public string UserGuid
        {
            get { return this._UserGuid; }
        }

        public SpotifyPlaylistChange[] Changes
        {
            get { return this._Changes; }
        }

        public ChangesModel(long iUserID, string sUserGuid, params SpotifyPlaylistChange[] oChanges)
        {
            this._UserID = iUserID;
            this._UserGuid = sUserGuid;
            this._Changes = oChanges;
        }

    }
}