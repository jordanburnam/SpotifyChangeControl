IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES  T
           WHERE T.TABLE_NAME = N'Spotify_Artist_Track' AND T.TABLE_SCHEMA = 'dbo' )
BEGIN
  CREATE TABLE dbo.Spotify_Artist_Track
  (
	ArtistID BIGINT NOT NULL
	,TrackID BIGINT NOT NULL
	,CreatedDate DATETIME NOT NULL DEFAULT(GETDATE())
	,FOREIGN KEY(ArtistID) REFERENCES dbo.Spotify_Artist(ArtistID)
	,FOREIGN KEY(TrackID) REFERENCES dbo.Spotify_Track(TrackID)
  )
  CREATE UNIQUE CLUSTERED  INDEX UCI_Artist_Track_ArtistID_TrackID ON dbo.Spotify_Artist_Track(ArtistID, TrackID);
END
GO