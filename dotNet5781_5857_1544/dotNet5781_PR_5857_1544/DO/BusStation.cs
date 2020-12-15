using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusStation
    {
        #region Code

        private readonly int code;

        public int Code
        {
            get { return code; }
        }

        #endregion

        #region Location

        private readonly GeoCoordinate location;

        public GeoCoordinate Location
        {
            get { return location; }
        }

        #endregion

        #region Name

        private string name;

        public string Name
        {
            get { return name; }
        }

        #endregion

        #region Address

        private readonly string address;

        public string Address
        {
            get { return address; }
        }

        #endregion

        #region Accessibility

        private bool accessible;

        public bool Accessible
        {
            get { return accessible; }
        }

        #endregion

        #region Active

        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        #endregion


        #region CTOR

        public BusStation(int key, GeoCoordinate loc, string n, string ad, bool acc)
        {
            this.code = key;
            this.location = loc;
            this.name = n;
            this.address = ad;
            this.accessible = acc;
            this.active = true;
        }

        #endregion
    }
}
