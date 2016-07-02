using SpotifyChangeControlLib.DataManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyChangeControlLib.DataObjects;
using SpotifyAPI.SpotifyWebAPI.Models;

namespace SpotifyChangeControlLib
{
    public class SCCManager
    {
        private string _ClientPrivate;
        private string _ClientPublic;
       
        private StateManager _oStateManager;
        public SCCManager(string sClientPrivate, string sClientPublic, string sRedirectURL, string sSQLConnection, string sHost, int iPort, string sPassword)
        {
            this._ClientPrivate = sClientPrivate;
            this._ClientPublic = sClientPublic;

            SpotifyChangeControlLib.StorageLayer.CacheDatabase.Init(sHost, iPort, sPassword);
            SpotifyChangeControlLib.StorageLayer.RelationalDatabase.Init(sSQLConnection);
            SpotifyChangeControlLib.AccessLayer.SpotifyDatabase.Init(_ClientPublic, _ClientPrivate, sRedirectURL);
           
            this._oStateManager = new StateManager();
            
        }

        public void ExecuteStateMananger()
        {
            this._oStateManager.Run(); //They will notice we are running we are we are out of breath but we will still claim our innoncence!
        }

        public Int64 AddSpotifyUser(PrivateProfile oPrivateProfile, string sAuthCode, Token oToken)
        {
            SpotifyUser oSpotifyUser = new SpotifyUser(oPrivateProfile, sAuthCode, oToken);
            return oSpotifyUser.ID;
        }
       
    }
}
