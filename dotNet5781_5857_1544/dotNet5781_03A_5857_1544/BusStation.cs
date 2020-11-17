/*
 
A class representing a station of a bus - its ID, and geographic loaction(in state of Israel)

 */
using System;
using System.Collections.Generic;
using System.Device.Location;

namespace dotNet5781_02_5857_1544
{

    class BusStation
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        static List<int> unique_id = new List<int>(); //list of all station id - to prevent duplicates
        static List<BusStation> unique_station = new List<BusStation>(); //list of all stations - to prevent duplicates
        protected int BusStationKey; //6 digits ID of station, private - can't be changed
        public int BUSSTATIONKEY //geter
        {
            get { return BusStationKey; }
        }
        
        //Location of station - given once in the constructor
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
            while (unique_id.Contains(BUSSTATIONKEY)); //prevent duplicate station ID
            unique_id.Add(BusStationKey);
            unique_station.Add(this);
            
            //GeoCoordinate Location = new GeoCoordinate(Latitude, Longitude);
        }

        /// <summary>
        /// a help method - recives station id and returns its index in the station id list "uniqe"
        /// </summary>
        /// <param name="id"> id of station to be searchd </param>
        /// <returns> the index of station or -1 if not found </returns>
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

        /// <summary>
        /// this ctor recives a station ID and randomly assigns coordinates
        /// </summary>
        public BusStation(int id)
        {
            BusStationKey = id;
            if (unique_id.Contains(id)) //id exist = set same location (used for adding an existing station to a different bus)
            {
                int index = ReturnIndex(id);
                Latitude = unique_station[index].Latitude;
                Longitude = unique_station[index].Longitude;
            }
            else //id is new - give random location and update list
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
