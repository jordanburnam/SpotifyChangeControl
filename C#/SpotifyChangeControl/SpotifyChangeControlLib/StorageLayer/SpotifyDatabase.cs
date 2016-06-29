using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyChangeControlLib.DataObjects;

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

                foreach (SimplePlaylist playlist in oPaging.Items)
                {
                    if (!playlist.Owner.Id.Equals(oSpotifyUser.Name))
                    {
                        Playlists.Add(new SpotifyPlaylist(playlist));
                    }
                }
                while (oPaging.Next != null)
                {
                    oPaging = oWebCLient.DownloadData<Paging<SimplePlaylist>>(oPaging.Next);
                    foreach (SimplePlaylist playlist in oPaging.Items)
                    {
                        if (!playlist.Owner.Id.Equals(oSpotifyUser.Name))
                        {
                            Playlists.Add(new SpotifyPlaylist(playlist));
                        }
                    }
                }
            }
            return Playlists;
        }

        private static SpotifyWebAPIClient GetWebClient(SpotifyUser oSpotifyUser)
        {

            AutorizationCodeAuth oAuth = new AutorizationCodeAuth()
            {
                ClientId = _ClientPublic,
                RedirectUri= "http://localhost",
                Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_LIBRARY_MODIFY | Scope.USER_READ_PRIVATE
                    | Scope.USER_FOLLOW_MODIFY | Scope.USER_FOLLOW_READ | Scope.PLAYLIST_MODIFY_PRIVATE | Scope.USER_READ_BIRTHDATE | Scope.PLAYLIST_MODIFY_PUBLIC
            };

            oAuth.StartHttpServer();//Not sure if this is needed or not but what the hell why not!
            if (!oSpotifyUser.UserAuth.HasBeenTokenized)//The user has authorized the app but no token has been created yet
            {
               Token oToken = oAuth.ExchangeAuthCode(oSpotifyUser.UserAuth.Code, _ClientPrivate);
                if (oToken.Error == null) //There were no errors found 
                {
                    oSpotifyUser.UpdateUserWithToken(oToken);
                }
                else
                {
                    //TODO:Add some error handling....just kinda everywhere
                }
            }
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
