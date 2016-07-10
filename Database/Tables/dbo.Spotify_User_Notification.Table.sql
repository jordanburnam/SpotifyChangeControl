IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Notification' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Notification
  (
	NotificationID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY
	,UserID BIGINT NOT NULL
	,NotificationTypeID INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(NotificationTypeID) REFERENCES dbo.Spotify_User_Notification_Type(ID)
  )
  CREATE NONCLUSTERED INDEX NCI_CreatedDate_UserID ON dbo.Spotify_User_Notification(CreatedDate, UserID)
END
GO
