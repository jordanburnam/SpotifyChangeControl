using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Diagnostics;

namespace SpotifyChangeControlLib.StorageLayer
{
    static class CacheDatabase
    {
        private const string _ObjectCounterKeyName = "LastSpotifyEntityID";

        private static string _Host;
        private static int _Port;
        private static bool _AuthRequired;
        private static string _Password;
        private static StackExchange.Redis.ConfigurationOptions _oConectionOptions;
        private static StackExchange.Redis.ConnectionMultiplexer _oCacheConnection;
        private static StackExchange.Redis.IDatabase _oCommand;

       


        public static void Init(string sHost, int iPort, string sPassword)
        {
            try
            {
                CacheDatabase._Host = sHost;
                CacheDatabase._Port = iPort;
                CacheDatabase._Password = sPassword;
                if (CacheDatabase._Password.Length > 0)
                {
                    CacheDatabase._AuthRequired = true;
                }
                else
                {
                    CacheDatabase._AuthRequired = false;
                }

                _oConectionOptions = new ConfigurationOptions();
                if (CacheDatabase._AuthRequired)
                {
                    CacheDatabase._oConectionOptions.Password = CacheDatabase._Password;
                }

                CacheDatabase._oConectionOptions.EndPoints.Add(CacheDatabase._Host + ":" + CacheDatabase._Port.ToString());
                CacheDatabase._oCacheConnection = ConnectionMultiplexer.Connect(CacheDatabase._oConectionOptions);
                CacheDatabase._oCommand = CacheDatabase._oCacheConnection.GetDatabase();
                //Check to make sure the Key Exists and if not then set it to 0
                if (!_oCommand.KeyExists(CacheDatabase._ObjectCounterKeyName))
                {
                    CacheDatabase._oCommand.StringSet(CacheDatabase._ObjectCounterKeyName, 0);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public static Int64 GetObjectIDForSpotifyID(string sSpotifyID)
        {
            Int64 iID = 0;
            if (_oCommand.KeyExists(sSpotifyID))
            {
                iID = Convert.ToInt64(CacheDatabase._oCommand.StringGet(sSpotifyID));
            }
            else
            {
                iID = CacheDatabase._oCommand.StringIncrement(CacheDatabase._ObjectCounterKeyName);
                CacheDatabase._oCommand.StringSet(sSpotifyID, iID);
            }
            return iID;
        }

       
    }
}
