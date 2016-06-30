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
        private UserManager _oUserManager;
        public SCCManager(string sClientPrivate, string sClientPublic, string sSQLConnection, string sHost, int iPort, string sPassword)
        {
            this._ClientPrivate = sClientPrivate;
            this._ClientPublic = sClientPublic;

            SpotifyChangeControlLib.StorageLayer.CacheDatabase.Init(sHost, iPort, sPassword);
            SpotifyChangeControlLib.StorageLayer.RelationalDatabase.Init(sSQLConnection);
            SpotifyChangeControlLib.AccessLayer.SpotifyDatabase.Init(_ClientPublic, _ClientPrivate);
            _oUserManager = new UserManager();
            
        }

       
    }
}
