using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.SpotifyWebAPI;
using SpotifyAPI.SpotifyWebAPI.Models;

namespace SpotifyAPI.Utilities.Extensions
{
     public static class PlaylistExtensions
    {
         /// <summary>
         /// Returns the number of duplicated tracks
         /// </summary>
         /// <param name="oSimplePlaylist"></param>
         /// <param name="_oSpotifyWebApi"></param>
         public static int RemoveDuplicateTracks(this SimplePlaylist oSimplePlaylist, SpotifyWebAPIClient _oSpotifyWebApi)
         {
             int iDupTracks = 0;
             PrivateProfile _oPrivateProfile = _oSpotifyWebApi.GetPrivateProfile();
             if (oSimplePlaylist.Owner.Id.Equals(_oPrivateProfile.Id))
             {
                
                 Dictionary<string, List<int>> dicTracks = new Dictionary<string, List<int>>();
                 int iPos = 0;

                 Paging<PlaylistTrack> col = _oSpotifyWebApi.GetPlaylistTracks(oSimplePlaylist.Id);
                 List<DeleteTrackArg> odelTracks = new List<DeleteTrackArg>();

                 foreach (PlaylistTrack track in col.Items)
                 {
                     // Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
                     if (dicTracks.ContainsKey(track.Track.Uri))
                     {
                         dicTracks[track.Track.Uri].Add(iPos);
                     }
                     else
                     {

                         dicTracks.Add(track.Track.Uri, new List<int> { iPos });

                     }
                     iPos++;
                 }

                 while (col.Next != null)
                 {
                     col = _oSpotifyWebApi.DownloadData<Paging<PlaylistTrack>>(col.Next);
                     foreach (PlaylistTrack track in col.Items)
                     {
                         //Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
                         if (dicTracks.ContainsKey(track.Track.Uri))
                         {
                             dicTracks[track.Track.Uri].Add(iPos);
                         }
                         else
                         {
                             dicTracks.Add(track.Track.Uri, new List<int> { iPos });
                         }
                         iPos++;
                     }
                 }
                 foreach (string sURI in dicTracks.Keys)
                 {
                     //Console.WriteLine(string.Format("{0}:{1}", sURI, dicTracks[sURI].Count));
                     List<int> iPoss = dicTracks[sURI];
                     int iMax = iPoss[iPoss.Count - 1];
                     iPoss.Remove(iMax);
                 }
                 foreach (string sUri in dicTracks.Keys)
                 {
                     List<int> iPoss = dicTracks[sUri];
                     if (iPoss.Count > 0)
                     {
                         DeleteTrackArg oDeleteTrackArg = new DeleteTrackArg(sUri, iPoss.ToArray());
                         odelTracks.Add(oDeleteTrackArg);
                     }

                 }

                 ErrorResponse oErrorResponse = _oSpotifyWebApi.DeletePlaylistTracks(_oPrivateProfile.Id, oSimplePlaylist.Id, odelTracks);
                 if (oErrorResponse != null)
                 {
                     if (oErrorResponse.Error != null)
                     {
                         throw new Exception(string.Format("Error: {0}", oErrorResponse.Error.Message));
                     }
                 }
                 iDupTracks = odelTracks.Count;

             }
             return iDupTracks;
         }
         public static int RemoveDuplicateTracks(this FullPlaylist oFullPlaylist, SpotifyWebAPIClient _oSpotifyWebApi)
         {
             int iDupTracks = 0;
             PrivateProfile _oPrivateProfile = _oSpotifyWebApi.GetPrivateProfile();
             if (oFullPlaylist.Owner.Id.Equals(_oPrivateProfile.Id))
             {
                 Dictionary<string, List<int>> dicTracks = new Dictionary<string, List<int>>();
                 int iPos = 0;

                 Paging<PlaylistTrack> col = _oSpotifyWebApi.GetPlaylistTracks(_oPrivateProfile.Id, oFullPlaylist.Id);
                 List<DeleteTrackArg> odelTracks = new List<DeleteTrackArg>();

                 foreach (PlaylistTrack track in col.Items)
                 {
                     // Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
                     if (dicTracks.ContainsKey(track.Track.Uri))
                     {
                         dicTracks[track.Track.Uri].Add(iPos);
                     }
                     else
                     {

                         dicTracks.Add(track.Track.Uri, new List<int> { iPos });

                     }
                     iPos++;
                 }

                 while (col.Next != null)
                 {
                     col = _oSpotifyWebApi.DownloadData<Paging<PlaylistTrack>>(col.Next);
                     foreach (PlaylistTrack track in col.Items)
                     {
                         //Console.WriteLine(track.Track.Name + " (" + track.Track.Id + ")");
                         if (dicTracks.ContainsKey(track.Track.Uri))
                         {
                             dicTracks[track.Track.Uri].Add(iPos);
                         }
                         else
                         {
                             dicTracks.Add(track.Track.Uri, new List<int> { iPos });
                         }
                         iPos++;
                     }
                 }
                 foreach (string sURI in dicTracks.Keys)
                 {
                     //Console.WriteLine(string.Format("{0}:{1}", sURI, dicTracks[sURI].Count));
                     List<int> iPoss = dicTracks[sURI];
                     int iMax = iPoss[iPoss.Count - 1];
                     iPoss.Remove(iMax);
                 }
                 foreach (string sUri in dicTracks.Keys)
                 {
                     List<int> iPoss = dicTracks[sUri];
                     if (iPoss.Count > 0)
                     {
                         DeleteTrackArg oDeleteTrackArg = new DeleteTrackArg(sUri, iPoss.ToArray());
                         odelTracks.Add(oDeleteTrackArg);
                     }

                 }

                 ErrorResponse oErrorResponse = _oSpotifyWebApi.DeletePlaylistTracks(_oPrivateProfile.Id, oFullPlaylist.Id, odelTracks);
                 if (oErrorResponse != null)
                 {
                     if (oErrorResponse.Error != null)
                     {
                         throw new Exception(string.Format("Error: {0}", oErrorResponse.Error.Message));
                     }
                 }

                 iDupTracks = odelTracks.Count;

             }

             return iDupTracks;
         }
    }

}
