using System;


namespace dotNet5781_02_5857_1544
{
    class BusLineStation : BusStation
    {
        private double distanceFromLast;
        private DateTime rideTimeFromLast;

        public BusLineStation(int code, string add, double distance, DateTime rideTime) : base(code, add)
        {
            distanceFromLast = distance;
            rideTimeFromLast = rideTime;
        }

    }
}
