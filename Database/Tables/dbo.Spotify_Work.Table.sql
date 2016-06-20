/*
	CreatedDate: 06/19/2016
	CreatedBy: JorDANG

	Purpose:
		These are the work tables used for BulkInserts of data
		so that a procedure can then process them

		These tables all have nonclustered indexes on the SpotifyID 
		which will be disabled upon insert and rebuilt once all rows have been added 
		to avoid fragmentation of the index 
*/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_State' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_State
  (
--User
	UserID INT NOT NULL
  --Playlist
	,PlaylistID INT NOT NULL
  --Track
	,TrackID INT NOT NULL
	--Artist
	,ArtistID INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
  CREATE CLUSTERED INDEX IX_WK_State_SpotifyID ON dbo.Spotify_WK_State(UserID, PlaylistID, ArtistID, TrackID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Track
  (
	SpotifyID NVARCHAR(850) NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE NONCLUSTERED INDEX IX_WK_Track_SpotifyID ON dbo.Spotify_WK_Track(SpotifyID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Playlist
  (
	SpotifyID NVARCHAR(850) NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE NONCLUSTERED INDEX IX_WK_Playlist_SpotifyID ON dbo.Spotify_WK_Playlist(SpotifyID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Artist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Artist
  (
	SpotifyID NVARCHAR(850) NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE NONCLUSTERED INDEX IX_NC_SpotifyID ON dbo.Spotify_WK_Artist(SpotifyID);
END
GO