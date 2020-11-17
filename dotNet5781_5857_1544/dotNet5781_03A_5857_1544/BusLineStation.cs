/*
 
A class representing a specific station refering to a specific bus line. inherits busStation,
abf adding the distance and driving time from the last station

*/

using System;
using System.Device.Location;


namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation, IEquatable<BusLineStation> //inheritance from BusStation, and implementing equal from iequatable interface
    {
        public readonly TimeSpan MAX_INTERVAL = new TimeSpan(1, 0, 0); //a permanent value - 1 hour is the maximum ride time between stations

        private readonly int urbanSpeed = 16; //driving speed inside the city (approximatly 60 kmh, is 16 meters per sec)
        private readonly int INTERURBANSPEED = 25; //driving speed outside the city (approximatly 90 kmh, is 25 meters per sec)

        private double distanceFromLast;
        public double DISTANCEFROMLAST //distance from last station in meters
        {
            get { return distanceFromLast; }
        }

        public int Calc_dist_from_last() //calculate distance according to landmarks
        {
            var a = new GeoCoordinate(Latitude, Longitude);
            return 0;
        }


        private TimeSpan interval;
        public TimeSpan INTERVAL
        {
            get { return interval; }
        }

        /// <summary>
        /// this ctor calls the father empty ctor, and sets a random distance from last, and calculate the riding time
        /// </summary>
        public BusLineStation() : base()
        {
            distanceFromLast = r.Next(20); 

            //  calculate time interval in seconds
            interval = new TimeSpan(0, 0, (int)(distanceFromLast / urbanSpeed));
            if (interval > MAX_INTERVAL) //more than hour
            {
                throw new TooLongException(interval.ToString());
            }
        }

        /// <summary>
        /// this ctor calls the father ctor with a given station id, and sets a random distance from last, and calculate the riding time
        /// </summary>
        /// <param name="id"> the key number of the wanted station</param>
        public BusLineStation(int id) : base(id)
        {
            distanceFromLast = r.Next(20); 

            //  calculate time interval in seconds
            interval = new TimeSpan(0, 0, (int)(distanceFromLast / urbanSpeed));
            if (interval > MAX_INTERVAL)
            {
                throw new TooLongException(interval.ToString());
            }
        }
        /// <summary>
        /// overiding the Equals method - to compare between 2 stations by their id.
        /// it helps to use list.contains
        /// </summary>
        /// <param name="other"> a busline station to be compared </param>
        /// <returns> true if equal by id, else false </returns>
        public bool Equals(BusLineStation other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            //return MAX_INTERVAL.Equals(other.MAX_INTERVAL) &&
            //       urbanSpeed == other.urbanSpeed &&
            //       INTERURBANSPEED == other.INTERURBANSPEED &&
            //       distanceFromLast.Equals(other.distanceFromLast) &&
            //       interval.Equals(other.interval);
            return other != null && this.BUSSTATIONKEY == other.BUSSTATIONKEY;
        }
    }
}
