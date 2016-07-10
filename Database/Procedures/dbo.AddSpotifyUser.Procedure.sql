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

	IF NOT EXISTS(SELECT 1 FROM dbo.Spotify_User_Session WHERE UserID = @iUserID AND Expired = 0)
	BEGIN
		INSERT INTO dbo.Spotify_User_Session
		(
			UserID
		) 
		SELECT 
			@iUserID
	END

	UPDATE SU
			SET SU.LastLoginDate = GETUTCDATE()
		FROM dbo.Spotify_User SU
		WHERE (1=1)
			AND SU.UserID = @iUserID

	SELECT
		SU.UserGuid
		,SUS.SessionGuid
	FROM dbo.Spotify_User_Session SUS
	INNER JOIN (
				SELECT 
					MAX(SessionID) AS SessionID
					,UserID
				FROM dbo.Spotify_User_Session SUS
				WHERE (1=1)
					AND SUS.UserID = @iUserID
					AND SUS.Expired = 0
				GROUP BY UserID
			) AS LastSession ON LastSession.SessionID = SUS.SessionID AND LastSession.UserID = SUS.UserID
	INNER JOIN dbo.Spotify_User SU ON SU.UserID = SUS.UserID
END
GO
