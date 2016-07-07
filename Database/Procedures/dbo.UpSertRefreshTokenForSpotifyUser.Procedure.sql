IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertRefreshTokenForSpotifyUser')
BEGIN
	DROP PROCEDURE UpSertRefreshTokenForSpotifyUser
END
GO
CREATE PROCEDURE dbo.UpSertRefreshTokenForSpotifyUser (
	@iUserID BIGINT
	,@sCode NVARCHAR(2000) = NULL
	,@dtToken DATETIME
)
AS
BEGIN

	BEGIN TRANSACTION;
	MERGE  dbo.Spotify_User_RefreshToken  AS targetTable
	USING ( 
			SELECT 
				@iUserID AS UserID,
				@sCode AS Code,
				@dtToken AS TokenDate
			) AS sourceTable 
	ON (sourceTable.UserID = targetTable.UserID)
	WHEN NOT MATCHED THEN 
		INSERT ( UserID, Code, TokenDate) VALUES(sourceTable.UserID, sourceTable.Code, sourceTable.TokenDate)
	WHEN MATCHED THEN 
		UPDATE 
			SET targetTable.Code = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate AND sourceTable.Code IS NOT NULL THEN sourceTable.Code ELSE targetTable.Code END
				,targetTable.TokenDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate  AND sourceTable.Code IS NOT NULL  THEN sourceTable.TokenDate ELSE targetTable.TokenDate END
				,targetTable.UpdatedDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate  AND sourceTable.Code IS NOT NULL  THEN GETDATE() ELSE targetTable.UpdatedDate END
				;
	COMMIT TRANSACTION;
END
GO
