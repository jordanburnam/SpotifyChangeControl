using SpotifyChangeControlLib.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SpotifyChangeControlLib.DataObjects;
using SpotifyChangeControlLib.StorageLayer;
using SpotifyChangeControlLib.Types.Abstract;
using System.Data;
using System.Data.SqlClient;

namespace SpotifyChangeControlLib.AccessLayer
{
    internal static class SpotifyAccessLayer 
    {
        public static Int64 GetObjectIDForSpotifyID(string sSpotifyID)
        {
            Int64 iObjectID;
            iObjectID = CacheDatabase.GetObjectIDForSpotifyID(sSpotifyID);
            return iObjectID;

        }


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
        public static SpotifyUser GetSpotifyUserForGuid(string sUserGuid)
        {
            SpotifyUser oSpotifyUser = null;
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetSpotifyUsersAndSpotifyTokens");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if(dr["UserGuid"].ToString().ToUpper().Equals(sUserGuid.ToUpper()))
                {
                    oSpotifyUser = new SpotifyUser(dr);
                }

            }
            return oSpotifyUser;
        }
        public static string GetUserGuidForUserID(long iUserID)
        {
            string sUserGuid = "";
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetSpotifyUserGuidForUserID", new SqlParameter("iUserID", iUserID));
            if (ds.Tables[0].Rows.Count == 1)
            {
                sUserGuid = ds.Tables[0].Rows[0]["UserGuid"].ToString();
            }
            return sUserGuid;
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


        public static void SaveUserAndAuthToDatabase(SpotifyUser oSpotifyUser)
        {
            
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("AddSpotifyUser",
                                                    new SqlParameter("iUserID", oSpotifyUser.ID),
                                                    new SqlParameter("sUserName", oSpotifyUser.Name),
                                                    new SqlParameter("sAuthCode", oSpotifyUser.UserAuth.Code),
                                                    new SqlParameter("dtAuth", oSpotifyUser.UserAuth.AuthDate)
                                               );
            oSpotifyUser.SessionGuid = ds.Tables[0].Rows[0]["SessionGuid"].ToString();
            
            
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

        public static void SaveArtistsToDatabase(Dictionary<Int64, SpotifyArtist> Artists, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("ArtistID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            dtArtists.Columns.Add(new DataColumn("SpotifyID", typeof(string)));
            foreach (KeyValuePair<long, SpotifyArtist> oKVP in Artists)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["ArtistID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value.Name;
                drArtist["SpotifyID"] = oKVP.Value.SpotifyID;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("ArtistID", "ArtistID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");
            SqlBulkCopyColumnMapping cmSpotifyID = new SqlBulkCopyColumnMapping("SpotifyID", "SpotifyID");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName, cmSpotifyID);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyArtists", CommandType.StoredProcedure);
        }

     
     

        public static void SavePlaylistsToDatabase(Dictionary<Int64, SpotifyPlaylist> Playlists, WorkTableState oWorkTableState)
        {
            
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("PlaylistID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            dtArtists.Columns.Add(new DataColumn("SpotifyID", typeof(string)));

            foreach (KeyValuePair<long, SpotifyPlaylist> oKVP in Playlists)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["PlaylistID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value.Name;
                drArtist["SpotifyID"] = oKVP.Value.SpotifyID;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("PlaylistID", "PlaylistID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");
            SqlBulkCopyColumnMapping cmSpotifyID = new SqlBulkCopyColumnMapping("SpotifyID", "SpotifyID");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName, cmSpotifyID);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyPlaylists", CommandType.StoredProcedure);
       
        }

        

        public static void SaveTracksToDatabase(Dictionary<Int64, SpotifyTrack> Tracks, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("TrackID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            dtArtists.Columns.Add(new DataColumn("SpotifyID", typeof(string)));
            foreach (KeyValuePair<long, SpotifyTrack> oKVP in Tracks)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["TrackID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value.Name;
                drArtist["SpotifyID"] = oKVP.Value.SpotifyID;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("TrackID", "TrackID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");
            SqlBulkCopyColumnMapping cmSpotifyID = new SqlBulkCopyColumnMapping("SpotifyID", "SpotifyID");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName, cmSpotifyID);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyTracks", CommandType.StoredProcedure);
        }
        public static void SaveStatesToDatabase(List<SpotifyState> SpotifyStates, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("UserID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("PlaylistID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("TrackID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Position", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("ArtistID", typeof(long)));

            foreach (SpotifyState oSpotifyState in SpotifyStates)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["UserID"] = oSpotifyState.UserID;
                drArtist["PlaylistID"] = oSpotifyState.PlaylistID;
                drArtist["TrackID"] = oSpotifyState.TrackID;
                drArtist["Position"] = oSpotifyState.Position;
                drArtist["ArtistID"] = oSpotifyState.ArtistID;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmUserID = new SqlBulkCopyColumnMapping("UserID", "UserID");
            SqlBulkCopyColumnMapping cmPlaylistID = new SqlBulkCopyColumnMapping("PlaylistID", "PlaylistID");
            SqlBulkCopyColumnMapping cmTrackID = new SqlBulkCopyColumnMapping("TrackID", "TrackID");
            SqlBulkCopyColumnMapping cmPosition = new SqlBulkCopyColumnMapping("Position", "Position");
            SqlBulkCopyColumnMapping cmArtistID = new SqlBulkCopyColumnMapping("ArtistID", "ArtistID");


            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmUserID, cmPlaylistID, cmTrackID, cmPosition, cmArtistID);
            RelationalDatabase.ExecuteNonQuery("UpsertSpotifyState", CommandType.StoredProcedure);
        }

        public static SpotifyPlaylistChange[] GetPlaylistChanges(string sUserGuid = null)
        {
            List<SpotifyPlaylistChange> Changes = new List<SpotifyPlaylistChange>();
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetPlaylistChanges");

            List<string> Artists = new List<string>();
            DataRow drLastRow = ds.Tables[0].NewRow();
            bool bFirstTime = true;
            for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow drCurrentRow = ds.Tables[0].Rows[i];
                if (!bFirstTime)
                {
                    if (drLastRow["TrackName"].ToString().Equals(drCurrentRow["TrackName"].ToString()))
                    {
                        Artists.Add(drLastRow["ArtistName"].ToString());
                    }
                    else
                    {
                        Artists.Add(drLastRow["ArtistName"].ToString());
                        Changes.Add(
                                       new SpotifyPlaylistChange(
                                                                Convert.ToInt64(drLastRow["UserID"].ToString())
                                                                , drLastRow["UserGuid"].ToString()
                                                                , drLastRow["UserName"].ToString()
                                                                ,Convert.ToInt64(drLastRow["PlaylistID"].ToString())
                                                                ,drLastRow["PlaylistSpotifyID"].ToString()
                                                                , drLastRow["PlaylistName"].ToString()
                                                                ,Convert.ToInt64(drLastRow["TrackID"].ToString())
                                                                ,drLastRow["TrackSpotifyID"].ToString()
                                                                , drLastRow["TrackName"].ToString()
                                                                , drLastRow["ChangeCode"].ToString().ToCharArray()[0]
                                                                , Artists.ToArray()
                                                                ,Convert.ToDateTime(drLastRow["ChangedDate"].ToString())
                                                                )
                                   );

                        Artists = new List<string>();
                        Artists.Add(drCurrentRow["ArtistName"].ToString());
                    }
                }
                else
                {
                    bFirstTime = false;
                }
                

                drLastRow = drCurrentRow;
            }

            SpotifyPlaylistChange[] UserChanges = ( 
                                                    from Change in Changes
                                                    where (sUserGuid == null ? true: Change.UserGuid == sUserGuid)
                                                    select Change
                                                 ).ToArray();

            return UserChanges;
        }

        public static bool AuthenticateUserWithSession(string sUserGuid, string sSessionGuid)
        {
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetActiveSessions");
            bool bisAuth = (ds.Tables[0].Select(string.Format("UserGuid = '{0}' AND SessionGUid = '{1}'", sUserGuid, sSessionGuid)).Count() > 0 ? true : false);
            return bisAuth;
        }

        public static void DeleteSessionForUser(Int64 iUserID)
        {
            RelationalDatabase.ExecuteNonQuery("DeleteSessionsForUser", CommandType.StoredProcedure, new SqlParameter("iUserID", iUserID));
        }

    }
}
