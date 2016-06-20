IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Playlist_Following' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Playlist_Following
  (
	UserID INT NOT NULL
	,PlaylistID INT NOT NULL 
	,Deleted BIT NOT NULL DEFAULT(0)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX IX_UQ_NC_UserID_PlaylistID ON dbo.Spotify_User_Playlist_Following(UserID, PlaylistID);
END
GO