using SpotifyChangeControlLib.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyChangeControlLib.DataObjects;
using SpotifyChangeControlLib.StorageLayer;
using SpotifyChangeControlLib.Types.Abstract;
using System.Data;
using System.Data.SqlClient;

namespace SpotifyChangeControlLib.AccessLayer
{
    public static class SpotifyAccessLayer 
    {
        public  static void UpdateObjectIDForSpotifyObject(SpotifyObjectBase oSpotifyObject)
        {
            if (oSpotifyObject.ID == 0)
            {
                oSpotifyObject.ID = CacheDatabase.GetObjectIDForSpotifyID(oSpotifyObject.SpotifyID);
            }

        }

        public static void UpdateObjectIDForSpotifyObjects(params SpotifyObjectBase[] oSpotifyObjects)
        {
            foreach (SpotifyObjectBase oSpotifyObject in oSpotifyObjects)
            {
                SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(oSpotifyObject);
            }

        }

        public static IEnumerable<SpotifyUser> GetSpotifyUsersAndTokens()
        {
            List<SpotifyUser> oUsers = new List<SpotifyUser>();
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetSpotifyUsersAndSpotifyTokens");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SpotifyUser oUser = new SpotifyUser(dr);
                oUsers.Add(oUser);
                oUser = null;
            }
            return oUsers;
        }

        public static void SaveAccessTokenForUserToDatabase(Int64 iUserID, SpotifyUserAccessToken oSpotifyAccessToken)
        {
            
            RelationalDatabase.ExecuteNonQuery("UpSertAccessTokenForSpotifyUser", CommandType.StoredProcedure,
                                                    new SqlParameter("iUserID", iUserID),
                                                    new SqlParameter("sCode", oSpotifyAccessToken.Code),
                                                    new SqlParameter("sTokenType", oSpotifyAccessToken.TokenType),
                                                     new SqlParameter("iExpiresIn", oSpotifyAccessToken.ExpiresIn),
                                                      new SqlParameter("dtToken", oSpotifyAccessToken.TokenDate)
                                               );
            
        }

        public static void SaveRefreshTokenForUserToDatabase(Int64 iUserID, SpotifyUserRefreshToken oSpotifyUserRefreshToken)
        {

            RelationalDatabase.ExecuteNonQuery("UpSertRefreshTokenForSpotifyUser", CommandType.StoredProcedure,
                                                    new SqlParameter("iUserID", iUserID),
                                                    new SqlParameter("sCode", oSpotifyUserRefreshToken.Code),
                                                      new SqlParameter("dtToken", oSpotifyUserRefreshToken.TokenDate)
                                               );

        }

        public static void SaveAuthTokenForUserToDatabase(Int64 iUserID, SpotifyUserAuth oSpotifyUserAuth)
        {

            RelationalDatabase.ExecuteNonQuery("UpSertAuthTokenForSpotifyUser", CommandType.StoredProcedure,
                                                    new SqlParameter("iUserID", iUserID),
                                                    new SqlParameter("sCode", oSpotifyUserAuth.Code),
                                                      new SqlParameter("dtAuth", oSpotifyUserAuth.AuthDate)
                                               );

        }

        

    }
}
