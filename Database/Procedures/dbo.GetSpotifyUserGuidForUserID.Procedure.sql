IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetSpotifyUserGuidForUserID')
BEGIN
	DROP PROCEDURE GetSpotifyUserGuidForUserID
END
GO
CREATE PROCEDURE dbo.GetSpotifyUserGuidForUserID
(
	@iUserID BIGINT
)
AS
BEGIN
	SELECT 
		UserGuid
	FROM dbo.Spotify_User SU
	WHERE (1=1)
		AND SU.UserID = @iUserID;
END
GO
