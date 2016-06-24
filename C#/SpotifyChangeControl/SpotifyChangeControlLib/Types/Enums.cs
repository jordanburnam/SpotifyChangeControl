using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Types
{
    class Enums
    {
        [Flags]
        public enum DatabaseTableState { Truncated, Heaped, Querable, Unknown}
    }
}
