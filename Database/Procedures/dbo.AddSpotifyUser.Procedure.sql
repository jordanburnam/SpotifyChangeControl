IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyUser')
BEGIN
	DROP PROCEDURE AddSpotifyUser
END
GO
CREATE PROCEDURE dbo.AddSpotifyUser (
	@sSpotifyID NVARCHAR(1000)
	,@sAccessToken NVARCHAR(2000)
	,@sRefreshToken NVARCHAR(2000)
	,@iExpires_In INT
)
AS
BEGIN
	BEGIN TRANSACTION;
	DECLARE @iUserID INT;
	
	--Check if User Exists and if not then add them
	IF NOT EXISTS(SELECT 1 FROM dbo.Spotify_User WHERE SpotifyID = @sSpotifyID)
	BEGIN 
		INSERT INTO dbo.Spotify_User 
		(
			SpotifyID
		)
		SELECT 
			@sSpotifyID
	END

	--Get the userId of the User that we are trying to Update their access tokens for
	SELECT 
		@iUserID = UserID
	FROM dbo.Spotify_User SU WHERE SU.SpotifyID = @sSpotifyID

	

		
	COMMIT TRANSACTION;
--Return the UserID to the calling application or procedure in case it needs it		
		SELECT 
			@iUserID AS UserID	

END
GO