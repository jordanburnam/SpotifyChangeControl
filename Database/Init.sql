CREATE TABLE Spotify_Playlist
(
PlaylistID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Name TEXT NOT NULL
,SpotifyID TEXT NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
);

CREATE TABLE Spotify_Artist
(
ArtistID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Name TEXT NOT NULL
,SpotifyID TEXT NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
);

CREATE TABLE Spotify_Track
(
TrackID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Name TEXT NOT NULL
,SpotifyID TEXT NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
);

CREATE TABLE Spotify_User
(
UserID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Name TEXT NOT NULL 
,UserGuid TEXT NOT NULL
,LastLoginDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
);

CREATE TABLE Spotify_User_AccessToken
(
UserID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Code TEXT NOT NULL
,TokenType TEXT NOT NULL
,ExpiresIn INTEGER NOT NULL 
,TokenDate TEXT NOT NULL  --This is when the spotify api generated the token
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))--This is the date the row was created may help in troubleshooting later problems
,UpdatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now')) --This is the date the row was updated may help in troubleshooting later problems
,FOREIGN KEY(UserID) REFERENCES Spotify_User(UserID)
);

CREATE TABLE Spotify_User_Auth
(
UserID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Code TEXT NOT NULL
,AuthDate TEXT NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now')) --Shows when the row was added
,UpdatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now')) --Shows when the row was last updated
,FOREIGN KEY(UserID) REFERENCES Spotify_User(UserID)
);

CREATE TABLE Spotify_User_Playlist
(
UserID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,PlaylistID INTEGER NOT NULL 
,UserOwnsPlaylist INTEGER NOT NULL DEFAULT(1) CHECK(UserOwnsPlaylist BETWEEN 0 AND 1)
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,FOREIGN KEY(UserID) REFERENCES Spotify_User(UserID)
,FOREIGN KEY(PlaylistID) REFERENCES Spotify_Playlist(PlaylistID)
);

CREATE TABLE Spotify_User_RefreshToken
(
UserID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,Code TEXT NOT NULL
,TokenDate TEXT NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,UpdatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,FOREIGN KEY(UserID) REFERENCES Spotify_User(UserID)
);

CREATE TABLE Spotify_User_Session
(
SessionID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,SessionGuid TEXT NOT NULL
,UserID INTEGER NOT NULL 
,ExpiresIn INTEGER NOT NULL DEFAULT(900)
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))--This is the date the row was created may help in troubleshooting later problems
,FOREIGN KEY(UserID) REFERENCES Spotify_User(UserID)
);

CREATE TABLE Spotify_Artist_Track
(
ArtistID INTEGER NOT NULL
,TrackID INTEGER NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,FOREIGN KEY(ArtistID) REFERENCES Spotify_Artist(ArtistID)
,FOREIGN KEY(TrackID) REFERENCES Spotify_Track(TrackID)
);

CREATE TABLE Spotify_Playlist_Track
(
PlaylistID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,TrackID INTEGER NOT NULL
,Position INTEGER NOT NULL
,Seq INTEGER NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,FOREIGN KEY(TrackID) REFERENCES Spotify_Track(TrackID)
,FOREIGN KEY(PlaylistID) REFERENCES Spotify_Playlist(PlaylistID)
);

CREATE TABLE Spotify_Playlist_Version
(
VersionID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE
,VersionGuid UNIQUEIDENTIFIER DEFAULT(NEWID())
,PlaylistID INTEGER NOT NULL 
,SEQ INTEGER NOT NULL
,CreatedDate TEXT NOT NULL DEFAULT(STRFTIME('%Y-%m-%dT%H:%M:%S', 'now'))
,FOREIGN KEY(PlaylistID) REFERENCES Spotify_Playlist(PlaylistID)
);


CREATE VIEW VW_Spotify_User_AccessToken
AS 
SELECT 
UserID,
Code,
TokenType,
ExpiresIn,
CASE WHEN DATEDIFF(SECOND, TokenDate, STRFTIME('%Y-%m-%dT%H:%M:%S', 'now')) > ExpiresIn THEN 1 ELSE 0 END AS AccessExpired,
TokenDate,
CreatedDate,
UpdatedDate
FROM Spotify_User_AccessToken