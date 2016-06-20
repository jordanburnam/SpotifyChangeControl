IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PrepareWorkTables')
BEGIN
	DROP PROCEDURE PrepareWorkTables
END
GO
CREATE PROCEDURE dbo.PrepareWorkTables (
	@iUserID INT
	,@sAccessToken NVARCHAR(2000)
	,@sRefreshToken NVARCHAR(2000)
	,@iExpiresIn INT
)
AS
BEGIN
	
IF EXISTS(
		SELECT 1 
		FROM sys.indexes 
		WHERE name='IX_WK_State_SpotifyID' AND object_id = OBJECT_ID('Spotify_WK_State')
		)
BEGIN
	ALTER INDEX ALL ON dbo.Spotify_WK_State DISABLE;
END

	TRUNCATE TABLE dbo.Spotify_WK_State;

IF EXISTS(
		SELECT 1 
		FROM sys.indexes 
		WHERE name='IX_WK_Track_SpotifyID' AND object_id = OBJECT_ID('Spotify_WK_Track')
		)
BEGIN
	DROP INDEX IX_WK_Track_SpotifyID ON dbo.Spotify_WK_Track;
END

	TRUNCATE TABLE dbo.Spotify_WK_Track;
IF EXISTS(
		SELECT 1 
		FROM sys.indexes 
		WHERE name='IX_WK_Playlist_SpotifyID' AND object_id = OBJECT_ID('Spotify_WK_Playlist')
		)
BEGIN
	DROP INDEX IX_WK_Playlist_SpotifyID ON dbo.Spotify_WK_Playlist;
END
	TRUNCATE TABLE dbo.Spotify_WK_Playlist;

	TRUNCATE TABLE dbo.Spotify_WK_Artist;
END
GO