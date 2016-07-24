 /* 
 GeneratedBy: Admin 
 GeneratedDate: 20160724 
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
	,SpotifyID NVARCHAR(1000) NOT NULL
  )
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Artist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Artist_Track
  (
	ArtistID BIGINT NOT NULL
	,TrackID BIGINT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(ArtistID) REFERENCES dbo.Spotify_Artist(ArtistID)
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
  )
  CREATE UNIQUE CLUSTERED  INDEX UCI_Artist_Track_ArtistID_TrackID ON dbo.Spotify_Artist_Track(ArtistID, TrackID);
END
GO
/*
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
	,SpotifyID NVARCHAR(1000) NOT NULL
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
	,SpotifyID NVARCHAR(1000) NOT NULL
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
	,SpotifyID NVARCHAR(1000) NOT NULL
  )
  CREATE CLUSTERED INDEX UCI_WK_Artsit_ArtistID ON dbo.Spotify_WK_Artist(ArtistID);
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist
  (
	PlaylistID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,SpotifyID NVARCHAR(1000) NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
  )
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Track
  (
	PlaylistID BIGINT NOT NULL 
	,TrackID BIGINT NOT NULL
	,Position BIGINT NOT NULL
	,Seq INT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_PlaylistID_TrackID_Position_Seq ON dbo.Spotify_Playlist_Track(PlaylistID, TrackID, Position, Seq);
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Playlist_Version' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Playlist_Version
  (
	VersionID BIGINT NOT NULL IDENTITY(1,1)
	,VersionGuid UNIQUEIDENTIFIER DEFAULT(NEWID())
	,PlaylistID BIGINT NOT NULL 
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_VersionID_PlaylistID ON dbo.Spotify_Playlist_Version(VersionID, PlaylistID);
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Track
  (
	TrackID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,SpotifyID NVARCHAR(1000) NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
  )
 
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Name NVARCHAR(1000) NOT NULL
	,UserGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID())
	,LastLoginDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
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
	,AccessExpired AS CASE WHEN DATEDIFF(SECOND, TokenDate, GETUTCDATE()) > ExpiresIn THEN 1 ELSE 0 END
	,TokenDate DATETIME NOT NULL  --This is when the spotify api generated the token
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())--This is the date the row was created may help in troubleshooting later problems
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE()) --This is the date the row was updated may help in troubleshooting later problems
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
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE()) --Shows when the row was added
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE()) --Shows when the row was last updated
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
	
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Playlist' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Playlist
  (
	UserID BIGINT NOT NULL
	,PlaylistID BIGINT NOT NULL 
	,UserOwnsPlaylist BIT NOT NULL DEFAULT(1)
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
	,FOREIGN KEY(PlaylistID) REFERENCES dbo.Spotify_Playlist(PlaylistID)
  )
  CREATE UNIQUE CLUSTERED INDEX UCI_User_Playlist_UserID_PlaylistID ON dbo.Spotify_User_Playlist(PlaylistID, UserID);
END
GO
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_RefreshToken' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_RefreshToken
  (
	UserID BIGINT NOT NULL PRIMARY KEY
	,Code NVARCHAR(2000) NOT NULL
	,TokenDate DATETIME NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,UpdatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_User_Session' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_User_Session
  (
	SessionID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1)
	,SessionGuid UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID())
	,UserID BIGINT NOT NULL 
	,ExpiresIn INT NOT NULL DEFAULT(900)
	,Expired AS CASE WHEN DATEDIFF(SECOND, CreatedDate, GETUTCDATE()) > ExpiresIn THEN 1 ELSE 0 END
	,CreatedDate DATETIME NOT NULL DEFAULT(GETUTCDATE())--This is the date the row was created may help in troubleshooting later problems
	,FOREIGN KEY(UserID) REFERENCES dbo.Spotify_User(UserID)
  )
  CREATE NONCLUSTERED INDEX NCI_UserID ON dbo.Spotify_User_Session(CreatedDate, UserID) INCLUDE(SessionGuid);
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
  CREATE CLUSTERED INDEX IX_WK_State_ID ON dbo.Spotify_WK_State(UserID, PlaylistID, ArtistID, TrackID );
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
		,SpotifyID
	)
	SELECT 
		SWA.ArtistID,
		SWA.Name,
		SWA.SpotifyID
	FROM dbo.Spotify_WK_Artist SWA
	LEFT JOIN dbo.Spotify_Artist  SA ON SA.ArtistID = SWA.ArtistID
	WHERE (1=1)
		AND SA.ArtistID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyPlaylists')
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
			Name,
			SpotifyID
		)
		SELECT 
			SWP.PlaylistID,
			SWP.Name,
			SWP.SpotifyID
		FROM dbo.Spotify_WK_Playlist SWP 
		LEFT JOIN dbo.Spotify_Playlist SP ON SP.PlaylistID = SWP.PlaylistID
		WHERE (1=1)
			AND SP.PlaylistID IS NULL --Does not already exist
	COMMIT TRANSACTION;
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyTracks')
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
		Name,
		SpotifyID
	)
	SELECT 
		SWT.TrackID,
		SWT.Name
		,SWT.SpotifyID
	FROM dbo.Spotify_WK_Track SWT 
	LEFT JOIN dbo.Spotify_Track  ST ON ST.TrackID = SWT.TrackID
	WHERE (1=1)
		AND ST.TrackID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddSpotifyUser')
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

	IF NOT EXISTS(SELECT 1 FROM dbo.Spotify_User_Session WHERE UserID = @iUserID AND Expired = 0)
	BEGIN
		INSERT INTO dbo.Spotify_User_Session
		(
			UserID
		) 
		SELECT 
			@iUserID
	END

	UPDATE SU
			SET SU.LastLoginDate = GETUTCDATE()
		FROM dbo.Spotify_User SU
		WHERE (1=1)
			AND SU.UserID = @iUserID

	SELECT
		SU.UserGuid
		,SUS.SessionGuid
	FROM dbo.Spotify_User_Session SUS
	INNER JOIN (
				SELECT 
					MAX(SessionID) AS SessionID
					,UserID
				FROM dbo.Spotify_User_Session SUS
				WHERE (1=1)
					AND SUS.UserID = @iUserID
					AND SUS.Expired = 0
				GROUP BY UserID
			) AS LastSession ON LastSession.SessionID = SUS.SessionID AND LastSession.UserID = SUS.UserID
	INNER JOIN dbo.Spotify_User SU ON SU.UserID = SUS.UserID
END
GO
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
ENDIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetActiveSessions')
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
ENDIF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPlaylistChangesForUser')
BEGIN
	DROP PROCEDURE dbo.GetPlaylistChangesForUser
END
GO
CREATE PROCEDURE dbo.GetPlaylistChangesForUser (
	@iUserID BIGINT,
	@dtStart DATETIME 
	,@dtEnd DATETIME 
)
AS
BEGIN


SELECT 
	SPT.PlaylistID
	,SP.Name AS PlaylistName
	,SP.SpotifyID AS PlaylistGuid
	,SPT.TrackID
	,ST.Name AS TrackName
	,ST.SpotifyID AS TrackGuid
	,SA.ArtistID 
	,SA.Name AS ArtistName
	,SA.SpotifyID AS ArtistGuid
	,SPT.ChangeCode
	,SPT.ChangedDate
FROM (
			
--Get Adds
SELECT 
	SPT.PlaylistID
	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
	,'A' AS ChangeCode
	,SPT.ChangedDate
FROM (
	SELECT 
		SPT.PlaylistID
		,SPT.TrackID AS newTrackID
		,LAG(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldTrackID
		,SPT.Position AS newPosition
		,LAG(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldPosition
		,SPT.Seq AS newSeq
		,LAG(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldSeq
		,SPT.CreatedDate AS ChangedDate
	FROM dbo.Spotify_Playlist_Track SPT
	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID =  SPT.PlaylistID
	WHERE (1=1)
		AND SUP.UserId = @iUserID
		
) SPT
INNER JOIN (
			 SELECT 
				SPVS.PlaylistID 
				,MIN(SPVS.Seq) AS Seq
			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
			 WHERE (1=1)
			 GROUP BY SPVS.PlaylistID
			) AS MinSeq ON MinSeq.PlaylistID = SPT.PlaylistID AND SPT.newSeq > MinSeq.Seq --When everything is loaded the first time it is given a seq of 1 so lets not include those...it looks like the tracks were added but in reality thats just the first time we grabbed them
WHERE (1=1)
	AND ( SPT.oldTrackID IS NULL)
	AND (SPT.oldSeq IS NULL)
	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd) 
UNION ALL --Get Deletes
SELECT 
	SPT.PlaylistID
	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
	,'D' AS ChangeCode
	,SPT.ChangedDate
FROM (
	SELECT 
		SPT.PlaylistID
		,SPT.TrackID AS oldTrackID
		,LEAD(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newTrackID
		,SPT.Position AS oldPosition
		,LEAD(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newPosition
		,SPT.Seq AS oldSeq
		,LEAD(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newSeq
		,SPVS.CreatedDate AS ChangedDate
	FROM dbo.Spotify_Playlist_Track SPT
	INNER JOIN dbo.Spotify_Playlist_Version_Seq SPVS ON SPVS.PlaylistID = SPT.PlaylistID AND SPVS.Seq = SPT.Seq
	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID = SPT.PlaylistID 
	WHERE (1=1)
		AND SUP.UserID = @iUserID
		 
) SPT
INNER JOIN (
			 SELECT 
				SPVS.PlaylistID 
				,MAX(SPVS.Seq) AS Seq
			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
			 WHERE (1=1)
			 GROUP BY SPVS.PlaylistID
			) AS MaxSeq ON MaxSeq.PlaylistID = SPT.PlaylistID AND SPT.oldSeq < MaxSeq.Seq  --Make sure that just because the most recent seq does not think they were deleted because there is no other seq for them yet
WHERE (1=1)
	AND SPT.newTrackID IS NULL
	AND SPT.oldTrackID IS NOT NULL
	AND SPT.oldSeq IS NOT NULL
	AND SPT.newSeq IS NULL	
	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd)  
--UNION ALL --Get Moves
--SELECT 
--	SPT.PlaylistID
--	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
--	,'M' AS ChangeCode
--	,SPT.ChangedDate
--FROM (
--	SELECT 
--		SPT.PlaylistID
--		,SPT.TrackID AS newTrackID
--		,LAG(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate ASC) AS oldTrackID
--		,SPT.Position AS newPosition
--		,LAG(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate ASC) AS oldPosition
--		,SPT.Seq AS newSeq
--		,LAG(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate) AS oldSeq
--		,SPT.CreatedDate AS ChangedDate
--	FROM dbo.Spotify_Playlist_Track SPT
--	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID = SPT.PlaylistID
--	WHERE (1=1)
--		AND SUP.UserID = @iUserID
		
--) SPT
--INNER JOIN (
--			 SELECT 
--				SPVS.PlaylistID 
--				,MIN(SPVS.Seq) AS Seq
--			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
--			 WHERE (1=1)
--			 GROUP BY SPVS.PlaylistID
--			) AS MinSeq ON MinSeq.PlaylistID = SPT.PlaylistID AND SPT.newSeq > MinSeq.Seq --When everything is loaded the first time it is given a seq of 1 so lets not include those...it looks like the tracks were added but in reality thats just the first time we grabbed them
--WHERE (1=1)
--	AND ( SPT.oldTrackID IS NULL)
--	AND (SPT.oldSeq IS NULL)  
--	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd) 
) AS SPT 
INNER JOIN dbo.Spotify_Playlist SP ON SP.PlaylistID = SPT.PlaylistID
INNER JOIN dbo.Spotify_Track ST ON ST.TrackID = SPT.TrackID
INNER JOIN dbo.Spotify_Artist_Track SAT ON SAT.TrackID = ST.TrackID
INNER JOIN dbo.Spotify_Artist SA ON SA.ArtistID = SAT.ArtistID
ORDER BY SPT.PlaylistID, SPT.ChangeCode, SPT.TrackID, SAT.ArtistID
END
GO

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
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetSpotifyUsersAndSpotifyTokens')
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
GO
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpSertAccessTokenForSpotifyUser')
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
				,targetTable.UpdatedDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate THEN GETUTCDATE() ELSE targetTable.UpdatedDate END
				;
	COMMIT TRANSACTION;

END
GO
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
				,targetTable.UpdatedDate = CASE WHEN sourceTable.AuthDate > targetTable.AuthDate THEN GETUTCDATE() ELSE targetTable.UpdatedDate END
				;

	
	COMMIT TRANSACTION;
END
GO
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
				,targetTable.UpdatedDate = CASE WHEN sourceTable.TokenDate > targetTable.TokenDate  AND sourceTable.Code IS NOT NULL  THEN GETUTCDATE() ELSE targetTable.UpdatedDate END
				;
	COMMIT TRANSACTION;
END
GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'UpsertSpotifyState')
BEGIN
	DROP PROCEDURE dbo.UpsertSpotifyState
END
GO
CREATE PROCEDURE dbo.UpsertSpotifyState 
AS
BEGIN
	/*
		Add a verion for playlists that have changed
	*/
	BEGIN TRANSACTION;
		INSERT INTO dbo.Spotify_Playlist_Version 
		(
			PlaylistID
		)
		SELECT
			SWS.PlaylistID
		FROM dbo.Spotify_WK_State SWS
		WHERE (1=1)
		GROUP BY SWS.PlaylistID
	COMMIT TRANSACTION;
	
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
		Add the versions for playlists
	*/

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
	BEGIN TRY
	BEGIN TRANSACTION;

	/*
		Identify tracks that had their position in the 
		playlist changed because this is something we can tell the user
		later on that they can then know that we didn't miss the playlist changing 
		but the songs did get jumbled up
	*/
		
		INSERT INTO dbo.Spotify_Playlist_Track 
		(
			PlaylistID
			,TrackID
			,Position
			,Seq
		)
		SELECT 
			SWS.PlaylistID
			,SWS.TrackID
			,SWS.Position
			,SPVS.Seq
		FROM dbo.Spotify_WK_State SWS
		INNER JOIN ( 
						SELECT 
							SPVS.PlaylistID
							,MAX(SPVS.Seq) AS Seq
						FROM  dbo.Spotify_Playlist_Version_Seq AS SPVS 
						GROUP BY SPVS.PlaylistID
					) AS SPVS ON SPVS.PlaylistID = SWS.PlaylistID 
		WHERE (1=1)
		GROUP BY SWS.PlaylistID, SWS.TrackID, SWS.Position, SPVS.Seq
			
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
GO
 /* 
 END PROCEDURES 
 */ 
 
 
 
