using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib.DataObjects;
using SpotifyChangeControlLib.Utilities;

namespace SpotifyChangeControlLib.AccessLayer
{
    public static class SpotifyDatabase
    {
        /*
            This will be used to access the spotify database
        */
        private static string _ClientPrivate;
        private static string _ClientPublic;

        public static void Init(string sPublicClient, string sPrivateClient)
        {
            _ClientPublic = sPublicClient;
            _ClientPrivate = sPrivateClient;
        }

        public static IEnumerable<SpotifyPlaylist> GetUsersFollowedPlaylists(SpotifyUser oSpotifyUser)
        {
            List<SpotifyPlaylist> Playlists = new List<SpotifyPlaylist>();
            using (SpotifyWebAPIClient oWebCLient = GetWebClient(oSpotifyUser))
            {
                Paging<SimplePlaylist> oPaging = oWebCLient.GetUserPlaylists();

                foreach (SimplePlaylist oSimplePlaylist in oPaging.Items)
                {
                    if (!oSimplePlaylist.Owner.Id.Equals(oSpotifyUser.Name))
                    {
                        List<SpotifyTrack> oTracks = new List<SpotifyTrack>();
                        oTracks.AddRange(GetTracksForPlaylist(oWebCLient, oSimplePlaylist, oSpotifyUser));
                        Playlists.Add(new SpotifyPlaylist(oSimplePlaylist, oTracks));
                    }
                }
                while (oPaging.Next != null)
                {
                    oPaging = oWebCLient.DownloadData<Paging<SimplePlaylist>>(oPaging.Next);
                    foreach (SimplePlaylist oSimplePlaylist in oPaging.Items)
                    {
                        if (!oSimplePlaylist.Owner.Id.Equals(oSpotifyUser.Name))
                        {
                            List<SpotifyTrack> oTracks = new List<SpotifyTrack>();
                            oTracks.AddRange(GetTracksForPlaylist(oWebCLient, oSimplePlaylist, oSpotifyUser));
                            Playlists.Add(new SpotifyPlaylist(oSimplePlaylist, oTracks));
                        }
                    }
                }
            }
            return Playlists;
        }




        private static IEnumerable<SpotifyTrack> GetTracksForPlaylist(SpotifyWebAPIClient oWebCLient, SimplePlaylist oSimplePlaylist, SpotifyUser oSpotifyUser)
        {
            List<SpotifyTrack> oTracks = new List<SpotifyTrack>();
            Paging<PlaylistTrack> oPagingPlaylistTracks = oWebCLient.GetPlaylistTracks(oSimplePlaylist.Owner.Id, oSimplePlaylist.Id, market:"");
           
            foreach (PlaylistTrack oPlaylistTrack in oPagingPlaylistTracks.Items)
            {
                List<SpotifyArtist> oArtists = new List<SpotifyArtist>();
                oArtists.AddRange(GetArtistsForTrack(oPlaylistTrack));
                if (oPlaylistTrack.Track.Id == null)
                {
                    if (oPlaylistTrack.Track.Name != null && oPlaylistTrack.Track.Type != null)
                    {//Issues with some tracks having null id properties and the only way to deal with that is to try to hash an id myself
                        oPlaylistTrack.Track.Id = HashSlingingSlasher.HashString(oSimplePlaylist.Name + oSimplePlaylist.Type);
                    }

                }
                oTracks.Add(new SpotifyTrack(oPlaylistTrack, oArtists));
                
            }
            while (oPagingPlaylistTracks.Next != null)
            {
                oPagingPlaylistTracks = oWebCLient.DownloadData<Paging<PlaylistTrack>>(oPagingPlaylistTracks.Next);
                foreach (PlaylistTrack oPlaylistTrack in oPagingPlaylistTracks.Items)
                {
                    List<SpotifyArtist> oArtists = new List<SpotifyArtist>();
                    oArtists.AddRange(GetArtistsForTrack(oPlaylistTrack));
                    if (oPlaylistTrack.Track.Id == null)
                    {
                        if (oPlaylistTrack.Track.Name != null && oPlaylistTrack.Track.Type != null)
                        {//Issues with some tracks having null id properties and the only way to deal with that is to try to hash an id myself
                            oPlaylistTrack.Track.Id = HashSlingingSlasher.HashString(oSimplePlaylist.Name + oSimplePlaylist.Type);
                        }
                        
                    }
                    oTracks.Add(new SpotifyTrack(oPlaylistTrack, oArtists));

                }
            }
            
            return oTracks;
        }

        private static IEnumerable<SpotifyArtist> GetArtistsForTrack(PlaylistTrack oPlaylistTrack)
        {
            List<SpotifyArtist> oArtists = new List<SpotifyArtist>();
            foreach (SimpleArtist oSimpleArtist in oPlaylistTrack.Track.Artists)
            {
                if (oSimpleArtist.Id == null)
                {//Their web model causes this to be null sometimes and I have no idea why but my only course of action is to hash the name and use that as the spotifyID
                    oSimpleArtist.Id = HashSlingingSlasher.HashString(oSimpleArtist.Name + oSimpleArtist.Type);//Using the name and the type should make it pretty darn unique I would think....
                }
                oArtists.Add(new SpotifyArtist(oSimpleArtist));
            }

            return oArtists;
        }
       
        private static SpotifyWebAPIClient GetWebClient(SpotifyUser oSpotifyUser)
        {

            AutorizationCodeAuth oAuth = new AutorizationCodeAuth()
            {
                ClientId = _ClientPublic,
                RedirectUri= "http://localhost",
                Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ |  Scope.USER_READ_PRIVATE | Scope.USER_FOLLOW_READ
            };

            oAuth.StartHttpServer();//Not sure if this is needed or not but what the hell why not!
           
            if (oSpotifyUser.AccessToken.AccessExpired)//The user has authorized us and was tokenized but the temp access token has expired
            {
                Token oToken = oAuth.RefreshToken(oSpotifyUser.RefreshToken.Code, _ClientPrivate);
                if (oToken.Error == null)
                {
                    oSpotifyUser.UpdateUserWithToken(oToken);
                }
            }
            




            oAuth.StopHttpServer();
            SpotifyWebAPIClient oWebClient = new SpotifyWebAPIClient()
            {
                AccessToken = oSpotifyUser.AccessToken.Code,
                TokenType = oSpotifyUser.AccessToken.TokenType,
                UseAuth = true
            };

            return oWebClient;
        }

    }
}
