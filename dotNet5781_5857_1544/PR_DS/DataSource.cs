using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
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
        //public static List<LineExit> LineExitsList;
        public static List<LineStation> LineStationsList;
        public static List<TravelingBus> TravelingBusesList;
        public static List<User> UsersList;
        public static List<UserTravel> UserTravelsList;

        public static Dictionary<int, string> Match;

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
                    FirstStation = 39827,
                    LastStation = 39826,
                    LineNumber = 6
                },

                new BusLine()
                {
                    Active = true,
                    AllAccessible = false,
                    BusArea = Area.Jerusalem,
                    BusLineId = KeyGenerator.IdGenerator(),
                    FirstStation = 39826,
                    LastStation = 39827,
                    LineNumber = 6
                },
            };

            #endregion

            #region Bus station List

            var code = File.ReadAllLines(@"..\PR_DS\DataSource\code.txt");
            var name = File.ReadAllLines(@"..\PR_DS\DataSource\name.txt");
            var longitude = File.ReadAllLines(@"..\PR_DS\DataSource\longitude.txt");
            var latitude = File.ReadAllLines(@"..\PR_DS\DataSource\latitude.txt");
            var address = File.ReadAllLines(@"..\PR_DS\DataSource\address.txt");

            BusStationsList = new List<BusStation>();
            //Match = new Dictionary<int, string>();

            for (var i = 0; i < 27816; i++)
            {
                BusStationsList.Add(
                    new BusStation()
                    {
                        //ID = KeyGenerator.IdGenerator(),
                        Accessible = true,
                        Active = true,
                        Address = address[i],
                        Code = Convert.ToInt32(code[i]),
                        Location = new GeoCoordinate(Convert.ToDouble(latitude[i]), Convert.ToDouble(longitude[i])),
                        Name = name[i]
                    });

                //Match.Add(Convert.ToInt32(code[i]), name[i]);
            }
            //BusStationsList.Add(
            //    new BusStation()
            //    {
            //        ID = KeyGenerator.IdGenerator(),
            //        Accessible = true,
            //        Active = true,
            //        Address = address[22633],
            //        Code = Convert.ToInt32(code[22633]),
            //        Location = new GeoCoordinate(Convert.ToDouble(latitude[22633]), Convert.ToDouble(longitude[22633])),
            //        Name = name[22633]
            //    });
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

            #region Line station

            LineStationsList = new List<LineStation>()
            {
                new LineStation()
                {
                    StationIndex = 0,
                    StationNumber = 38831,
                    Active = true,
                    BusLineId = 0
                },

                new LineStation()
                {
                    StationIndex = 2,
                    StationNumber = 38832,
                    Active = true,
                    BusLineId = 0
                },

                new LineStation()
                {
                    StationIndex = 1,
                    StationNumber = 38833,
                    Active = true,
                    BusLineId = 0
                },

                new LineStation()
                {
                    StationIndex = 3,
                    StationNumber = 38834,
                    Active = true,
                    BusLineId = 0
                },

                new LineStation()
                {
                    StationIndex = 4,
                    StationNumber = 38836,
                    Active = true,
                    BusLineId = 0
                }
            };

            #endregion
            
            //LineExitsList = new List<LineExit>();

            TravelingBusesList = new List<TravelingBus>();

            UsersList = new List<User>();

            UserTravelsList = new List<UserTravel>();
        }
    }
}