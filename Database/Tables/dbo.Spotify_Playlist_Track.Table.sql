IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Track
  (
	PlaylistID INT NOT NULL 
	,TrackID INT NOT NULL
	,Position INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX IX_UQ_NC_PlaylistID_TrackID ON dbo.Spotify_Playlist_Track(PlaylistID, TrackID, Position);
END
GO