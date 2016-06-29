IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyUser')
BEGIN
	DROP PROCEDURE AddSpotifyUser
END
GO
CREATE PROCEDURE dbo.AddSpotifyUser (
	@iUserID BIGINT
	,@sUserName NVARCHAR(1000)
	,@sAuthCode NVARCHAR(2000)
	,@dtAuth DATETIME
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM dbo.Spotify_User WHERE UserID = @iUserID)
	BEGIN
		INSERT INTO dbo.Spotify_User
		(
			UserID
			,Name
		)
		SELECT 
			@iUserID,
			@sUserName
	END

	EXEC dbo.UpSertAuthTokenForSpotifyUser @iUserID = @iUserID, @sCode = @sAuthCode, @dtAuth = @dtAuth;
END
GO