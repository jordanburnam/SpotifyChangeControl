/*
	Threw this together quickly to get 
	a good bit of data in here to make sure this will work well enough 
	that I won't need to find a different storage solution like redis
	or SQLLite etc.
*/
DECLARE @iCurrentUsers INT = 0, @iMaxUsers INT;

SET @iMaxUsers = 10; --MAKE THIS CONFIG

/*
	Create USERS
*/

WHILE @iCurrentUsers < @iMaxUsers
BEGIN
	SET @iCurrentUsers = @iCurrentUsers + 1;
	DECLARE @iCurrentUserID INT;
	DECLARE @sCurrentUserSpotifyID VARCHAR(2000);

	DECLARE @sCurrentAccessToken VARCHAR(2000);
	DECLARE @sCurrentRefreshToken VARCHAR(2000);
	DECLARE @iCurrentExpires_In INT; 

	EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentUserSpotifyID OUTPUT
	EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentAccessToken OUTPUT
	EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentRefreshToken OUTPUT
	SET @iCurrentExpires_In = RAND() * 10000;

	EXECUTE [dbo].[AddSpotifyUser]
	@sSpotifyID = @sCurrentUserSpotifyID
	,@sAccessToken = @sCurrentAccessToken
	,@sRefreshToken = @sCurrentRefreshToken
	,@iExpires_In = @iCurrentExpires_In
	
	SELECT 
		@iCurrentUserID = SU.UserID
	FROM dbo.Spotify_User SU
	WHERE (1=1)
		AND SU.SpotifyID = @sCurrentUserSpotifyID
	
	

	DECLARE @iCurrentPlaylists INT = 0, @iMaxPlaylists INT;

	SET @iMaxPlaylists = RAND() * 100 
	
	WHILE @iCurrentPlaylists < @iMaxPlaylists
	BEGIN 
		SET @iCurrentPlaylists = @iCurrentPlaylists + 1
		DECLARE @sCurrentPlaylistSpotifyID VARCHAR(800)
		DECLARE @iCurrentPlaylistID INT;
		EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentPlaylistSpotifyID OUTPUT

		INSERT INTO dbo.Spotify_Playlist
		(
			SpotifyID
			,Name
		)
		SELECT
			@sCurrentPlaylistSpotifyID
			,'Playlist ' + CONVERT(VARCHAR(1000), @iCurrentPlaylists)  AS NAME

		SELECT 
			@iCurrentPlaylistID = SP.PlaylistID
		FROM dbo.Spotify_Playlist SP
		WHERE (1=1)
			AND SP.SpotifyID = @sCurrentPlaylistSpotifyID
		
		INSERT INTO dbo.Spotify_User_Playlist_Following
		(
			UserID
			,PlaylistID
		)
		SELECT 
			@iCurrentUserID
			,@iCurrentPlaylistID

		DECLARE @iCurrentTracks INT = 0, @iMaxTracks INT;
		SET @iMaxTracks = RAND() * 300
		DECLARE @iCurrentArtists INT = 0;
		WHILE @iCurrentTracks < @iMaxTracks
		BEGIN 
			SET @iCurrentArtists = @iCurrentArtists + 1; 
			SET @iCurrentTracks = @iCurrentTracks + 1
			DECLARE @sCurrentTrackSpotifyID VARCHAR(800)
			DECLARE @iCurrentTrackID INT
			EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentTrackSpotifyID OUTPUT
			INSERT INTO dbo.Spotify_Track
			(
				SpotifyID
				,Name
			)
			SELECT 
				@sCurrentTrackSpotifyID
				,'Track ' + CONVERT(VARCHAR(1000), @iCurrentTracks)  AS NAME

			SELECT 
				@iCurrentTrackID = ST.TrackID
			FROM dbo.Spotify_Track ST
			WHERE (1=1)
				AND ST.SpotifyID = @sCurrentTrackSpotifyID

			INSERT INTO dbo.Spotify_Playlist_Track
			(
				PlaylistID
				,TrackID
				,Position
			)
			SELECT 
				@iCurrentPlaylistID
				,@iCurrentTrackID
				,@iCurrentTracks
			
			DECLARE @sCurrentArtistSpotifyID VARCHAR(800)
			DECLARE @iCurrentArtistID INT
			EXECUTE [dbo].[usp_generateIdentifier] @string = @sCurrentArtistSpotifyID OUTPUT

			INSERT INTO dbo.Spotify_Artist
			(
				SpotifyID
				,Name
			)
			SELECT 
				@sCurrentArtistSpotifyID
				,'Artist ' + CONVERT(VARCHAR(1000), @iCurrentArtists) AS Name
			SELECT 
				@iCurrentArtistID = SA.ArtistId
			FROM dbo.Spotify_Artist SA
			WHERE (1=1)
				AND SA.SpotifyID = @sCurrentArtistSpotifyID
				
			INSERT INTO dbo.Spotify_Artist_Track
			(
				ArtistID
				,TrackID
			)
			SELECT 
				@iCurrentArtistID
				,@iCurrentTrackID
			


		END
	END

END
GO


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Artist'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Artist REBUILD;


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Playlist'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Playlist REBUILD;


	
	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Track'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Track REBUILD;

	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_User'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_User REBUILD;


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_User_Token'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_User_Token REBUILD;


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Artist_Track'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Artist_Track REBUILD;


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Playlist_Track'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Playlist_Track REBUILD;


	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_User_Playlist_Following'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_User_Playlist_Following REBUILD;

	--SELECT * FROM sys.dm_db_index_physical_stats  
 --   (DB_ID(N'SpotifyChangeControl_PROD'), OBJECT_ID(N'dbo.Spotify_Playlist_Change'), NULL, NULL , 'DETAILED'); 

	ALTER INDEX ALL ON dbo.Spotify_Playlist_Change REBUILD;

