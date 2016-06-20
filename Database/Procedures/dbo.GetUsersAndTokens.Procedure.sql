IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'GetUsersAndTokens')
BEGIN
	DROP PROCEDURE GetUsersAndTokens
END
GO
CREATE PROCEDURE dbo.GetUsersAndTokens
AS
BEGIN

	SELECT 
		SU.UserID
		,Token.MTokenID
		,Token.AccessToken
		,Token.RefreshToken
		,Token.AccessExpired
		,Token.ExpiresIn 
	FROM dbo.Spotify_User AS SU
	INNER JOIN (
				SELECT 
						SUT.UserID
					,MTokenID
					,SUT.AccessToken
					,SUT.RefreshToken
					,SUT.AccessExpired
					,SUT.ExpiresIn 
				FROM dbo.Spotify_User_Token SUT
				INNER JOIN (
							SELECT 
								MAX(TokenID) AS MTokenID
								,UserID
							FROM dbo.Spotify_User_Token SUT
							GROUP BY UserID
							) AS MToken ON MToken.MTokenID = SUT.TokenID AND MToken.UserID = SUT.UserID
				) Token ON Token.UserID = SU.UserID
					
				
END
GO