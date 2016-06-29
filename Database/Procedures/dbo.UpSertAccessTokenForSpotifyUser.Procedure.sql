IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertAccessTokenForSpotifyUser')
BEGIN
	DROP PROCEDURE UpSertAccessTokenForSpotifyUser
END
GO
CREATE PROCEDURE dbo.UpSertAccessTokenForSpotifyUser (
	@iUserID BIGINT
	,@sCode NVARCHAR(2000)
	,@sTokenType NVARCHAR(2000)
	,@iExpiresIn INT
	,@dtToken DATETIME
)
AS
BEGIN

	BEGIN TRANSACTION;
	MERGE  dbo.Spotify_User_AccessToken  AS targetTable
	USING ( 
			SELECT 
				@iUserID AS UserID,
				@sCode AS Code,
				@sTokenType AS TokenType,
				@iExpiresIn AS ExpiresIn,
				@dtToken AS TokenDate
			) AS sourceTable 
	ON (sourceTable.UserID = targetTable.UserID)
	WHEN NOT MATCHED THEN 
		INSERT ( UserID, Code, TokenType, ExpiresIn, TokenDate) VALUES(sourceTable.UserID, sourceTable.Code, sourceTable.TokenType, sourceTable.ExpiresIn, sourceTable.TokenDate)
	WHEN MATCHED THEN 
		UPDATE 
			SET targetTable.Code = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN sourceTable.Code ELSE targetTable.Code END
				,targetTable.TokenType = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN  sourceTable.TokenType ELSE targetTable.TokenType END
				,targetTable.ExpiresIn = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN  sourceTable.ExpiresIn ELSE targetTable.ExpiresIn END
				,targetTable.TokenDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN sourceTable.TokenDate ELSE targetTable.TokenDate END
				,targetTable.UpdatedDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN GETDATE() ELSE targetTable.UpdatedDate END
				;
	COMMIT TRANSACTION;

END
GO