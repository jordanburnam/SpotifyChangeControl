using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Types
{
    class Enums
    {
        /*
            I would like to take this oppurtunity to 
            allow you, the reader of this code, to get 
            to know me on an intimate level...

            Everytime I hear the word Enum I 
            picture the cookie monster from sesame street
        */
        [Flags]
        public enum DatabaseTableState { Truncated, Heaped, Querable, Unknown}
    }
}
