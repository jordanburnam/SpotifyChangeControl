IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPlaylistChanges')
BEGIN
	DROP PROCEDURE dbo.GetPlaylistChanges
END
GO
CREATE PROCEDURE dbo.GetPlaylistChanges(
	@iUserID BIGINT = NULL
)
AS
BEGIN
	SELECT 
		SU.UserID
		,SU.UserGuid
		,SU.Name AS UserName
		,SPCT.Code AS ChangeCode
		,SP.Name AS PlaylistName
		,ST.Name AS TrackName
		,SA.Name AS ArtistName
		,SPC.CreatedDate AS ChangedDate
	FROM dbo.Spotify_Playlist_Change SPC
	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID = SPC.PlaylistID
	INNER JOIN dbo.Spotify_User SU ON SU.UserID = SUP.UserID
	INNER JOIN dbo.Spotify_Playlist SP ON SP.PlaylistID = SPC.PlaylistID
	INNER JOIN dbo.Spotify_Track ST ON ST.TrackID = SPC.TrackID
	INNER JOIN dbo.Spotify_Artist_Track SAT ON SAT.TrackID = ST.TrackID
	INNER JOIN dbo.Spotify_Artist SA ON SA.ArtistID = SAT.ArtistID
	INNER JOIN dbo.Spotify_Playlist_Change_Type SPCT ON SPCT.ID = SPC.ChangeTypeID
	LEFT JOIN ( 
					SELECT 
						SUN.UserID
						,MAX(SUN.CreatedDate) AS MNotifiedDate
					FROM dbo.Spotify_User_Notification SUN 
					WHERE (1=1)
					GROUP BY UserID
				) AS LastUserNotify ON LastUserNotify.UserID = SU.UserID
	WHERE (1=1)
		AND (SPC.CreatedDate > LastUserNotify.MNotifiedDate OR LastUserNotify.MNotifiedDate IS NULL)
		AND (SU.UserID = @iUserID OR @iUserID IS NULL)
	ORDER BY UserID, ChangeCode, PlaylistName, TrackName, ArtistName
END
GO

