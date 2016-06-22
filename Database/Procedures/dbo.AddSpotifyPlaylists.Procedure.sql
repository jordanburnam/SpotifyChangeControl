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
			SpotifyID,
			Name
		)
		SELECT 
			SWP.SpotifyID,
			SWP.Name
		FROM dbo.Spotify_WK_Playlist SWP 
		LEFT JOIN dbo.Spotify_Playlist SP ON SP.SpotifyID = SWP.SpotifyID
		WHERE (1=1)
			AND SP.PlaylistID IS NULL --Does not already exist
	COMMIT TRANSACTION;
END
GO