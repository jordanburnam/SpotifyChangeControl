DECLARE @iID INT, @sDescription VARCHAR(1000)

SET @iID = 1
SET @sDescription = 'New Tracks Added to Playlists User Follows'
IF NOT EXISTS ( SELECT 1 FROM dbo.Spotify_User_Notification_Type SNT WHERE SNT.ID = @iID)
BEGIN 
	INSERT INTO dbo.Spotify_User_Notification_Type
	(
		ID
		,[Description]
	)
	SELECT 
		 @iID,  
		 @sDescription
END
GO
