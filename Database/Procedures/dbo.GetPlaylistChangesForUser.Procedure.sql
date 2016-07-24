IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPlaylistChangesForUser')
BEGIN
	DROP PROCEDURE dbo.GetPlaylistChangesForUser
END
GO
CREATE PROCEDURE dbo.GetPlaylistChangesForUser (
	@iUserID BIGINT,
	@dtStart DATETIME 
	,@dtEnd DATETIME 
)
AS
BEGIN


SELECT 
	SPT.PlaylistID
	,SP.Name AS PlaylistName
	,SP.SpotifyID AS PlaylistGuid
	,SPT.TrackID
	,ST.Name AS TrackName
	,ST.SpotifyID AS TrackGuid
	,SA.ArtistID 
	,SA.Name AS ArtistName
	,SA.SpotifyID AS ArtistGuid
	,SPT.ChangeCode
	,SPT.ChangedDate
FROM (
			
--Get Adds
SELECT 
	SPT.PlaylistID
	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
	,'A' AS ChangeCode
	,SPT.ChangedDate
FROM (
	SELECT 
		SPT.PlaylistID
		,SPT.TrackID AS newTrackID
		,LAG(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldTrackID
		,SPT.Position AS newPosition
		,LAG(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldPosition
		,SPT.Seq AS newSeq
		,LAG(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS oldSeq
		,SPT.CreatedDate AS ChangedDate
	FROM dbo.Spotify_Playlist_Track SPT
	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID =  SPT.PlaylistID
	WHERE (1=1)
		AND SUP.UserId = @iUserID
		
) SPT
INNER JOIN (
			 SELECT 
				SPVS.PlaylistID 
				,MIN(SPVS.Seq) AS Seq
			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
			 WHERE (1=1)
			 GROUP BY SPVS.PlaylistID
			) AS MinSeq ON MinSeq.PlaylistID = SPT.PlaylistID AND SPT.newSeq > MinSeq.Seq --When everything is loaded the first time it is given a seq of 1 so lets not include those...it looks like the tracks were added but in reality thats just the first time we grabbed them
WHERE (1=1)
	AND ( SPT.oldTrackID IS NULL)
	AND (SPT.oldSeq IS NULL)
	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd) 
UNION ALL --Get Deletes
SELECT 
	SPT.PlaylistID
	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
	,'D' AS ChangeCode
	,SPT.ChangedDate
FROM (
	SELECT 
		SPT.PlaylistID
		,SPT.TrackID AS oldTrackID
		,LEAD(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newTrackID
		,SPT.Position AS oldPosition
		,LEAD(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newPosition
		,SPT.Seq AS oldSeq
		,LEAD(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID ORDER BY SPT.CreatedDate ASC) AS newSeq
		,SPVS.CreatedDate AS ChangedDate
	FROM dbo.Spotify_Playlist_Track SPT
	INNER JOIN dbo.Spotify_Playlist_Version_Seq SPVS ON SPVS.PlaylistID = SPT.PlaylistID AND SPVS.Seq = SPT.Seq
	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID = SPT.PlaylistID 
	WHERE (1=1)
		AND SUP.UserID = @iUserID
		 
) SPT
INNER JOIN (
			 SELECT 
				SPVS.PlaylistID 
				,MAX(SPVS.Seq) AS Seq
			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
			 WHERE (1=1)
			 GROUP BY SPVS.PlaylistID
			) AS MaxSeq ON MaxSeq.PlaylistID = SPT.PlaylistID AND SPT.oldSeq < MaxSeq.Seq  --Make sure that just because the most recent seq does not think they were deleted because there is no other seq for them yet
WHERE (1=1)
	AND SPT.newTrackID IS NULL
	AND SPT.oldTrackID IS NOT NULL
	AND SPT.oldSeq IS NOT NULL
	AND SPT.newSeq IS NULL	
	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd)  
--UNION ALL --Get Moves
--SELECT 
--	SPT.PlaylistID
--	,COALESCE(SPT.newTrackID, SPT.oldTrackID) AS TrackID
--	,'M' AS ChangeCode
--	,SPT.ChangedDate
--FROM (
--	SELECT 
--		SPT.PlaylistID
--		,SPT.TrackID AS newTrackID
--		,LAG(SPT.TrackID, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate ASC) AS oldTrackID
--		,SPT.Position AS newPosition
--		,LAG(SPT.Position, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate ASC) AS oldPosition
--		,SPT.Seq AS newSeq
--		,LAG(SPT.Seq, 1) OVER (PARTITION BY SPT.PlaylistID, SPT.TrackID, SPT.Position ORDER BY SPT.CreatedDate) AS oldSeq
--		,SPT.CreatedDate AS ChangedDate
--	FROM dbo.Spotify_Playlist_Track SPT
--	INNER JOIN dbo.Spotify_User_Playlist SUP ON SUP.PlaylistID = SPT.PlaylistID
--	WHERE (1=1)
--		AND SUP.UserID = @iUserID
		
--) SPT
--INNER JOIN (
--			 SELECT 
--				SPVS.PlaylistID 
--				,MIN(SPVS.Seq) AS Seq
--			 FROM dbo.Spotify_Playlist_Version_Seq SPVS 
--			 WHERE (1=1)
--			 GROUP BY SPVS.PlaylistID
--			) AS MinSeq ON MinSeq.PlaylistID = SPT.PlaylistID AND SPT.newSeq > MinSeq.Seq --When everything is loaded the first time it is given a seq of 1 so lets not include those...it looks like the tracks were added but in reality thats just the first time we grabbed them
--WHERE (1=1)
--	AND ( SPT.oldTrackID IS NULL)
--	AND (SPT.oldSeq IS NULL)  
--	AND (SPT.ChangedDate BETWEEN @dtStart AND @dtEnd) 
) AS SPT 
INNER JOIN dbo.Spotify_Playlist SP ON SP.PlaylistID = SPT.PlaylistID
INNER JOIN dbo.Spotify_Track ST ON ST.TrackID = SPT.TrackID
INNER JOIN dbo.Spotify_Artist_Track SAT ON SAT.TrackID = ST.TrackID
INNER JOIN dbo.Spotify_Artist SA ON SA.ArtistID = SAT.ArtistID
ORDER BY SPT.PlaylistID, SPT.ChangeCode, SPT.TrackID, SAT.ArtistID
END
GO

