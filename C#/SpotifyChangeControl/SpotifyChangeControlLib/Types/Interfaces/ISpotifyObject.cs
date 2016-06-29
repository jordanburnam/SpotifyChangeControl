using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Types.Interfaces
{
    interface ISpotifyObject
    {
        string SpotifyID { get; set; }
        Int64 ID { get; set;}

        string Name { get; set; }
        bool LookedUpObjectID { get; }

    }
}
