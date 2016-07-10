IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Auth' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Auth
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,AuthDate DATETIME NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE()) --Shows when the row was added
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE()) --Shows when the row was last updated
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
	
END
GO
