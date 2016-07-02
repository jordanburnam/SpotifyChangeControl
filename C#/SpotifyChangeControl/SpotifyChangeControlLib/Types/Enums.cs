using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Types
{
    internal class Enums
    {
        /*
            I would like to take this oppurtunity to 
            allow you, the reader of this code, to get 
            to know me on an intimate level...

            Everytime I hear the word Enum I 
            picture the cookie monster from sesame street
        */
        [Flags]
        internal enum DatabaseTableState
        {
            Truncated = 0x01,
            Whoops2 =   0x02,
            Heaped =    0x04,
            Whoops8 =   0x08,
            Querable =  0x10,
            Unknown =   0x20
        }
    }
}
