/*
	CreatedDate: 06/19/2016
	CreatedBy: JorDANG

	Purpose:
	Get Current State of the spotify data
	and compare to the state we have saved from the last time
*/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_State' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_State
  (
--User
	UserID BIGINT NOT NULL
  --Playlist
	,PlaylistID BIGINT NOT NULL
  --Track
	,TrackID BIGINT NOT NULL
  --Position 
	,Position BIGINT NOT NULL
  --Artist
	,ArtistID BIGINT NOT NULL
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
	,FOREIGN KEY(ArtistID) REFERENCES dbo.Spotify_Artist(ArtistID)
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
  )
  CREATE CLUSTERED INDEX IX_WK_State_ID ON dbo.Spotify_WK_State(UserID, PlaylistID, ArtistID, TrackID);
END
GO
