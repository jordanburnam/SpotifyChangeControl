using SpotifyChangeControlLib.AccessLayer;
using SpotifyChangeControlLib.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyChangeControlLib.Types.Abstract
{
    public abstract class SpotifyObjectBase : ISpotifyObject
    {
        private Int64 _ID;
        private string _SpotifyID;
        private string _Name;

        public long ID
        {
            get
            {
                return this._ID;
            }

            set
            {
                this._ID = value;
            }
        }

        public string SpotifyID
        {
            get
            {
                return this._SpotifyID;
            }
            set
            {
                this._SpotifyID = value;
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }

            set
            {
                this._Name = value;
            }
        }

        protected SpotifyObjectBase(Int64 iID, string sName)
        {
            this._ID = iID;
            this._Name = sName;
        }

        protected SpotifyObjectBase(string sSpotifyID, string Name)
        {
            this.Name = Name;
            this.SpotifyID = sSpotifyID;
            SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(this);
        }

        public static bool operator == (SpotifyObjectBase obj1, SpotifyObjectBase obj2)
        {
            if (obj1.ID == 0)
            {
                SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(obj1);
            }

            if (obj2.ID == 0)
            {
                SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(obj2);
            }

            return obj1.ID == obj2.ID;
        }

        public static bool operator !=(SpotifyObjectBase obj1, SpotifyObjectBase obj2)
        {
            if (obj1.ID == 0)
            {
                SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(obj1);
            }

            if (obj2.ID == 0)
            {
                SpotifyAccessLayer.UpdateObjectIDForSpotifyObject(obj2);
            }

            return obj1.ID != obj2.ID;
        }
    }
}
