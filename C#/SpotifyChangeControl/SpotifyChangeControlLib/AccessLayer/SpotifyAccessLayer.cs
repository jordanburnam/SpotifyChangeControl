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
using static SpotifyChangeControlLib.Types.Enums;

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


        public static void UpdateObjectIDForSpotifyObject(SpotifyObjectBase oSpotifyObject)
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
                if (dr["UserGuid"].ToString().ToUpper().Equals(sUserGuid.ToUpper()))
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
            dtArtists.Columns.Add(new DataColumn("PlaylistOwner", typeof(string)));

            foreach (KeyValuePair<long, SpotifyPlaylist> oKVP in Playlists)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["PlaylistID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value.Name;
                drArtist["SpotifyID"] = oKVP.Value.SpotifyID;
                drArtist["PlaylistOwner"] = oKVP.Value.Owner;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("PlaylistID", "PlaylistID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");
            SqlBulkCopyColumnMapping cmSpotifyID = new SqlBulkCopyColumnMapping("SpotifyID", "SpotifyID");
            SqlBulkCopyColumnMapping cmPlaylistOwner = new SqlBulkCopyColumnMapping("PlaylistOwner", "PlaylistOwner");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName, cmSpotifyID, cmPlaylistOwner);
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

        public static SpotifyPlaylistChange[] GetPlaylistChanges(long iUserID, DateTime dtStart, DateTime dtEnd)
        {
            List<SpotifyPlaylistChange> PlaylistChanges = new List<SpotifyPlaylistChange>();
            DataSet ds = RelationalDatabase.ExecuteStoredProcedure("GetPlaylistChangesForUser"
                                                                   , new SqlParameter("iUserID", iUserID)
                                                                   , new SqlParameter("dtStart", dtStart)
                                                                   , new SqlParameter("dtEnd", dtEnd)
                                                                );
            List<SpotifyArtist> Artists = new List<SpotifyArtist>();
            List<SpotifyTrackChange> TrackChanges = new List<SpotifyTrackChange>();
            DataRow drLastRow = ds.Tables[0].NewRow();
            bool bFirstTrack = true;

            string CurrentPlaylistGuid;
            string NextPlaylistGuid;
            long CurrentPlaylistID;
            long NextPlaylistID;
            string CurrentPlaylistName;
            string CurrentPlaylistOwner;
            string NextPlaylistName;
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                
                DataRow drCurrentRow = ds.Tables[0].Rows[j];
                if (!bFirstTrack)
                {
                    if (drLastRow["TrackGuid"].ToString().Equals(drCurrentRow["TrackGuid"].ToString()) && drLastRow["PlaylistGuid"].ToString().Equals(drCurrentRow["PlaylistGuid"].ToString()))
                        {
                            long iLastArtistID = Convert.ToInt64(drLastRow["ArtistID"].ToString());
                            string sLastArtistName = drLastRow["ArtistName"].ToString();
                            string sLastArtistGuid = drLastRow["ArtistGuid"].ToString();
                            Artists.Add(new SpotifyArtist(iLastArtistID, sLastArtistGuid, sLastArtistName));
                        }
                        else
                        {

                            long iLastArtistID = Convert.ToInt64(drLastRow["ArtistID"].ToString());
                            string sLastArtistName = drLastRow["ArtistName"].ToString();
                            string sLastArtistGuid = drLastRow["ArtistGuid"].ToString();
                            Artists.Add(new SpotifyArtist(iLastArtistID, sLastArtistGuid, sLastArtistName));
                            string sChangeType = drLastRow["ChangeCode"].ToString();
                            SpotifyChangeType eSpotifyChangeType;
                            switch (sChangeType)
                            {
                                case "A":
                                    eSpotifyChangeType = SpotifyChangeType.Add;
                                    break;
                                case "D":
                                    eSpotifyChangeType = SpotifyChangeType.Delete;
                                    break;
                                default:
                                    eSpotifyChangeType = SpotifyChangeType.Move;
                                    break;
                            }

                            TrackChanges.Add(
                                                   new SpotifyTrackChange(
                                                                            Convert.ToInt64(drLastRow["TrackID"].ToString())
                                                                            , drLastRow["TrackGuid"].ToString()
                                                                            , drLastRow["TrackName"].ToString()
                                                                            , Artists.ToArray()
                                                                            , eSpotifyChangeType
                                                                            , Convert.ToDateTime(drLastRow["ChangedDate"].ToString())
                                                                            )
                                               );

                            Artists = new List<SpotifyArtist>();
                            long iCurrentArtistID = Convert.ToInt64(drCurrentRow["ArtistID"].ToString());
                            string sCurrentArtistName = drCurrentRow["ArtistName"].ToString();
                            string sCurrentArtistGuid = drCurrentRow["ArtistGuid"].ToString();
                            Artists.Add(new SpotifyArtist(iCurrentArtistID, sCurrentArtistGuid, sCurrentArtistName));
                        }



                    if (!drLastRow["PlaylistGuid"].ToString().Equals(drCurrentRow["PlaylistGuid"].ToString()))
                    {
                        CurrentPlaylistGuid = drLastRow["PlaylistGuid"].ToString();
                        CurrentPlaylistID = Convert.ToInt64(drLastRow["PlaylistID"].ToString());
                        CurrentPlaylistName = drLastRow["PlaylistName"].ToString();
                        CurrentPlaylistOwner = drLastRow["PlaylistOwner"].ToString();
                        SpotifyPlaylistChange oSpotifyPlaylistChange = new SpotifyPlaylistChange(CurrentPlaylistID, CurrentPlaylistGuid, CurrentPlaylistName, CurrentPlaylistOwner, TrackChanges.ToArray());
                        PlaylistChanges.Add(oSpotifyPlaylistChange);
                        TrackChanges = new List<SpotifyTrackChange>();
                    }
                    
                       
                    
                    
                    
                    
                }
                else
                {
                    bFirstTrack = false;
                }
                

                drLastRow = drCurrentRow;
            }







            return PlaylistChanges.ToArray();
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
