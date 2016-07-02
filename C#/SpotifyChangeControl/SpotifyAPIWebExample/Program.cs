using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;
using SpotifyAPI.Utilities.Extensions;
using System.Data;
using System.Data.SqlClient;
using SpotifyChangeControlLib;
using SpotifyChangeControlLib.DataObjects;
using SpotifyChangeControlLib.DataManagers;

namespace SpotifyWebAPIExample
{
    class Program
    {
        static ImplicitGrantAuth auth;
        static SpotifyWebAPIClient _oSpotifyWebApi;
        static PrivateProfile _oPrivateProfile;
        static AutorizationCodeAuth oAutorizationCodeAuth;
        static string sSpotifyPrivateID;
        static string sSpotifyPublicID;
        private static string SCC_PRIVATE_ID;
        private static string SCC_PUBLIC_ID;
        private static string SCC_SQL_CON;
        private static string SCC_REDIS_HOST;
        private static string SCC_REDIS_PASS;
        private static int SCC_REDIS_PORT;
       

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                SCC_PRIVATE_ID = System.Environment.GetEnvironmentVariable("SCC_PRIVATE_ID");
                SCC_PUBLIC_ID = System.Environment.GetEnvironmentVariable("SCC_PUBLIC_ID");

                SCC_SQL_CON = System.Environment.GetEnvironmentVariable("SCC_SQL_CON");

                SCC_REDIS_HOST = System.Environment.GetEnvironmentVariable("SCC_REDIS_HOST");
                SCC_REDIS_PASS = System.Environment.GetEnvironmentVariable("SCC_REDIS_PASS");
                string sPort = System.Environment.GetEnvironmentVariable("SCC_REDIS_PORT");
                if (!int.TryParse(sPort, out SCC_REDIS_PORT))
                {
                    SCC_REDIS_PORT = 6379;
                }
            }
            SCCManager oSCCManager = new SCCManager(SCC_PRIVATE_ID, SCC_PUBLIC_ID, "http://localhost", SCC_SQL_CON, SCC_REDIS_HOST, SCC_REDIS_PORT, SCC_REDIS_PASS);

           
            Console.WriteLine("### SpotifyWebAPI .NET Test App");
            Console.WriteLine("Starting auth process...");
            //Create the auth object
            
