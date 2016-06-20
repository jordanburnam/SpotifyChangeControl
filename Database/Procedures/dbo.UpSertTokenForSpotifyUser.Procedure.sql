IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertTokenForSpotifyUser')
BEGIN
	DROP PROCEDURE UpSertTokenForSpotifyUser
END
GO
CREATE PROCEDURE dbo.UpSertTokenForSpotifyUser (
	@iUserID INT
	,@sAccessToken NVARCHAR(2000)
	,@sRefreshToken NVARCHAR(2000)
	,@iExpiresIn INT
)
AS
BEGIN
	MERGE  dbo.Spotify_User_Token  AS targetTable
	USING ( 
			SELECT 
				@iUserID AS UserID,
				@sAccessToken AS AccessToken,
				@sRefreshToken AS RefreshToken,
				@iExpiresIn AS ExpiresIn
			) AS sourceTable 
	ON (sourceTable.UserID = targetTable.UserID)
	WHEN NOT MATCHED THEN 
		INSERT ( UserID, AccessToken, RefreshToken, ExpiresIn) VALUES(sourceTable.UserID, sourceTable.AccessToken, sourceTable.RefreshToken, sourceTable.ExpiresIn)
	WHEN MATCHED THEN 
		UPDATE 
			SET targetTable.AccessToken = sourceTable.AccessToken
				,targetTable.RefreshToken = sourceTable.RefreshToken
				,targetTable.ExpiresIn = sourceTable.ExpiresIn
				,targetTable.UpdatedDate = GETDATE();
END
GO