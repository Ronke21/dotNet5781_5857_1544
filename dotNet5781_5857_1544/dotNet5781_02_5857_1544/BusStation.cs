using System;

namespace dotNet5781_02_5857_1544
{
    class BusStation
    {
        protected int BusStationKey;
        protected double Latitude;
        protected double Longitude;
        protected string address = "";

        // קונסטרקטור ריק???
        public BusStation(int code, string add = "")
        {
            Random r = new Random(DateTime.Now.Millisecond);
            Latitude = r.NextDouble() * (33.3 - 31) + 31;
            Longitude = r.NextDouble() * 35.5 - 34.3 + 34.3;
            BusStationKey = code;
            address = add;
        }
        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E";
        }
    }
}
