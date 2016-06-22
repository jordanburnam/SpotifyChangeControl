IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetSpotifyUsersAndSpotifyTokens')
BEGIN
	DROP PROCEDURE GetSpotifyUsersAndSpotifyTokens
END
GO
CREATE PROCEDURE dbo.GetSpotifyUsersAndSpotifyTokens
AS
BEGIN

	SELECT 
		SU.UserID
		,Token.MUpdatedDate AS TokenUpdatedDate
		,Token.AccessToken
		,Token.RefreshToken
		,Token.AccessExpired
		,Token.ExpiresIn 
	FROM dbo.Spotify_User AS SU
	INNER JOIN (
				SELECT 
						SUT.UserID
					,MUpdatedDate
					,SUT.AccessToken
					,SUT.RefreshToken
					,SUT.AccessExpired
					,SUT.ExpiresIn 
				FROM dbo.Spotify_User_Token SUT
				INNER JOIN (
							SELECT 
								MAX(UpdatedDate) AS MUpdatedDate
								,UserID
							FROM dbo.Spotify_User_Token SUT
							GROUP BY UserID
							) AS MToken ON MToken.MUpdatedDate = SUT.UpdatedDate AND MToken.UserID = SUT.UserID
				) Token ON Token.UserID = SU.UserID
					
				
END
GO