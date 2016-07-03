IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'UpsertSpotifyState')
BEGIN
	DROP PROCEDURE dbo.UpsertSpotifyState
END
GO
CREATE PROCEDURE dbo.UpsertSpotifyState 
AS
BEGIN
	/*
		Insert the new tracks 
		for artist that do not already exist
	*/
	BEGIN TRANSACTION;
		INSERT INTO dbo.Spotify_Artist_Track
		(
			ArtistID
			,TrackID
		)
		SELECT 
			SWS.ArtistID
			,SWS.TrackID
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_Artist_Track SAT ON SAT.ArtistID = SWS.ArtistID AND SAT.TrackID = SWS.TrackID
		WHERE (1=1)
			AND SAT.ArtistID IS NULL
		GROUP BY SWS.ArtistID, SWS.TrackID --Grouping to get our records to the Artist/Track level
	COMMIT TRANSACTION;

	/*
		UpSert the playlists being followed
		by a user. 
	*/
	BEGIN TRANSACTION; 
		INSERT INTO dbo.Spotify_User_Playlist
		(
			UserID
			,PlaylistID
		)
		SELECT 
			SWS.UserID,
			SWS.PlaylistID 
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_User_Playlist SUP ON SUP.UserID = SWS.UserID AND SUP.PlaylistID = SWS.PlaylistID
		WHERE (1=1)
			AND SUP.UserID IS NULL --Make sure it does not already exist
		GROUP BY SWS.UserID, SWS.PlaylistID

		DELETE SUP
		FROM dbo.Spotify_User_Playlist SUP
		LEFT JOIN dbo.Spotify_WK_State SWS ON SWS.PlaylistID = SUP.PlaylistID AND SWS.UserID = SUP.UserID
		WHERE (1=1)
			AND SWS.PlaylistID IS NULL --The user stopped following a playlist

	COMMIT TRANSACTION;
	/*
		Now for the biggie
		Update all changes and track changes so we can analyze it later and
		decide what to update the user with
	*/
	/*
		I am going to use an 
		in memory table for now 
		just to give us some performance for now
		...shouldn't be too BIG of a deal but 
		we will see....	
	*/
	DECLARE @SpotifyPlaylistUpdates TABLE
	(
		oldPlaylistID INT  NULL
		,newPlaylistID INT NULL
		,oldTrackID INT NULL
		,newTrackID INT NULL
		,oldPosition INT NULL
		,newPosition INT NULL
	);
	BEGIN TRY
	BEGIN TRANSACTION;

	/*
		Identify tracks that had their position in the 
		playlist changed because this is something we can tell the user
		later on that they can then know that we didn't miss the playlist changing 
		but the songs did get jumbled up
	*/
		UPDATE SPT 
			SET SPT.Position = SWS.Position
		OUTPUT deleted.PlaylistID, inserted.PlaylistID, deleted.TrackID, inserted.TrackID, deleted.Position, inserted.Position INTO @SpotifyPlaylistUpdates(oldPlaylistID, newPlaylistID, oldTrackID, newTrackID, oldPosition, newPosition)
		FROM dbo.Spotify_Playlist_Track SPT
		INNER JOIN dbo.Spotify_WK_State SWS ON SWS.PlaylistID = SPT.PlaylistID AND SWS.TrackID = SPT.TrackID
		LEFT JOIN dbo.Spotify_WK_State UNIQUE_SWS ON UNIQUE_SWS.PlaylistID = SPT.PlaylistID AND UNIQUE_SWS.TrackID = SPT.TrackID AND UNIQUE_SWS.Position = SPT.Position
		WHERE (1=1)
			AND SWS.Position <> SPT.Position --Make sure the postion is different
			AND UNIQUE_SWS.UserID IS NULL --If a track is duplicated in a playlist we make sure that there is not example of it already existing in the same place it is in

		INSERT INTO dbo.Spotify_Playlist_Track
		(
			PlaylistID,
			TrackID,
			Position
		)
		OUTPUT inserted.PlaylistID, inserted.TrackID, inserted.Position INTO @SpotifyPlaylistUpdates(newPlaylistID, newTrackID, newPosition)
		SELECT 
			SWS.PlaylistID
			,SWS.TrackID
			,SWS.Position
		FROM dbo.Spotify_WK_State SWS
		LEFT JOIN dbo.Spotify_Playlist_Track SPT ON SPT.PlaylistID = SWS.PlaylistID AND SPT.TrackID = SWS.TrackID AND SPT.Position = SWS.Position
		WHERE (1=1)
			AND SPT.PlaylistID IS NULL -- Make sure the track does not already exist
		GROUP BY SWS.PlaylistID, SWS.TrackID,SWS.Position
		
		DELETE SPT 
		OUTPUT deleted.PlaylistID, deleted.TrackID, deleted.Position INTO @SpotifyPlaylistUpdates(oldPlaylistID, oldTrackID, oldPosition)
		FROM dbo.Spotify_Playlist_Track SPT
		LEFT JOIN dbo.Spotify_WK_State SWS ON SPT.PlaylistID = SWS.PlaylistID AND SPT.TrackID = SWS.TrackID AND SPT.Position = SWS.Position
		WHERE (1=1)
			AND SWS.PlaylistID IS NULL --The track does not exist in the state anymore....it was removed from the playlist

		INSERT INTO dbo.Spotify_Playlist_Change  
		(
			PlaylistID
			,TrackID
			,Position
			,ChangeTypeID
		)
		SELECT 
			COALESCE(SPU.newPlaylistID,SPU.oldPlaylistID)
			,COALESCE(SPU.newTrackID, SPU.oldTrackID)
			,COALESCE(SPU.newPosition, SPU.oldPosition)
			,CASE 
				WHEN SPU.oldTrackID IS NULL AND SPU.newTrackID IS NOT NULL THEN 1 --The track was added to the playlist
				WHEN SPU.newTrackID IS NULL AND SPU.oldTrackID IS NOT NULL THEN 3 --The track was deleted
				WHEN SPU.oldPosition IS NOT NULL AND SPU.newPosition IS NOT NULL AND SPU.oldPosition <> SPU.newPosition AND SPU.oldTrackID IS NOT NULL AND SPU.newTrackID IS NOT NULL THEN 2 --The track was moved in the playlist
			END AS ChangeTypeID
		FROM @SpotifyPlaylistUpdates SPU
		WHERE (1=1)
			

		SELECT * FROM @SpotifyPlaylistUpdates;
	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 
		BEGIN 
			ROLLBACK TRANSACTION;
		END
		SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_MESSAGE() AS ErrorMessage
	END CATCH
	



END
GO