using System;
using System.Collections.Generic;

namespace dotNet5781_02_5857_1544
{
    class BusStation
    {
        static List<int> unique = new List<int>();
        protected int BusStationKey;
        public int BUSSTATIONKEY
        {
            get { return BusStationKey; }
        }

        protected double Latitude;
        protected double Longitude;
        //protected string address;

        /// <summary>
        /// 
        /// </summary>
        public BusStation()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            Latitude = r.NextDouble() * (33.3 - 31) + 31;
            Longitude = r.NextDouble() * (35.5 - 34.3) + 34.3;

            if (Latitude > 33.3 || Latitude < 31 || Longitude > 35.5 || Longitude < 34.3)
            {
                throw new NotInIsraelException("You can only add stations in Israel");
            }
            
            do
            {
                BusStationKey = r.Next(999999);
            }
            while (unique.Contains(BUSSTATIONKEY));

            //if (unique.Contains(BUSSTATIONKEY))
            //{
            //    throw new StationAlreadyExistsException("Station already exists");
            //}

            //if (BusStationKey > 999999 || BusStationKey < 1)
            //{
            //    throw new OutOfRangeException("Bus station id must be an up to 6 digits integer");
            //}
            
            unique.Add(BusStationKey);
        }

        /// <summary>
        /// return full info about the location of station in string
        /// </summary>
        /// <returns>
        /// string contains the location info about the station
        /// </returns>
        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E";
        }
    }
}
