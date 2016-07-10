IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetActiveSessions')
BEGIN
	DROP PROCEDURE GetActiveSessions
END
GO
CREATE PROCEDURE dbo.GetActiveSessions
AS
BEGIN
	SELECT 
		SU.UserGuid
		,SUS.SessionGuid
	FROM dbo.Spotify_User_Session SUS
	INNER JOIN dbo.Spotify_User SU ON SU.UserID = SUS.UserID
	WHERE (1=1)
		AND SUS.Expired = 0;
END