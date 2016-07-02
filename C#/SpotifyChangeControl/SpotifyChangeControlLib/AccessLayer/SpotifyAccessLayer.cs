﻿using SpotifyChangeControlLib.Types.Interfaces;
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
            
            RelationalDatabase.ExecuteNonQuery("AddSpotifyUser", CommandType.StoredProcedure,
                                                    new SqlParameter("iUserID", oSpotifyUser.ID),
                                                    new SqlParameter("sUserName", oSpotifyUser.Name),
                                                    new SqlParameter("sAuthCode", oSpotifyUser.UserAuth.Code),
                                                    new SqlParameter("dtAuth", oSpotifyUser.UserAuth.AuthDate)
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

        public static void SaveArtistsToDatabase(Dictionary<Int64, string> Artists, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("ArtistID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            foreach (KeyValuePair<long, string> oKVP in Artists)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["ArtistID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("ArtistID", "ArtistID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyArtists", CommandType.StoredProcedure);
        }

        public static void SaveTracksToDatabase(Dictionary<Int64, string> Tracks, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("TrackID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            foreach (KeyValuePair<long, string> oKVP in Tracks)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["TrackID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("TrackID", "TrackID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyTracks", CommandType.StoredProcedure);
        }
        public static void SavePlaylistsToDatabase(Dictionary<Int64, string> Playlists, WorkTableState oWorkTableState)
        {
            DataTable dtArtists = new DataTable();
            dtArtists.Columns.Add(new DataColumn("PlaylistID", typeof(long)));
            dtArtists.Columns.Add(new DataColumn("Name", typeof(string)));
            foreach (KeyValuePair<long, string> oKVP in Playlists)
            {
                DataRow drArtist = dtArtists.NewRow();
                drArtist["PlaylistID"] = oKVP.Key;
                drArtist["Name"] = oKVP.Value;
                dtArtists.Rows.Add(drArtist);
            }
            SqlBulkCopyColumnMapping cmID = new SqlBulkCopyColumnMapping("PlaylistID", "PlaylistID");
            SqlBulkCopyColumnMapping cmName = new SqlBulkCopyColumnMapping("Name", "Name");

            RelationalDatabase.BulkInsert(dtArtists, oWorkTableState, cmID, cmName);
            RelationalDatabase.ExecuteNonQuery("AddSpotifyPlaylists", CommandType.StoredProcedure);
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



    }
}
