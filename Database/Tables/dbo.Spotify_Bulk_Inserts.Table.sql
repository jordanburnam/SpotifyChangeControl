/*
	CreatedDate: 06/19/2016
	CreatedBy: JorDANG

	Purpose:
	Tables for Bulk Insert from
	app so that we can quickly insert any
	new objects BIGINTo its respective table
*/

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Track
  (
	TrackID BIGINT NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX CI_Track_WK_TrackID ON dbo.Spotify_WK_Track(TrackID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Playlist
  (
	PlaylistID BIGINT NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX CI_WK_Playlist_PlaylistID ON dbo.Spotify_WK_Playlist(PlaylistID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Artist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Artist
  (
	ArtistID BIGINT NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE NONCLUSTERED INDEX CI_WK_Artsit_ArtistID ON dbo.Spotify_WK_Artist(ArtistID);
END
GO