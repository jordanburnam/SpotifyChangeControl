IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertAuthTokenForSpotifyUser')
BEGIN
	DROP PROCEDURE UpSertAuthTokenForSpotifyUser
END
GO
CREATE PROCEDURE dbo.UpSertAuthTokenForSpotifyUser (
	@iUserID BIGINT
	,@sCode NVARCHAR(2000)
	,@dtAuth DATETIME
)
AS
BEGIN

	BEGIN TRANSACTION;
	MERGE  dbo.Spotify_User_Auth  AS targetTable
	USING ( 
			SELECT 
				@iUserID AS UserID,
				@sCode AS Code,
				@dtAuth AS AuthDate
			) AS sourceTable 
	ON (sourceTable.UserID = targetTable.UserID)
	WHEN NOT MATCHED THEN 
		INSERT ( UserID, Code, AuthDate) VALUES(sourceTable.UserID, sourceTable.Code, sourceTable.AuthDate)
	WHEN MATCHED THEN 
		UPDATE 
			SET targetTable.Code = CASE WHEN sourceTable.AuthDate > targetTable.AuthDate THEN sourceTable.Code ELSE targetTable.Code END
				,targetTable.AuthDate = CASE WHEN sourceTable.AuthDate > targetTable.AuthDate THEN sourceTable.AuthDate ELSE targetTable.AuthDate END
				,targetTable.UpdatedDate = CASE WHEN sourceTable.AuthDate > targetTable.AuthDate THEN GETDATE() ELSE targetTable.UpdatedDate END
				;
	COMMIT TRANSACTION;
END
GO
