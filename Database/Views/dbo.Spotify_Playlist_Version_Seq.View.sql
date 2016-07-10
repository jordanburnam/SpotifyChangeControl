IF EXISTS(SELECT 1 FROM sys.views WHERE name = 'Spotify_Playlist_Version_Seq')
BEGIN 
	DROP VIEW dbo.Spotify_Playlist_Version_Seq
END
GO
CREATE VIEW dbo.Spotify_Playlist_Version_Seq 
AS 
SELECT 
	SPV.PlaylistID
	,SPV.VersionID
	,ROW_NUMBER() OVER (PARTITION BY SPV.PlaylistID ORDER BY CreatedDate ASC) AS Seq
	,SPV.CreatedDate
FROM dbo.Spotify_Playlist_Version AS SPV

GO