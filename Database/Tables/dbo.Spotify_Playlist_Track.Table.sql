IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Track
  (
	PlaylistID BIGINT NOT NULL 
	,TrackID BIGINT NOT NULL
	,Position BIGINT NOT NULL
	,Seq INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_PlaylistID_TrackID_Position_Seq ON dbo.Spotify_Playlist_Track(PlaylistID, TrackID, Position, Seq);
END
GO
