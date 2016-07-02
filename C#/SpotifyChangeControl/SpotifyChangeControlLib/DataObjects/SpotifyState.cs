using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.DataObjects
{
    public class SpotifyState
    {
        private long _UserID;
        private long _PlaylistID;
        private long _TrackID;
        private int _Position;
        private long _ArtistID;

        public long UserID
        {
            get { return this._UserID; }
        }

        public long PlaylistID
        {
            get { return this._PlaylistID; }
        }

        public long TrackID
        {
            get { return this._TrackID; }
        }
        public int Position
        {
            get { return this._Position; }
        }
        public long ArtistID
        {
            get { return this._ArtistID; }
        }



        public SpotifyState(long iUserID, long iPlaylistID, long iTrackID, int iPosition, long iArtistID)
        {
            this._ArtistID = iArtistID;
            this._PlaylistID = iPlaylistID;
            this._TrackID = iTrackID;
            this._Position = iPosition;
            this._UserID = iUserID;
        }

    }
}
