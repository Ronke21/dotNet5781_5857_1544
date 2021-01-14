using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using DalApi;
using DS;
using DO;

namespace Dal
{
    internal sealed class DalObject : IDal
    {

        #region singleton

        private static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        private DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus

        public void AddBus(Bus bus)
        {
            var bu = DataSource.BusesList.Find(b => b.LicenseNum == bus.LicenseNum);

            if (bu == null)
            {
                DataSource.BusesList.Add(bus.Clone());
            }

            else if (bu.Active is false)
            {
                bu.Active = true;
            }

            else throw new BusAlreadyExistsException($"Bus number {bus.LicenseNum} already exists");

        }

        public IEnumerable<Bus> GetAllActiveBuses()
        {
            if (DataSource.BusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from bus in DataSource.BusesList
                   where bus.Active is true
                   select bus.Clone();
        }

        public IEnumerable<Bus> GetAllInActiveBuses()
        {
            if (DataSource.BusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from bus in DataSource.BusesList
                   where bus.Active is false
                   select bus.Clone();
        }

        public DO.Bus GetBus(int licenseNum)
        {
            var bus = DataSource.BusesList.Find(b => b.LicenseNum == licenseNum);
            if (bus != null) return bus;
            throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
        }

        public void UpdateBus(DO.Bus bus)
        {
            var updatedBus = DataSource.BusesList.Find(b => b.LicenseNum == bus.LicenseNum);
            if (updatedBus is null) throw new BusDoesNotExistsException($"Bus number {bus.LicenseNum} does not exist");
            bus.Mover(updatedBus);
        }

        //  UPDATE EXAMPLE
        //public void UpdateFuel(int num, int km)
        //{
        //    var b = DataSource.BusesList.Find(bus => bus.LicenseNum == num);
        //    b.Fuel -= km;
        //}

        public void DeleteBus(int licenseNum)
        {
            var bus = DataSource.BusesList.Find(b => b.LicenseNum == licenseNum);
            if (bus is null) throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            bus.Active = false;
        }

        #endregion

        #region BusStation
        public void AddBusStation(BusStation busStation)
        {
            var bs = DataSource.BusStationsList.Find(b => b.Code == busStation.Code);

            if (bs == null)
            {
                DataSource.BusStationsList.Add(busStation.Clone());
            }

            else if (bs.Active is false)
            {
                bs.Active = true;
            }

            else throw new StationAlreadyExistsException($"Station number {busStation.Code} already exists");
        }
        public IEnumerable<BusStation> GetAllActiveBusStations()
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusStationsList)} is Empty");
            }

