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
		SpotifyID,
		Name
	)
	SELECT 
		SWA.SpotifyID,
		SWA.Name
	FROM dbo.Spotify_WK_Artist SWA
	LEFT JOIN dbo.Spotify_Artist  SA ON SA.SpotifyID = SWA.SpotifyID
	WHERE (1=1)
		AND SA.ArtistID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GO