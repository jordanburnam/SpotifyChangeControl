IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Version' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Version
  (
	VersionID BIGINT NOT NULL IDENTITY(1,1)
	,VersionGuid UNIQUEIDENTIFIER DEFAULT(NEWID())
	,PlaylistID BIGINT NOT NULL 
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_VersionID_PlaylistID ON dbo.Spotify_Playlist_Version(VersionID, PlaylistID);
END
GO
