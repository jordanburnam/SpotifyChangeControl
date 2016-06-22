IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetSpotifyWorkTablesWithIDs')
BEGIN
	DROP PROCEDURE dbo.GetSpotifyWorkTablesWithIDs
END
GO
CREATE PROCEDURE dbo.GetSpotifyWorkTablesWithIDs
AS
BEGIN

--Return the work table with the IDs for the Playlists
	SELECT 
		SP.PlaylistID
		,SWP.SpotifyID
		,SWP.Name
	FROM dbo.Spotify_WK_Playlist SWP
	INNER JOIN dbo.Spotify_Playlist SP ON SP.SpotifyID = SWP.SpotifyID
	
--Return the work table with the IDS for the Artists
	SELECT 
		SA.ArtistID
		,SWA.SpotifyID
		,SWA.Name
	FROM dbo.Spotify_WK_Artist SWA 
	INNER JOIN dbo.Spotify_Artist SA ON SA.SpotifyID = SWA.SpotifyID

--Return the work table with the IDs for the Tracks
	SELECT 
		ST.TrackID
		,SWT.SpotifyID
		,SWT.Name
	FROM dbo.Spotify_WK_Track SWT 
	INNER JOIN dbo.Spotify_Track ST ON ST.SpotifyID = SWT.SpotifyID

				
END
GO