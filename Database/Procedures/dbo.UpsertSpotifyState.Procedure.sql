IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'UpsertSpotifyState')
BEGIN
	DROP PROCEDURE dbo.UpsertSpotifyState
END
GO
CREATE PROCEDURE dbo.UpsertSpotifyState 
AS
BEGIN
	/*
		Add a verion for playlists that have changed
	*/
	BEGIN TRANSACTION;
		INSERT INTO dbo.Spotify_Playlist_Version 
		(
			PlaylistID
		)
		SELECT
			SWS.PlaylistID
		FROM dbo.Spotify_WK_State SWS
		WHERE (1=1)
		GROUP BY SWS.PlaylistID
	COMMIT TRANSACTION;
	
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
		Add the versions for playlists
	*/

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
	BEGIN TRY
	BEGIN TRANSACTION;

	/*
		Identify tracks that had their position in the 
		playlist changed because this is something we can tell the user
		later on that they can then know that we didn't miss the playlist changing 
		but the songs did get jumbled up
	*/
		
		INSERT INTO dbo.Spotify_Playlist_Track 
		(
			PlaylistID
			,TrackID
			,Position
			,Seq
		)
		SELECT 
			SWS.PlaylistID
			,SWS.TrackID
			,SWS.Position
			,SPVS.Seq
		FROM dbo.Spotify_WK_State SWS
		INNER JOIN ( 
						SELECT 
							SPVS.PlaylistID
							,MAX(SPVS.Seq) AS Seq
						FROM  dbo.Spotify_Playlist_Version_Seq AS SPVS 
						GROUP BY SPVS.PlaylistID
					) AS SPVS ON SPVS.PlaylistID = SWS.PlaylistID 
		WHERE (1=1)
		GROUP BY SWS.PlaylistID, SWS.TrackID, SWS.Position, SPVS.Seq
			
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
