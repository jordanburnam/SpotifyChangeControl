using SpotifyChangeControlLib.AccessLayer;
using SpotifyChangeControlLib.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SpotifyChangeControlLib.Types.Enums;

namespace Mvc.Client.Models.Changes
{
    public class ResultModel
    {
        private SearchModel _oSearchModel;
        private IEnumerable<SpotifyPlaylistChange> _PlaylistChanges;
      

        public IEnumerable<SpotifyPlaylistChange> PlaylistChanges
        {
            get { return this._PlaylistChanges; }
        }

       

        public ResultModel(long iUserID, SearchModel oSearchModel)
        {
            this._oSearchModel = oSearchModel;
            SpotifyChangeControlLib.SCCManager oSSCManager = new SpotifyChangeControlLib.SCCManager();
            this._PlaylistChanges = oSSCManager.GetPlaylistChangesForUser(1, oSearchModel.Start, oSearchModel.End);
        }

    }
}
