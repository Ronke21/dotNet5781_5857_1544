using System;
using System.Data;


namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation
    {
        private int urbanspeed = 16;
        private int INTERURBANSPEED = 25;
        
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
           interval = new TimeSpan(0,0,(int)(distanceFromLast / urbanspeed));

        }

    }
}
