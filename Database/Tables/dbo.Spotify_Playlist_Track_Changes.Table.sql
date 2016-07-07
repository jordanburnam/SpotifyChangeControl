IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Change' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Change
  (
  --Playlist
	PlaylistID BIGINT NOT NULL
  --Track
	,TrackID BIGINT NOT NULL
  --Position old and new :)
    ,Position INT NOT NULL
    ,ChangeTypeID INT NOT NULL
    ,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
	,FOREIGN KEY(ChangeTypeID) REFERENCES dbo.Spotify_Playlist_Change_Type(ID)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
  )
  CREATE  UNIQUE CLUSTERED INDEX CI_PLaylist_Change_Playlist_Track_ChangeType ON dbo.Spotify_Playlist_Change(CreatedDate, PlaylistID, TrackID, Position, ChangeTypeID);
END
GO
