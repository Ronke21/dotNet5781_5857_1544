using System;
using System.Collections.Generic;

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


    class BusLine
    {
        private int BusLineNumber;
        private List<BusLineStation> Stations;
        private BusLineStation FirstStation;
        private BusLineStation LastStation;
        private string Area;

        public BusLine(int num)
        {

        }
        public override string ToString()
        {
            //לייצר 2 מחרוזות עם איטרטורים שמכילות כל התחנות
            return "Bus Line Number: " + BusLineNumber + ", Area: " + Area + "\n Stations regular side:  " + "\n Stations reverse side:  ";
        }

        public void addStation(BusLineStation stat, int place)
        {
            Stations.Insert(place, stat);
            if (place == 1)
                FirstStation = stat;
            if (place == Stations.Count)
                LastStation = stat;
        }

        public void delStation(BusLineStation stat)
        {
            Stations.Remove(stat);
            //לבדוק אם זה מתייחס כמצביע או ערך
            if (FirstStation == stat)
                FirstStation = Stations[0];
            if (LastStation == stat)
                LastStation = Stations[-1];
        }

        public bool existStation(BusLineStation stat)
        {
            return Stations.Exists(stat);
        }
    }

    //מסעיף 4 ואילך

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
