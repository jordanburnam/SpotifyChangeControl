IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Session' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Session
  (
	SessionID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,SessionGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID())
	,UserID BIGINT NOT NULL 
	,ExpiresIn INT NOT NULL DEFAULT(900)
	,Expired AS CASE WHEN DATEDIFF(SECOND, CreatedDate, GETUTCDATE()) > ExpiresIn THEN 1 ELSE 0 END
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())--This is the date the row was created may help in troubleshooting later problems
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
  CREATE NONCLUSTERED INDEX NCI_UserID ON dbo.Spotify_User_Session(CreatedDate, UserID) INCLUDE(SessionGuid);
END
GO