            return from bs in DataSource.BusStationsList
                   where bs.Active is true
                   select bs.Clone();
        }
        public IEnumerable<BusStation> GetAllInActiveBusStations()
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusStationsList)} is Empty");
            }

            return from bs in DataSource.BusStationsList
                   where bs.Active is false
                   select bs.Clone();
        }
        public DO.BusStation GetBusStation(int code)
        {
            var bs = DataSource.BusStationsList.Find(b => b.Code == code);
            if (bs != null) return bs;
            throw new StationDoesNotExistException($"Station number {code} does not exist");
        }
        public void UpdateBusStation(DO.BusStation bs)
        {
            var updatedStation = DataSource.BusStationsList.Find(s => s.Code == bs.Code);
            if (updatedStation is null) throw new StationDoesNotExistException($"Station number {bs.Code} does not exist");
            bs.Mover(updatedStation);
        }
        public void DeleteBusStation(int code)
        {
            var bs = DataSource.BusStationsList.Find(b => b.Code == code);
            if (bs is null) throw new StationDoesNotExistException($"Station number {code} does not exist");
            bs.Active = false;
        }

        #endregion

        #region BusLine
        public void AddBusLine(BusLine busLine)
        {
            var bl = DataSource.BusLinesList.Find(b => b.BusLineId == busLine.BusLineId);

            if (bl is null)
            {
                DataSource.BusLinesList.Add(busLine.Clone());
            }
            else if (bl.Active is false)
            {
                bl.Active = true;
            }
            else throw new BusLineAlreadyExistsException($"Bus line {busLine.LineNumber} already exist and active ({busLine.BusLineId})");
        }
        public IEnumerable<BusLine> GetAllActiveBusLines()
        {
            if (DataSource.BusLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusLinesList)} is Empty");
            }

            return from busLine in DataSource.BusLinesList
                   where busLine.Active is true
                   select busLine.Clone();
        }
        public IEnumerable<BusLine> GetAllInActiveBusLines()
        {
            if (DataSource.BusLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusLinesList)} is Empty");
            }

            return from busLine in DataSource.BusLinesList
                   where busLine.Active is false
                   select busLine.Clone();
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            if (DataSource.BusLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusLinesList)} is Empty");
            }

            return from busLine in DataSource.BusLinesList
                   where predicate(busLine)
                   select busLine.Clone();
        }
        public BusLine GetBusLine(int busLineId)
        {
            var busLine = DataSource.BusLinesList.Find(b => b.BusLineId == busLineId);
            if (busLine != null) return busLine;
            throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
        }
        public void ActivateBusLine(int busLineId)
        {
            var busLine = DataSource.BusLinesList.Find(l => l.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            busLine.Active = true;
        }

        public void UpdateBusLine(BusLine update)
        {
            var updatedLine = DataSource.BusLinesList.Find(bl => bl.BusLineId == update.BusLineId);
            if (updatedLine is null) throw new BusDoesNotExistsException($"Line number {update.BusLineId} does not exist");
            update.Mover(updatedLine);
        }

        public void DeleteBusLine(int busLineId)
        {
            var busLine = DataSource.BusLinesList.Find(l => l.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            busLine.Active = false;
        }

        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            var ls = DataSource.LineStationsList.Find(l => l.BusLineId == lineStation.BusLineId &&
                                                            l.Code == lineStation.Code);
            if (ls == null)
            {
                DataSource.LineStationsList.Add(lineStation.Clone());
            }

            else if (ls.Active is false)
            {
                ls.StationIndex = lineStation.StationIndex;
                ls.Active = true;
            }

            else throw new LineStationsAlreadyExistsException($"Line station {lineStation.BusLineId}/{lineStation.Code} already exists");
        }
        public IEnumerable<LineStation> GetAllLineStationsByLineID(int LineID)
        {
            var stations = from ls in DataSource.LineStationsList
                           where ls.BusLineId == LineID && ls.Active is true
                           select ls.Clone();

            if (stations is null)
            {
                throw new LineStationsDoesNotExistsException($"Line {LineID} does not have any stations");
            }

            return stations;
        }
        public IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate)
        {
            var stations = from ls in DataSource.LineStationsList
                           where ls.Active is true && predicate(ls)
                           select ls.Clone();

            if (stations is null)
            {
                throw new LineStationsDoesNotExistsException($"No stations satisfies the condition {predicate}");
            }

            return stations;
        }
        public LineStation GetLineStation(int lineNumber, int stationNumber)
        {
            var lineStation = DataSource.LineStationsList.Find(ls => ls.BusLineId == lineNumber &&
                                                                     ls.Code == stationNumber &&
                                                                     ls.Active is true);
            if (lineStation is null)
            {
                throw new LineStationsDoesNotExistsException($"Line station {lineNumber}/{stationNumber} does not exists or not active");

            }

            return lineStation;
        }
        public void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex)
        {
            var lineStation = GetLineStation(lineNumber, stationNumber);
            lineStation.StationIndex = stationIndex;
        }
        public void DeleteLineStation(int lineNumber, int stationNumber)
        {
            var lineStation = GetLineStation(lineNumber, stationNumber);

            if (lineStation.Active is true)
            {
                lineStation.Active = false;
            }
            //else throw new AlreadyDeletedException($"Line station {lineNumber}/{stationNumber} already deleted");
        }

        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            var conStats = from cs in DataSource.ConsecutiveStationsList
                           where cs.Active
                           select cs;

            if (conStats is null)
            {
                throw new StationsAreNotConsecutiveException("No consecutive stations found");
            }

            return conStats;
        }
        public void AddConsecutiveStations(int statCode1, int statCode2, TimeSpan toNext, double distance)
        {
            var cons = DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                                    c.StatCode2 == statCode2);

            if (cons is null)
            {
                var con =
                    new ConsecutiveStations
                    {
                        StatCode1 = statCode1,
                        StatCode2 = statCode2,
                        Distance = distance,
                        AverageTravelTime = toNext,
                        Active = true
                    };
                DataSource.ConsecutiveStationsList.Add(con);
            }

            else throw new StationsAlreadyConsecutiveException($"Stations {statCode1} and {statCode2} are already consecutive stations");

            // check if redundant
        }
        public ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2)
        {
            var consecutiveStations = DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                                   c.StatCode2 == statCode2);

            if (consecutiveStations is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
            }

            return consecutiveStations;
        }
        public void UpdateConsecutiveStations(ConsecutiveStations conStat)
        {
            var updatedConStat = GetConsecutiveStations(conStat.StatCode1, conStat.StatCode2);
            conStat.Mover(updatedConStat);
        }
        public bool CheckConsecutiveStationsNotExist(int statCode1, int statCode2)
        {
            return (DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                 c.StatCode2 == statCode2) is null);
        }
        //public void DeleteConsecutiveStations(int statCode1, int statCode2)
        //{
        //    var consecutiveStations = DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 && c.StatCode2 == statCode2);
        //    if (consecutiveStations is null)
        //    {
        //        throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
        //    }

        //    DataSource.ConsecutiveStationsList.Remove(consecutiveStations);
        //}

        #endregion

        #region TravelingBus

        public void AddTravelingBus(TravelingBus travelingBus)
        {
            if (DataSource.TravelingBusesList.Find(ls => ls.Id == travelingBus.Id) == null)
            {
                DataSource.TravelingBusesList.Add(travelingBus);
            }
            else throw new TravelingBusesAlreadyExistsException($"Bus {travelingBus.Id} travel already exists");
        }

        public IEnumerable<TravelingBus> GetAllTravelingBuses()
        {
            if (DataSource.TravelingBusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.TravelingBusesList)} is Empty");
            }

            return from travelingBus in DataSource.TravelingBusesList
                   select travelingBus.Clone();
        }

        public IEnumerable<TravelingBus> GetAllTravelingBusesBy(Predicate<TravelingBus> predicate)
        {
            if (DataSource.TravelingBusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.TravelingBusesList)} is Empty");
            }

            return from travelingBus in DataSource.TravelingBusesList
                   where predicate(travelingBus)
                   select travelingBus.Clone();
        }

        public TravelingBus GetTravelingBus(int travelId)
        {
            var travelingBus = DataSource.TravelingBusesList.Find(tb => tb.Id == travelId);
            if (travelingBus != null) return travelingBus;
            throw new TravelingBusesDoesNotExistsException($"Bus {travelId} travel does not exist");
        }

        public void DeleteTravelingBus(int travelId)
        {
            var travelingBus = DataSource.TravelingBusesList.Find(tb => tb.Id == travelId);
            if (travelingBus is null) throw new TravelingBusesDoesNotExistsException($"Bus {travelId} travel does not exist");
            DataSource.TravelingBusesList.Remove(travelingBus);
        }

        #endregion

        #region Driver

        public void AddDriver(Driver driver)
        {
            if (DataSource.DriversList.Find(d => d.Id == driver.Id) == null)
            {
                DataSource.DriversList.Add(driver);
            }
            else throw new DriverAlreadyExistsException($"Driver {driver.Id} already exists");
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            if (DataSource.DriversList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.DriversList)} is Empty");
            }

            return from driver in DataSource.DriversList
                   select driver.Clone();
        }

        public IEnumerable<Driver> GetAllDriversBy(Predicate<Driver> predicate)
        {
            if (DataSource.DriversList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.DriversList)} is Empty");
            }

            return from driver in DataSource.DriversList
                   where predicate(driver)
                   select driver.Clone();
        }

        public Driver GetDriver(int id)
        {
            var driver = DataSource.DriversList.Find(d => d.Id == id);
            if (driver != null) return driver;
            throw new DriverDoesNotExistsException($"Driver {id} does not exist");
        }

        public void UpdateDriver(Driver driver)
        {
            DeleteDriver(driver.Id);
            AddDriver(driver);
        }

        public void DeleteDriver(int id)
        {
            var driver = DataSource.DriversList.Find(d => d.Id == id);
            if (driver is null) throw new DriverDoesNotExistsException($"Driver {id} does not exist");
            DataSource.DriversList.Remove(GetDriver(id));
        }

        #endregion

        #region User

        public void AddUser(User user)
        {
            if (DataSource.UsersList.Find(u => u.UserName == user.UserName) == null)
            {
                DataSource.UsersList.Add(user.Clone());
            }
            else throw new UserAlreadyExistsException($"User {user.UserName} already exists");
        }

        public IEnumerable<User> GetAllUsers()
        {
            if (DataSource.UsersList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.UsersList)} is Empty");
            }

            return from user in DataSource.UsersList
                   select user.Clone();
        }
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            if (DataSource.UsersList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.UsersList)} is Empty");
            }

            return from user in DataSource.UsersList
                   where predicate(user)
                   select user.Clone();
        }

        public User GetUser(string username)
        {
            var user = DataSource.UsersList.Find(u => u.UserName == username);
            if (user != null) return user;
            throw new UserDoesNotExistsException($"User {username} does not exist");
        }

        public void UpdateUser(User user)
        {
            DeleteUser(user.UserName);
            AddUser(user);
        }

        public void UpdateUsername(string username)
        {
            var user = GetUser(username);
            user.UserName = username;
        }

        public void UpdatePassword(string username, string password)
        {
            var user = GetUser(username);
            user.Password = password;
        }

        public void DeleteUser(string username)
        {
            var user = DataSource.UsersList.Find(b => b.UserName == username);
            if (user is null) throw new UserDoesNotExistsException($"User {username} does not exist");
            DataSource.UsersList.Remove(GetUser(username));
        }

        #endregion

        #region UserTravel

        public void AddUserTravel(UserTravel userTravel)
        {
            if (DataSource.UserTravelsList.Find(ut => ut.TravelId == userTravel.TravelId) == null)
            {
                DataSource.UserTravelsList.Add(userTravel.Clone());
            }
            else throw new UserTravelAlreadyExistsException($"User travel number {userTravel.TravelId} already exists");
        }

        public IEnumerable<UserTravel> GetAllUserTravels()
        {
            if (DataSource.UserTravelsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from ut in DataSource.UserTravelsList
                   select ut.Clone();
        }

        public IEnumerable<UserTravel> GetAllUserTravelsBy(Predicate<UserTravel> predicate)
        {
            if (DataSource.UserTravelsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from ut in DataSource.UserTravelsList
                   where predicate(ut)
                   select ut.Clone();
        }

        public UserTravel GetUserTravel(int travelId)
        {
            var userTravel = DataSource.UserTravelsList.Find(ut => ut.TravelId == travelId);
            if (userTravel != null) return userTravel;
            throw new UserTravelDoesNotExistsException($"User travel number {userTravel.TravelId} does not exist");
        }

        public void DeleteUserTravel(int travelId)
        {
            var userTravel = DataSource.UserTravelsList.Find(ut => ut.TravelId == travelId);
            if (userTravel is null) throw new UserTravelDoesNotExistsException($"User travel number {travelId} does not exist");
            DataSource.UserTravelsList.Remove(GetUserTravel(travelId));
        }

        #endregion
        public int GetKey()
        {
            return DS.KeyGenerator.IdGenerator();
        }
    }
}