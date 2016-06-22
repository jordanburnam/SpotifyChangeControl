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
		SpotifyID,
		Name
	)
	SELECT 
		SWT.SpotifyID,
		SWT.Name
	FROM dbo.Spotify_WK_Track SWT 
	LEFT JOIN dbo.Spotify_Track  ST ON ST.SpotifyID = SWT.SpotifyID
	WHERE (1=1)
		AND ST.TrackID IS NULL --The track does not already exist

		
	COMMIT TRANSACTION;

END
GO