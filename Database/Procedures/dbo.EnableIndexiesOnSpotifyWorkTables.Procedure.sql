/*
	CreatedDate: 6/20/2016
	CreatedBy: JorDANG

	Purpose: 
	To enable and rebuild the indexes on the work tables after
	the inert of data has completed. This is also to enable the indexes
	on the work state table after it has been inserted into. 

	However the work state is not inserted into until after the Work 
	tables for Track, Playlist, and Artist are done.
*/
IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'EnableIndexiesOnSpotifyWorkTables')
BEGIN
	DROP PROCEDURE dbo.EnableIndexiesOnSpotifyWorkTables
END
GO
CREATE PROCEDURE dbo.EnableIndexiesOnSpotifyWorkTables (
	@sRebuildWhat CHAR(1) = 'W'
)
AS
BEGIN
	IF (@sRebuildWhat = 'S')
	BEGIN 
		ALTER INDEX ALL ON dbo.Spotify_WK_State REBUILD;
	END
	ELSE IF (@sRebuildWhat = 'W')
	BEGIN
		ALTER INDEX ALL ON dbo.Spotify_WK_Track REBUILD;

		ALTER INDEX ALL ON dbo.Spotify_WK_Playlist REBUILD;

		ALTER INDEX ALL ON dbo.Spotify_WK_Artist REBUILD;
	END
END
GO