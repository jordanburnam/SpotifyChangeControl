using SpotifyChangeControlLib.DataManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyChangeControlLib.DataObjects;
using SpotifyAPI.Web.Models;
using SpotifyChangeControlLib.AccessLayer;

namespace SpotifyChangeControlLib
{
    public class SCCManager
    {
        private string _ClientPrivate;
        private string _ClientPublic;
        private StateManager _oStateManager;

        private string _SCC_PRIVATE_ID;
        private string _SCC_PUBLIC_ID;
        private string _SCC_SQL_CON;
        private string _SCC_REDIS_HOST;
        private string _SCC_REDIS_PASS;
        private int _SCC_REDIS_PORT;
        private SCCManager _oSCCManager;
        private string _RedirectUrl;

        public string SCC_PRIVATE_ID
        {
            get { return _SCC_PRIVATE_ID; }
        }

        public string SCC_PUBLIC_ID
        {
            get { return _SCC_PUBLIC_ID; }
        }

        public string RedirectUrl
        {
            get { return _RedirectUrl; }
        }

        public SCCManager oSCCManager
        {
            get { return _oSCCManager; }
        }

        public SCCManager()
        {

            string sRedirectUrl = System.Environment.GetEnvironmentVariable("SCC_REDIRECT_URL");
            string sClientPrivate = System.Environment.GetEnvironmentVariable("SCC_PRIVATE_ID");
            string sClientPublic = System.Environment.GetEnvironmentVariable("SCC_PUBLIC_ID");

            string sSQLConnection = System.Environment.GetEnvironmentVariable("SCC_SQL_CON");

            string sHost = System.Environment.GetEnvironmentVariable("SCC_REDIS_HOST");
            string sPassword = System.Environment.GetEnvironmentVariable("SCC_REDIS_PASS");
            string sPort = System.Environment.GetEnvironmentVariable("SCC_REDIS_PORT");
            int iPort;
            if (!int.TryParse(sPort, out iPort))
            {
                iPort = 6379;
            }


            this._SCC_PRIVATE_ID = sClientPrivate;
            this._SCC_PUBLIC_ID = sClientPublic;
            this._RedirectUrl = sRedirectUrl;
            SpotifyChangeControlLib.StorageLayer.CacheDatabase.Init(sHost, iPort, sPassword);
            SpotifyChangeControlLib.StorageLayer.RelationalDatabase.Init(sSQLConnection);
            SpotifyChangeControlLib.AccessLayer.SpotifyDatabase.Init(_ClientPublic, _ClientPrivate, sRedirectUrl);
           
            this._oStateManager = new StateManager();
            
        }

        public void ExecuteStateMananger()
        {
            this._oStateManager.Run(); //They will notice we are running we are we are out of breath but we will still claim our innoncence!
        }

        public SpotifyUser AddSpotifyUser(PrivateProfile oPrivateProfile, string sAuthCode, Token oToken)
        {
            SpotifyUser oSpotifyUser = new SpotifyUser(oPrivateProfile, sAuthCode, oToken);
            return oSpotifyUser;
        }

        public SpotifyPlaylistChangeManager  GetUserForUserGuid(string sUserGuid, DateTime? dtLastSeen = null)
        {

            SpotifyPlaylistChange[] Changes = SpotifyAccessLayer.GetSpotifyUserForGuid(sUserGuid).Changes.ToArray();
            SpotifyPlaylistChange[] ChangesFilteredWithDate;
            if (dtLastSeen != null)
            {
                ChangesFilteredWithDate = (from Change in Changes where Change.ChangedDate > dtLastSeen.Value.ToUniversalTime() select Change).ToArray();
            }
            else
            {
                ChangesFilteredWithDate = Changes;
            }
            return new SpotifyPlaylistChangeManager(ChangesFilteredWithDate);
        }

        public bool AuthenticateUserSession(string sUserGuid, string sSessionGuid)
        {
            return SpotifyAccessLayer.AuthenticateUserWithSession(sUserGuid, sSessionGuid);
        }

        public void DeleteUserSession(string sUserGuid)
        {
            long iUserID = SpotifyAccessLayer.GetSpotifyUserForGuid(sUserGuid).ID;
            SpotifyAccessLayer.DeleteSessionForUser(iUserID);
        }

    }
}
