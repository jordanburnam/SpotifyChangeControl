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
		,SU.Name
		,SUA.Code AS AuthCode
		,SUA.AuthDate 
		,SUAT.code AS AccessTokenCode
		,SUAT.TokenType AS AccessTokenTokenType
		,SUAT.AccessExpired AS AccessExpired
		,SUAT.ExpiresIn AS AccessTokenExpiresIn
		,SUAT.TokenDate AS AccessTokenCreatedDate
		,SURT.code AS RefreshTokenCode
	FROM dbo.Spotify_User AS SU
	INNER JOIN dbo.Spotify_User_Auth SUA ON SUA.UserID = SU.UserID
	LEFT JOIN dbo.Spotify_User_RefreshToken SURT ON SURT.UserID = SU.UserID
	LEFT JOIN dbo.Spotify_User_AccessToken SUAT ON SUAT.UserID = SU.UserID
	;
					
				
END
GO