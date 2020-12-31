using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000001,
                    Active = true,
                    Fuel = 151,
                    Mileage = 25001,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000002,
                    Active = true,
                    Fuel = 152,
                    Mileage = 25002,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000003,
                    Active = true,
                    Fuel = 153,
                    Mileage = 25003,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000004,
                    Active = true,
                    Fuel = 154,
                    Mileage = 25004,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000005,
                    Active = true,
                    Fuel = 155,
                    Mileage = 25005,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000006,
                    Active = true,
                    Fuel = 153,
                    Mileage = 25006,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000007,
                    Active = true,
                    Fuel = 153,
                    Mileage = 25004,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000008,
                    Active = true,
                    Fuel = 153,
                    Mileage = 25004,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
                },

                new Bus()
                {
                    LicenseNum = 10000009,
                    Active = false,
                    Fuel = 153,
                    Mileage = 25004,
                    StartTime = new DateTime(2020,10,14),
                    Stat = Status.Ready,
                    LastMaint = new DateTime(2020,10,10)
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