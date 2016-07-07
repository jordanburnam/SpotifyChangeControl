IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Playlist
  (
	UserID BIGINT NOT NULL
	,PlaylistID BIGINT NOT NULL 
	,UserOwnsPlaylist BIT NOT NULL DEFAULT(1)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_User_Playlist_UserID_PlaylistID ON dbo.Spotify_User_Playlist(PlaylistID, UserID);
END
GO