            oAutorizationCodeAuth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = SCC_PUBLIC_ID,
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = "http://localhost",
                //How many permissions we need?
                Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_LIBRARY_MODIFY | Scope.USER_READ_PRIVATE
                    | Scope.USER_FOLLOW_MODIFY | Scope.USER_FOLLOW_READ | Scope.PLAYLIST_MODIFY_PRIVATE | Scope.USER_READ_BIRTHDATE | Scope.PLAYLIST_MODIFY_PUBLIC
            };

            //auth = new ImplicitGrantAuth()
            //{
            //    //Your client Id
            //    ClientId = sSpotifyPublicID,
            //    //Set this to localhost if you want to use the built-in HTTP Server
            //    RedirectUri = "http://localhost",
            //    //How many permissions we need?
            //    Scope = Scope.USER_READ_PRIVATE | Scope.USER_READ_EMAIL | Scope.PLAYLIST_READ_PRIVATE | Scope.USER_LIBRARAY_READ | Scope.USER_LIBRARY_MODIFY | Scope.USER_READ_PRIVATE
            //        | Scope.USER_FOLLOW_MODIFY | Scope.USER_FOLLOW_READ | Scope.PLAYLIST_MODIFY_PRIVATE | Scope.USER_READ_BIRTHDATE | Scope.PLAYLIST_MODIFY_PUBLIC
            //};
            //Start the internal http server
            //auth.ShowDialog = false;
            //auth.StartHttpServer();
           //oAutorizationCodeAuth.ShowDialog = false;
            //oAutorizationCodeAuth.StartHttpServer();
            //When we got our response
           //auth.OnResponseReceivedEvent += auth_OnResponseReceivedEvent;
            oAutorizationCodeAuth.OnResponseReceivedEvent += oAutorizationCodeAuth_OnResponseReceivedEvent;
            //Start
            //auth.DoAuth();
            //oAutorizationCodeAuth.DoAuth();

            
         
            //oRepsonse.Code = "BQAkRvpgMZHn76lNx0rVgKM4iwwb6FmZk7MTKlkgSk_ajr60oHxBCapqOXItvgQqk1rZ6CRNckDQ6UMQWqXSgBmQu4v7-k5j9OR2xYiuRWHdXDxFsK_KW_nMK0ikP8zh_7Wf6PhN_py1KG0Gj9A9cGJ61tE9uewUrFMH4Ye8z0tjdXwZmEoPQPd4cpOs8tCiwZp-sSTUfxCGAN8jJOMBUPFsoiJrsgmBI4q7rLtipjUeobnZxJsiG5SFunxU15-9KpHCaNXHMBEsdVYLT82Ctnmu67tFUF5NYRYr0dQf8OA7bqmQmuE";
            //oRepsonse.Code = "AQBSOdyT5Uu68S954-GRWqZWo0zIuUptLQaBPuf-f9Z8xlUosnEFSGfxqzwQbC7Jv1Y_yQLLgWyhcFkB6qNExRu9LLu590nm7CY1H8L0ZIUTugcu_NmF7N8mKnrjXbXakTc";
            //oAutorizationCodeAuth_OnResponseReceivedEvent(oRepsonse);
            //IEnumerable<SpotifyUser> oUsers = SpotifyChangeControlLib.AccessLayer.SpotifyAccessLayer.GetSpotifyUsersAndTokens();
            //foreach (SpotifyUser oSpotifyUser in oUsers)
            //{
            //    CurrentUser = oSpotifyUser;
            //    IEnumerable<SpotifyPlaylist> oPs = oSpotifyUser.Playlist;    

            //    int i = 0;
            //}

        }

        private static void oAutorizationCodeAuth_OnResponseReceivedEvent(AutorizationCodeAuthResponse response)
        {
            oAutorizationCodeAuth.StopHttpServer();
            
            if (response.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + response.Error);
                return;
            }



            Token oToken;
            oToken = oAutorizationCodeAuth.ExchangeAuthCode(response.Code, SCC_PRIVATE_ID);
            //oToken = oAutorizationCodeAuth.RefreshToken(response.Code, SCC_PRIVATE_ID);

            _oSpotifyWebApi = new SpotifyWebAPIClient()
            {
                AccessToken = oToken.AccessToken,
                TokenType = oToken.TokenType,
                UseAuth = true
            };

            //_oSpotifyWebApi = new SpotifyWebAPIClient()
            //{
            //    AccessToken = CurrentUser.Token.AccessToken,
            //    TokenType = CurrentUser.Token.TokenType,
            //    UseAuth = true
            //};

            _oPrivateProfile = _oSpotifyWebApi.GetPrivateProfile();
           
            




            DisplayMenu();

        }

        static void auth_OnResponseReceivedEvent(Token token, string state, string error)
        {
            //stop the http server
            auth.StopHttpServer();

            if(error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + error);
                return;
            }
            _oSpotifyWebApi = new SpotifyWebAPIClient()
                {
                    AccessToken = token.AccessToken,
                    TokenType = token.TokenType,
                    UseAuth = true
                };
            _oPrivateProfile = _oSpotifyWebApi.GetPrivateProfile();
             
           
            DisplayMenu();
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("Choose one of the following Tests:");
            Console.WriteLine("1 - Display Profile information");
            Console.WriteLine("2 - Display all of your playlists");
            Console.WriteLine("3 - List all of your playlist-tracks");
            Console.WriteLine("4 - Remove duplicates from a playlist");
            Console.WriteLine("5 - Remove duplicates from ALL playlists");
            Console.Write("Number: ");
            String number = Console.ReadLine();
            switch (number)
            {
                default:
                    DisplayMenu();
                    break;
                case "1":
                    DisplayProfile();
                    break;
                case "2":
                    DisplayPlaylists();
                    break;
                case "3":
                    DisplayPlaylistTracks();
                    break;
                case "4":
                    RemoveDuplicateTracksFromPlayList();
                    break;
                case "5" :
                    RemoveDuplicateTracksInALLPlaylists();
                    break;
                   
            }
        }
        public static void DisplayProfile()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            PrivateProfile profile = _oSpotifyWebApi.GetPrivateProfile();
            Console.WriteLine("Your Display name: " + profile.DisplayName);
            Console.WriteLine("Your Country: " + profile.Country);
            Console.WriteLine("Your ID: " + profile.Id);
            Console.WriteLine("Account product: " + profile.Product);
            Console.WriteLine("Your images:");
            foreach (Image image in profile.Images)
                Console.WriteLine("- " + image.Url);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            DisplayMenu();
        }

        private static void DisplayPlaylists()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
           
            //List<SimplePlaylist> oSimplePlaylists = new List<SimplePlaylist>();
            //oSimplePlaylists.AddRange(_oSpotifyWebApi.GetPlaylistsOwnedByUser());
            Paging<SimplePlaylist> oPaging = _oSpotifyWebApi.GetUserPlaylists(_oPrivateProfile.Id);

           
            
            Console.WriteLine("Printing playlists...");
            Console.WriteLine("");

            foreach (SimplePlaylist playlist in oPaging.Items)
            {
                if (!playlist.Owner.Id.Equals(_oPrivateProfile.Id))
                {
                    Console.WriteLine(playlist.Name + " (" + playlist.Id + ")**");
                }
                else
                {
                    Console.WriteLine(playlist.Name + " (" + playlist.Id + ")");
                }
               
            }
            while (oPaging.Next != null)
            {
                oPaging = _oSpotifyWebApi.DownloadData<Paging<SimplePlaylist>>(oPaging.Next);
                foreach (SimplePlaylist playlist in oPaging.Items)
                {
                    if (!playlist.Owner.Id.Equals(_oPrivateProfile.Id))
                    {
                        Console.WriteLine(playlist.Name + " (" + playlist.Id + ")**");
                    }
                    else
                    {
                        Console.WriteLine(playlist.Name + " (" + playlist.Id + ")");
                    }

                }
            }

            //foreach (SimplePlaylist playlist in oSimplePlaylists)
            //{
            //    Console.WriteLine(playlist.Name + " (" + playlist.Id + ")");
                
            //}
         
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            DisplayMenu();
        }
        private static void DisplayPlaylistTracks()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("Playlist ID (must be one of yours): ");
            String id = Console.ReadLine();

            Paging<PlaylistTrack> col = _oSpotifyWebApi.GetPlaylistTracks(_oPrivateProfile.Id, id);
            if(col.HasError())
            {
                Console.WriteLine("ERROR: " + col.ErrorResponse.Message);
                DisplayMenu();
                return;
            }
            foreach(PlaylistTrack track in col.Items)
                Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
            while (col.Next != null)
            {
                col = _oSpotifyWebApi.DownloadData<Paging<PlaylistTrack>>(col.Next);
                foreach (PlaylistTrack track in col.Items)
                    Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
            
            }
            DisplayMenu();
        }

        private static void RemoveDuplicateTracksFromPlayList()
        {
            Dictionary<string, List<int>> dicTracks = new Dictionary<string, List<int>>();

            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("Playlist ID (must be one of yours): ");
            String id = Console.ReadLine();

            int iDupTracks = 0;
            FullPlaylist oFullPlaylist = _oSpotifyWebApi.GetPlaylist(_oPrivateProfile.Id, id);
            if (oFullPlaylist != null)
            {
                iDupTracks = oFullPlaylist.RemoveDuplicateTracks(_oSpotifyWebApi);
                Console.WriteLine("Found '{0}' Tracks that were duplicated in '{1}'.", iDupTracks, oFullPlaylist.Name);
            }
            else
            {
                Console.WriteLine("Error, could not find playlist. Are you sure it was yours?...");
            }
           
            DisplayMenu();
        }



        private static void RemoveDuplicateTracksInALLPlaylists()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
           
            Paging<SimplePlaylist> playlists = _oSpotifyWebApi.GetUserPlaylists(_oPrivateProfile.Id);

            Console.WriteLine("Printing playlists...");
            Console.WriteLine("");
            foreach (SimplePlaylist playlist in playlists.Items)
            {

                playlist.RemoveDuplicateTracks(_oSpotifyWebApi);
            }

            while (playlists.Next != null)
            {
                playlists = _oSpotifyWebApi.DownloadData<Paging<SimplePlaylist>>(playlists.Next);
                foreach (SimplePlaylist playlist in playlists.Items)
                {

                    if (playlist.Owner.Id.Equals(_oPrivateProfile.Id))
                    {
                        //Console.WriteLine(playlist.Name + " (" + playlist.Id + ")");
                        playlist.RemoveDuplicateTracks(_oSpotifyWebApi);
                    }
                }
                     
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            DisplayMenu();
        }
      
        
        
    }
}
