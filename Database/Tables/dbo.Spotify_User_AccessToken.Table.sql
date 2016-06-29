IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_AccessToken' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_AccessToken
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,TokenType NVARCHAR(100) NOT NULL
	,ExpiresIn INT NOT NULL 
	,AccessExpired AS CASE WHEN DATEDIFF(SECOND, TokenDate, GETDATE()) > ExpiresIn THEN 1 ELSE 0 END
	,TokenDate DATETIME NOT NULL  --This is when the spotify api generated the token
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())--This is the date the row was created may help in troubleshooting later problems
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()) --This is the date the row was updated may help in troubleshooting later problems
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
END
GO

