using System;
using System.Data;

namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation, IEquatable<BusLineStation>
    {
        public readonly TimeSpan MAX_INTERVAL = new TimeSpan(1, 0, 0);

        private readonly int urbanSpeed = 16;
        private readonly int INTERURBANSPEED = 25;

        private double distanceFromLast;
        public double DISTANCEFROMLAST
        {
            get { return distanceFromLast; }
        }


        private TimeSpan interval;
        public TimeSpan INTERVAL
        {
            get { return interval; }
        }


        public BusLineStation() : base()
        {
            Random r = new Random();

            distanceFromLast = r.Next(20); // replace with real distance (m)

            //  calculate time interval in seconds
            interval = new TimeSpan(0, 0, (int)(distanceFromLast / urbanSpeed));
            if (interval > MAX_INTERVAL)
            {
                throw new TooLongException(interval.ToString());
            }
        }
        public BusLineStation(int id) : base(id)
        {
            Random r = new Random();

            distanceFromLast = r.Next(20); // replace with real distance (m)

            //  calculate time interval in seconds
            interval = new TimeSpan(0, 0, (int)(distanceFromLast / urbanSpeed));
            if (interval > MAX_INTERVAL)
            {
                throw new TooLongException(interval.ToString());
            }
        }

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
