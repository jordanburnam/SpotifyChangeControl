DECLARE @iID INT, @sCode CHAR(1), @sDescription VARCHAR(1000)



SET @iID = 1
SET @sCode = 'A'
SET @sDescription = 'Track was added'
IF NOT EXISTS ( SELECT 1 FROM dbo.Spotify_Playlist_Change_Type SPCT WHERE SPCT.ID = @iID)
BEGIN 
	INSERT INTO dbo.Spotify_Playlist_Change_Type
	(
		ID
		,Code
		,[Description]
	)
	SELECT 
		 @iID, 
		 @sCode, 
		 @sDescription
END

SET @iID = 2
SET @sCode = 'M'
SET @sDescription = 'Track position was changed'
IF NOT EXISTS ( SELECT 1 FROM dbo.Spotify_Playlist_Change_Type SPCT WHERE SPCT.ID = @iID)
BEGIN 
	INSERT INTO dbo.Spotify_Playlist_Change_Type
	(
		ID
		,Code
		,[Description]
	)
	SELECT 
		 @iID, 
		 @sCode, 
		 @sDescription
END

SET @iID = 3
SET @sCode = 'D'
SET @sDescription = 'Track was deleted'
IF NOT EXISTS ( SELECT 1 FROM dbo.Spotify_Playlist_Change_Type SPCT WHERE SPCT.ID = @iID)
BEGIN 
	INSERT INTO dbo.Spotify_Playlist_Change_Type
	(
		ID
		,Code
		,[Description]
	)
	SELECT 
		 @iID, 
		 @sCode, 
		 @sDescription
END
GO
