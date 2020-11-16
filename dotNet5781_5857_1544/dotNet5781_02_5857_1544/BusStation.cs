using System;
using System.Collections.Generic;
using System.Device.Location;

namespace dotNet5781_02_5857_1544
{

    class BusStation
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        static List<int> unique_id = new List<int>();
        static List<BusStation> unique_station = new List<BusStation>();
        protected int BusStationKey;
        public int BUSSTATIONKEY
        {
            get { return BusStationKey; }
        }

        public readonly double Latitude;
        public readonly double Longitude;
        //protected string address;

        /// <summary>
        /// this ctor randomly assigns coordinates and bus station key
        /// </summary>
        public BusStation()
        {

            Latitude = System.Math.Round(r.NextDouble() * (33.3 - 31) + 31, 3);
            Longitude = System.Math.Round(r.NextDouble() * 35.5 - 34.3 + 34.3, 3);
            do
            {
                BusStationKey = r.Next(999999);
            }
            while (unique_id.Contains(BUSSTATIONKEY));
            unique_id.Add(BusStationKey);
            unique_station.Add(this);
            
            //GeoCoordinate Location = new GeoCoordinate(Latitude, Longitude);
        }

        public int ReturnIndex(int id)
        {
            int index = 0;
            foreach (var stat in unique_station)
            {
                if (stat.BUSSTATIONKEY == id)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public BusStation(int id)
        {
            BusStationKey = id;
            if (unique_id.Contains(id))
            {
                int index = ReturnIndex(id);
                Latitude = unique_station[index].Latitude;
                Longitude = unique_station[index].Longitude;
            }
            else
            {
                Latitude = System.Math.Round(r.NextDouble() * (33.3 - 31) + 31, 3);
                Longitude = System.Math.Round(r.NextDouble() * 35.5 - 34.3 + 34.3, 3);
                unique_id.Add(BusStationKey);
                unique_station.Add(this);
            }
        }

        /// <summary>
        /// print out the station details in an orderly fashion
        /// </summary>
        /// <returns>
        /// a string that represents the current bus station
        /// </returns>
        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E";
        }
    }
}
