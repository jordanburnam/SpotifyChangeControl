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

       

        public ResultModel(SearchModel oSearchModel, IEnumerable<SpotifyPlaylistChange> oPlaylistChanges)
        {
            this._oSearchModel = oSearchModel;
            this._PlaylistChanges = oPlaylistChanges;
           
        }

    }
}
