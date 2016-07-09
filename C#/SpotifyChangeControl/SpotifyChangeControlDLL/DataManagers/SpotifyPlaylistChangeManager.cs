using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.DataManagers
{
    public class SpotifyPlaylistChangeManager : SpotifyPlaylistChange
    {
        private string[] _PlaylistNames;
        private string[] _TrackNames;
        private int _AddCount;
        private int _MoveCount;
        private int _DeleteCount;
        private int _iCurrentIndex;
        private string _sCurrentPlaylistName;
        private SpotifyPlaylistChange[] _Changes;

        public string[] PlaylistNames
        {
            get { return this._PlaylistNames; }
        }

        public string[] TrackNames
        {
            get { return this._TrackNames; }
        }

        public int ChangeCount
        {
            get { return this._AddCount + this._MoveCount + this._DeleteCount; }
        }

        public int AddCount
        {
            get { return this._AddCount; }
        }

        public int MoveCount
        {
            get { return this._MoveCount; }
        }

        public int DeleteCount
        {
            get { return this._DeleteCount; }
        }
        public SpotifyPlaylistChangeManager(SpotifyPlaylistChange[] oChanges)
        {
            this._Changes = oChanges;
            this._PlaylistNames = (from Change in this._Changes select Change.PlaylistName).Distinct().ToArray();
            this._TrackNames = (from Change in this._Changes select Change.TrackName).Distinct().ToArray();
            this._AddCount = (from Change in this._Changes where Change.ChangeType == 'A' select Change).ToArray().Length;
            this._MoveCount = (from Change in this._Changes where Change.ChangeType == 'M' select Change).ToArray().Length;
            this._DeleteCount = (from Change in this._Changes where Change.ChangeType == 'D' select Change).ToArray().Length;
        }

        public bool ReadForPlaylistName(string sPlaylistName)
        {
            SpotifyPlaylistChange[] ChangesForPlaylist = (from Change in this._Changes where Change.PlaylistName.Equals(sPlaylistName) select Change).ToArray();

            if (_sCurrentPlaylistName == null)
            {
                _sCurrentPlaylistName = sPlaylistName;
            }

            if (!this._sCurrentPlaylistName.Equals(sPlaylistName))
            {
                this._iCurrentIndex = 0;
            }

            if (_iCurrentIndex > ChangesForPlaylist.Length - 1)
            {
                this._iCurrentIndex = 0; 
                return false;
            }
            SpotifyPlaylistChange oSpotifyPlaylistChange = this._Changes[this._iCurrentIndex];

            this._UserID = oSpotifyPlaylistChange.UserID;
            this._UserGuid = oSpotifyPlaylistChange.UserGuid;
            this._UserName = oSpotifyPlaylistChange.UserName;
       
            this._PlaylistName = oSpotifyPlaylistChange.PlaylistName;
            this._TrackName = oSpotifyPlaylistChange.TrackName;
            this._ArtistNames = oSpotifyPlaylistChange.ArtistNames;
            this._ChangeType = oSpotifyPlaylistChange.ChangeType;

            return true;
        }

        public IEnumerable<SpotifyPlaylistChange> GetNewTracksSortedByPlaylistName()
        {
            return (from Change in this._Changes where Change.ChangeType == 'A' select Change).OrderBy(x => x.PlaylistName).ToArray();
        }

        public string GetMessageForPlaylist(string sPlaylistName)
        {
            string sMessage;
            SpotifyPlaylistChange[] oChangesForPlaylist = (from Change in _Changes where Change.PlaylistName.Equals(sPlaylistName) select Change).ToArray();
            int iAdds = (from Change in oChangesForPlaylist where Change.ChangeType == 'A' select Change).ToArray().Length;
            int iDeletes = (from Change in oChangesForPlaylist where Change.ChangeType == 'D' select Change).ToArray().Length;
            int iMoves = (from Change in oChangesForPlaylist where Change.ChangeType == 'M' select Change).ToArray().Length;
            if (iMoves > 0 && iAdds == 0 && iDeletes == 0)
            {
                sMessage = "Playlist resorted, No tracks added or renoved!";
            }
            else
            {
                if (iAdds > 0)
                {
                    sMessage = "Added " + iAdds.ToString() + " track" + (iAdds > 1 ? "s!":"!");
                }
                else
                {
                    sMessage = "Deleted " + iDeletes.ToString() + " track" + (iDeletes > 1 ? "s!":"!");
                }
            }

            return sMessage;
        }
    }
}
