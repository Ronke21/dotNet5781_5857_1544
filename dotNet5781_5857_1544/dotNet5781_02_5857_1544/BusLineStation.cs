using System;
using System.Data;

namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation
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

    }
}
