IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Change' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Change
  (
  --Playlist
	PlaylistID INT NOT NULL
  --Track
	,TrackID INT NOT NULL
  --Position old and new :)
    ,oldPosition INT NOT NULL
    ,newPosition INT NOT NULL
    ,ChangeTypeID INT NOT NULL
    ,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
	,FOREIGN KEY(ChangeTypeID) REFERENCES dbo.Spotify_Playlist_Change_Type(ID)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
  CREATE UNIQUE CLUSTERED INDEX IX_UNIQUE_Changes ON dbo.Spotify_Playlist_Change(PlaylistID, TrackID, ChangeTypeID);
  CREATE NONCLUSTERED INDEX IX_NC_CreatedDate ON dbo.Spotify_Playlist_Change(CreatedDate);
END
GO