IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Track_Changes' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Track_Changes
  (
	PlaylistID INT NOT NULL 
	,TrackID INT NOT NULL
	,ChangeType INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE CLUSTERED INDEX IX_CreatedDate ON dbo.Spotify_Playlist_Track_Changes(CreatedDate);
END
GO