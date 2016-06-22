IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'TruncateSpotifyWorkTablesAndDisableIndexies')
BEGIN
	DROP PROCEDURE TruncateSpotifyWorkTablesAndDisableIndexies
END
GO
CREATE PROCEDURE dbo.TruncateSpotifyWorkTablesAndDisableIndexies
AS
BEGIN
	
	/*
		Because we are dealing with high entropy strings
		for the SpotifyID which is essentially a 500 char
		GUID I need to disable the indexes to do the insert
		and then rebuild them. Otherwise the fragmentation would
		become unbearable after a few thousand inserts.
	*/
	ALTER INDEX ALL ON dbo.Spotify_WK_State DISABLE;
	TRUNCATE TABLE dbo.Spotify_WK_State;

	ALTER INDEX ALL ON dbo.Spotify_WK_Track DISABLE;
	TRUNCATE TABLE dbo.Spotify_WK_Track;

	ALTER INDEX ALL ON dbo.Spotify_WK_Playlist DISABLE;
	TRUNCATE TABLE dbo.Spotify_WK_Playlist;

	ALTER INDEX ALL ON dbo.Spotify_WK_Artist DISABLE;
	TRUNCATE TABLE dbo.Spotify_WK_Artist;

END
GO