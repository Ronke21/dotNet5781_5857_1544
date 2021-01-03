using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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

            #region Bus Lines List
            
            BusLinesList = new List<BusLine>()
            {
                new BusLine()
                {
                    Active = true,
                    AllAccessible = true,
                    BusArea = Area.Jerusalem,
                    BusLineId = KeyGenerator.IdGenerator(),
                    FirstStation = 73,
                    LastStation = 75,
                    LineNumber = 6
                },

                new BusLine()
                {
                    Active = true,
                    AllAccessible = true,
                    BusArea = Area.Jerusalem,
                    BusLineId = KeyGenerator.IdGenerator(),
                    FirstStation = 75,
                    LastStation = 73,
                    LineNumber = 6
                },
            };

            #endregion

            #region Bus station List

            BusStationsList = new List<BusStation>()
            {
                new BusStation()
                {
                    Code = 73,
                    Name = "Golda",
                    Address = "Golda st",
                    Accessible = true,
                    Active = true,
                    Location = new GeoCoordinate(31.825302, 35.188624)
                },

                new BusStation()
                {
                    Code = 74,
                    Name = "Ron",
                    Address = "Ron st",
                    Accessible = false,
                    Active = true,
                    Location = new GeoCoordinate(31.825455, 35.188100)
                },

                new BusStation()
                {
                    Code = 75,
                    Name = "AH",
                    Address = "AH st",
                    Accessible = false,
                    Active = true,
                    Location = new GeoCoordinate(31.825820, 35.187420)
                },

                new BusStation()
                {
                    Code = 81,
                    Name = "RK",
                    Address = "RK st",
                    Accessible = true,
                    Active = false,
                    Location = new GeoCoordinate(32, 34.9)
                }
            };

            #endregion

            #region Consecutive Stations

            ConsecutiveStationsList = new List<ConsecutiveStations>()
            {
                new ConsecutiveStations()
                {
                    StatCode1 = 73,
                    StatCode2 = 74,
                    Distance = new GeoCoordinate(31.825302, 35.188624).
                        GetDistanceTo(new GeoCoordinate(31.825455, 35.188100)),
                    AverageTravelTime = new TimeSpan(0, 3, 0),
                    Active = true
                },

                new ConsecutiveStations()
                {
                    StatCode1 = 74,
                    StatCode2 = 75,
                    Distance = new GeoCoordinate(31.825455, 35.188100).
                        GetDistanceTo(new GeoCoordinate(31.825820, 35.187420)),
                    AverageTravelTime = new TimeSpan(0, 3, 0),
                    Active = true
                },

                new ConsecutiveStations()
                {
                    StatCode1 = 74,
                    StatCode2 = 75,
                    Distance = new GeoCoordinate(31.825820, 35.187420).
                        GetDistanceTo(new GeoCoordinate(31.825820, 35.187420)),
                    AverageTravelTime = new TimeSpan(0, 3, 0),
                    Active = true
                },
            };

            #endregion

            DriversList = new List<Driver>();

            LineExitsList = new List<LineExit>();

            LineStationsList = new List<LineStation>();

            TravelingBusesList = new List<TravelingBus>();

            UsersList = new List<User>();

            UserTravelsList = new List<UserTravel>();
        }
    }
}