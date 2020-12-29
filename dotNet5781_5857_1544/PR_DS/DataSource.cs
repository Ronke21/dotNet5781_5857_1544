using System;
using System.Collections.Generic;
using DO;

namespace DS
{
    public static class KeyGenerator
    // after changing to XML the key will be stored in the NVM to guarantee uniformity
    {
        private static int _key = 0;
        public static int IdGenerator()
        {
            return _key++;
        }
    }

    public static class DataSource
    {
        public static List<Bus> BusesList;
        public static List<BusLine> BusLinesList;
        public static List<BusStation> BusStationsList;
        public static List<ConsecutiveStations> ConsecutiveStationsList;
        public static List<Driver> DriversList;
        public static List<LineExit> LineExitsList;
        public static List<LineStation> LineStationsList;
        public static List<TravelingBus> TravelingBusesList;
        public static List<User> UsersList;
        public static List<UserTravel> UserTravelsList;

        static DataSource()
        {
            InitializeLists();
        }

        private static void InitializeLists()
        {
            #region Bus List

            BusesList = new List<Bus>()
            {
                new Bus()
                {
                    LicenseNum = 10000000,
                    Active = true,
                    Fuel = 150,
                    Mileage = 25000,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready
                },

                new Bus()
                {
                    LicenseNum = 10000001,
                    Active = true,
                    Fuel = 150,
                    Mileage = 25000,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready
                },

                new Bus()
                {
                    LicenseNum = 10000002,
                    Active = true,
                    Fuel = 150,
                    Mileage = 25000,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready
                }
            };

            #endregion


            BusLinesList = new List<BusLine>();

            BusStationsList = new List<BusStation>();

            ConsecutiveStationsList = new List<ConsecutiveStations>();

            DriversList = new List<Driver>();

            LineExitsList = new List<LineExit>();

            LineStationsList = new List<LineStation>();

            TravelingBusesList = new List<TravelingBus>();

            UsersList = new List<User>();

            UserTravelsList = new List<UserTravel>();
        }
    }
}