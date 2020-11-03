using System;
using System.Data;


namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation
    {
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

            distanceFromLast = r.Next(20);
            interval = new TimeSpan(r.Next(1), r.Next(1, 60), 0);
        }

    }
}
