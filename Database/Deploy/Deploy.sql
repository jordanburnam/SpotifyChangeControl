 /* 
 GeneratedBy: Admin 
 GeneratedDate: 20160704 
 */ 
 
 
 
 /* 
 BEGIN TABLES 
 */ 
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Artist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Artist
  (
	ArtistID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
  )
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Artist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Artist_Track
  (
	ArtistID BIGINT NOT NULL
	,TrackID BIGINT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(ArtistID) REFERENCES dbo.Spotify_Artist(ArtistID)
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
  )
  CREATE UNIQUE CLUSTERED  INDEX UCI_Artist_Track_ArtistID_TrackID ON dbo.Spotify_Artist_Track(ArtistID, TrackID);
END
GO/*
	CreatedDate: 06/19/2016
	CreatedBy: JorDANG

	Purpose:
	Tables for Bulk Insert from
	app so that we can quickly insert any
	new objects BIGINTo its respective table
*/

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Track
  (
	TrackID BIGINT NOT NULL 
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX UCI_Track_WK_TrackID ON dbo.Spotify_WK_Track(TrackID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Playlist
  (
	PlaylistID BIGINT NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX UCI_WK_Playlist_PlaylistID ON dbo.Spotify_WK_Playlist(PlaylistID);
END
GO


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_Artist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_Artist
  (
	ArtistID BIGINT NOT NULL
	,Name NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX UCI_WK_Artsit_ArtistID ON dbo.Spotify_WK_Artist(ArtistID);
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist
  (
	PlaylistID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Change_Type' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Change_Type
  (
	ID INT NOT NULL
	,Code CHAR(1) NOT NULL
	,[Description] VARCHAR(1000) NOT NULL
) 
	CREATE UNIQUE CLUSTERED  INDEX IX_UNIQUE_ID ON dbo.Spotify_Playlist_Change_Type(ID);
END 
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Track
  (
	PlaylistID BIGINT NOT NULL 
	,TrackID BIGINT NOT NULL
	,Position BIGINT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_Playlist_Track_PlaylistID_TrackID_Position ON dbo.Spotify_Playlist_Track(PlaylistID, TrackID, Position);
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Change' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Change
  (
  --Playlist
	PlaylistID BIGINT NOT NULL
  --Track
	,TrackID BIGINT NOT NULL
  --Position old and new :)
    ,Position INT NOT NULL
    ,ChangeTypeID INT NOT NULL
    ,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
	,FOREIGN KEY(ChangeTypeID) REFERENCES dbo.Spotify_Playlist_Change_Type(ID)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
  CREATE  UNIQUE CLUSTERED INDEX CI_PLaylist_Change_Playlist_Track_ChangeType ON dbo.Spotify_Playlist_Change(CreatedDate, PlaylistID, TrackID, Position, ChangeTypeID);
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Track
  (
	TrackID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
 
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,UserGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID())
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
  )
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_AccessToken' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_AccessToken
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,TokenType NVARCHAR(100) NOT NULL
	,ExpiresIn INT NOT NULL 
	,AccessExpired AS CASE WHEN DATEDIFF(SECOND, TokenDate, GETDATE()) > ExpiresIn THEN 1 ELSE 0 END
	,TokenDate DATETIME NOT NULL  --This is when the spotify api generated the token
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())--This is the date the row was created may help in troubleshooting later problems
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()) --This is the date the row was updated may help in troubleshooting later problems
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Auth' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Auth
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,AuthDate DATETIME NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()) --Shows when the row was added
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE()) --Shows when the row was last updated
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
	
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Notification' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Notification
  (
	NotificationID BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY
	,UserID BIGINT NOT NULL
	,NotificationTypeID INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(NotificationTypeID) REFERENCES dbo.Spotify_User_Notification_Type(ID)
  )
  CREATE NONCLUSTERED INDEX NCI_CreatedDate_UserID ON dbo.Spotify_User_Notification(CreatedDate, UserID)
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Notification_Type' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Notification_Type
  (
	ID INT NOT NULL
	,[Description] VARCHAR(1000) NOT NULL
) 
	CREATE UNIQUE CLUSTERED  INDEX IX_UNIQUE_ID ON dbo.Spotify_User_Notification_Type(ID);
END IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Playlist
  (
	UserID BIGINT NOT NULL
	,PlaylistID BIGINT NOT NULL 
	,UserOwnsPlaylist BIT NOT NULL DEFAULT(1)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_User_Playlist_UserID_PlaylistID ON dbo.Spotify_User_Playlist(PlaylistID, UserID);
END
GOIF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_RefreshToken' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_RefreshToken
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,TokenDate DATETIME NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
END
GO

/*
	CreatedDate: 06/19/2016
	CreatedBy: JorDANG

	Purpose:
	Get Current State of the spotify data
	and compare to the state we have saved from the last time
*/
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_WK_State' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_WK_State
  (
--User
	UserID BIGINT NOT NULL
  --Playlist
	,PlaylistID BIGINT NOT NULL
  --Track
	,TrackID BIGINT NOT NULL
  --Position 
	,Position BIGINT NOT NULL
  --Artist
	,ArtistID BIGINT NOT NULL
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
  CREATE CLUSTERED INDEX IX_WK_State_ID ON dbo.Spotify_WK_State(UserID, PlaylistID, ArtistID, TrackID);
END
GO
 /* 
 END TABLES 
 */ 
 
 
 
 /* 
 BEGIN PROCEDURES 
 */ 
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyArtists')
BEGIN
	DROP PROCEDURE dbo.AddSpotifyArtists
END
GO
CREATE PROCEDURE dbo.AddSpotifyArtists
AS
BEGIN
	BEGIN TRANSACTION;
	INSERT INTO dbo.Spotify_Artist 
	(
		ArtistID
		,Name
	)
	SELECT 
		SWA.ArtistID,
		SWA.Name
	FROM dbo.Spotify_WK_Artist SWA
	LEFT JOIN dbo.Spotify_Artist  SA ON SA.ArtistID = SWA.ArtistID
	WHERE (1=1)
		AND SA.ArtistID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyPlaylists')
BEGIN
	DROP PROCEDURE dbo.AddSpotifyPlaylists
END
GO
CREATE PROCEDURE dbo.AddSpotifyPlaylists
AS
BEGIN
	BEGIN TRANSACTION;
		INSERT INTO dbo.Spotify_Playlist
		(
			PlaylistID,
			Name
		)
		SELECT 
			SWP.PlaylistID,
			SWP.Name
		FROM dbo.Spotify_WK_Playlist SWP 
		LEFT JOIN dbo.Spotify_Playlist SP ON SP.PlaylistID = SWP.PlaylistID
		WHERE (1=1)
			AND SP.PlaylistID IS NULL --Does not already exist
	COMMIT TRANSACTION;
END
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyTracks')
BEGIN
	DROP PROCEDURE dbo.AddSpotifyTracks
END
GO
CREATE PROCEDURE dbo.AddSpotifyTracks
AS
BEGIN
	BEGIN TRANSACTION;
	INSERT INTO dbo.Spotify_Track 
	(
		TrackID,
		Name
	)
	SELECT 
		SWT.TrackID,
		SWT.Name
	FROM dbo.Spotify_WK_Track SWT 
	LEFT JOIN dbo.Spotify_Track  ST ON ST.TrackID = SWT.TrackID
	WHERE (1=1)
		AND ST.TrackID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyUser')
BEGIN
	DROP PROCEDURE AddSpotifyUser
END
GO
CREATE PROCEDURE dbo.AddSpotifyUser (
	@iUserID BIGINT
	,@sUserName NVARCHAR(1000)
	,@sAuthCode NVARCHAR(2000)
	,@dtAuth DATETIME
)
AS
BEGIN
	IF NOT EXISTS (SELECT 1 FROM dbo.Spotify_User WHERE UserID = @iUserID)
	BEGIN
		INSERT INTO dbo.Spotify_User
		(
			UserID
			,Name
		)
		SELECT 
			@iUserID,
			@sUserName
	END

	EXEC dbo.UpSertAuthTokenForSpotifyUser @iUserID = @iUserID, @sCode = @sAuthCode, @dtAuth = @dtAuth;
END
GOIF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetSpotifyUsersAndSpotifyTokens')
BEGIN
	DROP PROCEDURE GetSpotifyUsersAndSpotifyTokens
END
GO
CREATE PROCEDURE dbo.GetSpotifyUsersAndSpotifyTokens
AS
BEGIN

	SELECT 
		SU.UserID
		,SU.Name
		,SU.UserGuid
		,SUA.Code AS AuthCode
		,SUA.AuthDate 
		,SUAT.code AS AccessTokenCode
		,SUAT.TokenType AS AccessTokenTokenType
		,SUAT.AccessExpired AS AccessExpired
		,SUAT.ExpiresIn AS AccessTokenExpiresIn
		,SUAT.TokenDate AS AccessTokenCreatedDate
		,SURT.code AS RefreshTokenCode
	FROM dbo.Spotify_User AS SU
	INNER JOIN dbo.Spotify_User_Auth SUA ON SUA.UserID = SU.UserID
	LEFT JOIN dbo.Spotify_User_RefreshToken SURT ON SURT.UserID = SU.UserID
	LEFT JOIN dbo.Spotify_User_AccessToken SUAT ON SUAT.UserID = SU.UserID
	;
					
				
END
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertAccessTokenForSpotifyUser')
BEGIN
	DROP PROCEDURE UpSertAccessTokenForSpotifyUser
END
GO
CREATE PROCEDURE dbo.UpSertAccessTokenForSpotifyUser (
	@iUserID BIGINT
	,@sCode NVARCHAR(2000)
	,@sTokenType NVARCHAR(100)
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
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertAuthTokenForSpotifyUser')
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
GOIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertRefreshTokenForSpotifyUser')
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
GOIF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'UpsertSpotifyState')
BEGIN
	DROP PROCEDURE dbo.UpsertSpotifyState
END
GO
CREATE PROCEDURE dbo.UpsertSpotifyState 
AS
BEGIN
	/*
		Insert the new tracks 
		for artist that do not already exist
	*/
	BEGIN TRANSACTION;
		INSERT INTO dbo.Spotify_Artist_Track
		(
			ArtistID
			,TrackID
		)
		SELECT 
			SWS.ArtistID
			,SWS.TrackID
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_Artist_Track SAT ON SAT.ArtistID = SWS.ArtistID AND SAT.TrackID = SWS.TrackID
		WHERE (1=1)
			AND SAT.ArtistID IS NULL
		GROUP BY SWS.ArtistID, SWS.TrackID --Grouping to get our records to the Artist/Track level
	COMMIT TRANSACTION;

	/*
		UpSert the playlists being followed
		by a user. 
	*/
	BEGIN TRANSACTION; 
		INSERT INTO dbo.Spotify_User_Playlist
		(
			UserID
			,PlaylistID
		)
		SELECT 
			SWS.UserID,
			SWS.PlaylistID 
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_User_Playlist SUP ON SUP.UserID = SWS.UserID AND SUP.PlaylistID = SWS.PlaylistID
		WHERE (1=1)
			AND SUP.UserID IS NULL --Make sure it does not already exist
		GROUP BY SWS.UserID, SWS.PlaylistID

		DELETE SUP
		FROM dbo.Spotify_User_Playlist SUP
		LEFT JOIN dbo.Spotify_WK_State SWS ON SWS.PlaylistID = SUP.PlaylistID AND SWS.UserID = SUP.UserID
		WHERE (1=1)
			AND SWS.PlaylistID IS NULL --The user stopped following a playlist

	COMMIT TRANSACTION;
	/*
		Now for the biggie
		Update all changes and track changes so we can analyze it later and
		decide what to update the user with
	*/
	/*
		I am going to use an 
		in memory table for now 
		just to give us some performance for now
		...shouldn't be too BIG of a deal but 
		we will see....	
	*/
	DECLARE @SpotifyPlaylistUpdates TABLE
	(
		oldPlaylistID INT  NULL
		,newPlaylistID INT NULL
		,oldTrackID INT NULL
		,newTrackID INT NULL
		,oldPosition INT NULL
		,newPosition INT NULL
	);
	BEGIN TRY
	BEGIN TRANSACTION;

	/*
		Identify tracks that had their position in the 
		playlist changed because this is something we can tell the user
		later on that they can then know that we didn't miss the playlist changing 
		but the songs did get jumbled up
	*/
		UPDATE SPT 
			SET SPT.Position = SWS.Position
		OUTPUT deleted.PlaylistID, inserted.PlaylistID, deleted.TrackID, inserted.TrackID, deleted.Position, inserted.Position INTO @SpotifyPlaylistUpdates(oldPlaylistID, newPlaylistID, oldTrackID, newTrackID, oldPosition, newPosition)
		FROM dbo.Spotify_Playlist_Track SPT
		INNER JOIN dbo.Spotify_WK_State SWS ON SWS.PlaylistID = SPT.PlaylistID AND SWS.TrackID = SPT.TrackID
		LEFT JOIN dbo.Spotify_WK_State UNIQUE_SWS ON UNIQUE_SWS.PlaylistID = SPT.PlaylistID AND UNIQUE_SWS.TrackID = SPT.TrackID AND UNIQUE_SWS.Position = SPT.Position
		WHERE (1=1)
			AND SWS.Position <> SPT.Position --Make sure the postion is different
			AND UNIQUE_SWS.UserID IS NULL --If a track is duplicated in a playlist we make sure that there is not example of it already existing in the same place it is in

		INSERT INTO dbo.Spotify_Playlist_Track
		(
			PlaylistID,
			TrackID,
			Position
		)
		OUTPUT inserted.PlaylistID, inserted.TrackID, inserted.Position INTO @SpotifyPlaylistUpdates(newPlaylistID, newTrackID, newPosition)
		SELECT 
			SWS.PlaylistID
			,SWS.TrackID
			,SWS.Position
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_Playlist_Track SPT ON SPT.PlaylistID = SWS.PlaylistID AND SPT.TrackID = SWS.TrackID AND SPT.Position = SWS.Position
		WHERE (1=1)
			AND SPT.PlaylistID IS NULL -- Make sure the track does not already exist
		GROUP BY SWS.PlaylistID, SWS.TrackID,SWS.Position
		
		DELETE SPT 
		OUTPUT deleted.PlaylistID, deleted.TrackID, deleted.Position INTO @SpotifyPlaylistUpdates(oldPlaylistID, oldTrackID, oldPosition)
		FROM dbo.Spotify_Playlist_Track SPT
		LEFT JOIN dbo.Spotify_WK_State SWS ON SPT.PlaylistID = SWS.PlaylistID AND SPT.TrackID = SWS.TrackID AND SPT.Position = SWS.Position
		WHERE (1=1)
			AND SWS.PlaylistID IS NULL --The track does not exist in the state anymore....it was removed from the playlist

		INSERT INTO dbo.Spotify_Playlist_Change  
		(
			PlaylistID
			,TrackID
			,Position
			,ChangeTypeID
		)
		SELECT 
			COALESCE(SPU.newPlaylistID,SPU.oldPlaylistID)
			,COALESCE(SPU.newTrackID, SPU.oldTrackID)
			,COALESCE(SPU.newPosition, SPU.oldPosition)
			,CASE 
				WHEN SPU.oldTrackID IS NULL AND SPU.newTrackID IS NOT NULL THEN 1 --The track was added to the playlist
				WHEN SPU.newTrackID IS NULL AND SPU.oldTrackID IS NOT NULL THEN 3 --The track was deleted
				WHEN SPU.oldPosition IS NOT NULL AND SPU.newPosition IS NOT NULL AND SPU.oldPosition <> SPU.newPosition AND SPU.oldTrackID IS NOT NULL AND SPU.newTrackID IS NOT NULL THEN 2 --The track was moved in the playlist
			END AS ChangeTypeID
		FROM @SpotifyPlaylistUpdates SPU
		WHERE (1=1)
			

		SELECT * FROM @SpotifyPlaylistUpdates;
	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 
		BEGIN 
			ROLLBACK TRANSACTION;
		END
		SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage
	END CATCH
	



END
GO /* 
 END PROCEDURES 
 */ 
 
 
 
 /* 
 BEGIN DATA 
 */ 
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
 /* 
 END DATA 
 */ 
