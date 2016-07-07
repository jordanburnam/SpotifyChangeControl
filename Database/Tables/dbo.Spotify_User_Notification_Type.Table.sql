IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Notification_Type' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Notification_Type
  (
	ID INT NOT NULL
	,[Description] VARCHAR(1000) NOT NULL
) 
	CREATE UNIQUE CLUSTERED  INDEX IX_UNIQUE_ID ON dbo.Spotify_User_Notification_Type(ID);
END 
GO
