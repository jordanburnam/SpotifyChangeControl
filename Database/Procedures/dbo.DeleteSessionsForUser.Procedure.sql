IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteSessionsForUser')
BEGIN
	DROP PROCEDURE DeleteSessionsForUser
END
GO
CREATE PROCEDURE dbo.DeleteSessionsForUser(
	@iUserID BIGINT
)
AS
BEGIN
SELECT 'HOMO'
	--DELETE SUS
	--FROM dbo.Spotify_User_Session SUS
	--WHERE (1=1)
	--	AND SUS.UserID = @iUserID;
END