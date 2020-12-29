using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DS;
using DO;
using PR_DalApi;

namespace Dal
{
    internal sealed class DalObject : IDal
    {

        #region singelton
        // ReSharper disable once InconsistentNaming
        private static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        private DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region Bus

        public void AddBus(Bus bus)
        {
            if (DataSource.BusesList.Find(b => b.LicenseNum == bus.LicenseNum) == null)
            {
                DataSource.BusesList.Add(bus.Clone());
            }
            else throw new BusAlreadyExistsException($"Bus number {bus.LicenseNum} already exists");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            if (DataSource.BusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from bus in DataSource.BusesList
                   select bus.Clone();
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            if (DataSource.BusesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusesList)} is Empty");
            }

            return from bus in DataSource.BusesList
                   where predicate(bus)
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
            DeleteBus(bus.LicenseNum);
            AddBus(bus);
        }

        //  UPDATE EXAMPLE
        //public void UpdateFuel(int num, int km)
        //{
        //    var b = DataSource.BusesList.Find(bus => bus.LicenseNum == num);
        //    if (b is null) throw new BusDoesNotExistsException($"Bus number {num} does not exist");
        //    b.Fuel -= km;
        //}

        public void DeleteBus(int licenseNum)
        {
            var bus = DataSource.BusesList.Find(b => b.LicenseNum == licenseNum);
            if (bus is null) throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            DataSource.BusesList.Remove(GetBus(licenseNum));
        }

        #endregion

        #region BusLine
        public void AddBusLine(BusLine busLine)
        {
            if (DataSource.BusLinesList.Find(b => b.BusLineId == busLine.BusLineId) == null)
            {
                DataSource.BusLinesList.Add(busLine.Clone());
            }
            else throw new BusLineAlreadyExistsException($"Bus number {busLine.BusLineId} already exists");
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            if (DataSource.BusLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusLinesList)} is Empty");
            }

            return from busLine in DataSource.BusLinesList
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

        public void UpdateBusLine(BusLine busLine)
        {
            DeleteBusLine(busLine.BusLineId);
            AddBusLine(busLine);
        }

        public void DeleteBusLine(int busLineId)
        {
            var busLine = DataSource.BusLinesList.Find(l => l.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            DataSource.BusLinesList.Remove(GetBusLine(busLineId));
        }

        #endregion

        #region BusStation
        public void AddBusStation(BusStation busStation)
        {
            if (DataSource.BusStationsList.Find(b => b.Code == busStation.Code) == null)
            {
                DataSource.BusStationsList.Add(busStation);
            }
            else throw new StationAlreadyExistsException($"Station {busStation.Code} already exists");
        }
        public IEnumerable<BusStation> GetAllBusStations()
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusStationsList)} is Empty");
            }

            return from busStation in DataSource.BusStationsList
                   select busStation.Clone();
        }

        public IEnumerable<BusStation> GetAllBusLinesBy(Predicate<BusStation> predicate)
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.BusStationsList)} is Empty");
            }

            return from busStation in DataSource.BusStationsList
                   where predicate(busStation)
                   select busStation.Clone();
        }

        public BusStation GetBusStation(int code)
        {
            var busStation = DataSource.BusStationsList.Find(b => b.Code == code);
            if (busStation != null) return busStation;
            throw new StationDoesNotExistException($"Bus line number {code} does not exist");
        }
        public void UpdateBusStation(BusStation busStation)
        {
            DeleteBusStation(busStation.Code);
            AddBusStation(busStation);
        }

        public void DeleteBusStation(int code)
        {
            var busStation = DataSource.BusStationsList.Find(s => s.Code == code);
            if (busStation is null) throw new StationDoesNotExistException($"Station {code} does not exist");
            DataSource.BusStationsList.Remove(GetBusStation(code));
        }

        #endregion

        #region ConsecutiveStations
        //IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        public void AddConsecutiveStations(int statCode1, int statCode2)
        {
            if (DataSource.ConsecutiveStationsList.Find((c => c.StatCode1 == statCode1 && c.StatCode2 == statCode2)) == null)
            {
                var con = new ConsecutiveStations { StatCode1 = statCode1, StatCode2 = statCode2 };
                DataSource.ConsecutiveStationsList.Add(con);
            }
            else throw new StationsAlreadyConsecutiveException($"Stations {statCode1} and {statCode2} are already consecutive stations");
        }

        public ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2)
        {
            var consecutiveStations = DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 && c.StatCode2 == statCode2);
            if (consecutiveStations != null) return consecutiveStations;
            throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
        }

        public void DeleteConsecutiveStations(int statCode1, int statCode2)
        {
            var consecutiveStations = DataSource.ConsecutiveStationsList.Find(c => c.StatCode1 == statCode1 && c.StatCode2 == statCode2);
            if (consecutiveStations is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
            }

            DataSource.ConsecutiveStationsList.Remove(consecutiveStations);
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

        #region LineStation

        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.LineStationsList.Find(ls => ls.LineNumber == lineStation.LineNumber &&
                                                       ls.StationNumber == lineStation.StationNumber) == null)
            {
                DataSource.LineStationsList.Add(lineStation);
            }
            else throw new LineStationsAlreadyExistsException($"Line station {lineStation.LineNumber}/{lineStation.StationNumber} already exists");
        }

        public IEnumerable<LineStation> GetAlLineStations()
        {
            if (DataSource.LineStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.LineStationsList)} is Empty");
            }

            return from lineStation in DataSource.LineStationsList
                   select lineStation.Clone();
        }

        public IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate)
        {
            if (DataSource.LineStationsList.Count == 0)
            {
                throw new EmptyListException($"{nameof(DataSource.LineStationsList)} is Empty");
            }

            return from lineStation in DataSource.LineStationsList
                   where predicate(lineStation)
                   select lineStation.Clone();
        }

        public LineStation GetLineStation(int lineNumber, int stationNumber)
        {
            var lineStation = DataSource.LineStationsList.Find(ls => ls.LineNumber == lineNumber && ls.StationNumber == stationNumber);
            if (lineStation != null) return lineStation;
            throw new LineStationsDoesNotExistsException($"Line station {lineNumber}/{stationNumber} does not exists");
        }

        public void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex)
        {
            var lineStation = GetLineStation(lineNumber, stationNumber);
            lineStation.StationIndex = stationIndex;
        }

        public void DeleteLineStation(int lineNumber, int stationNumber)
        {
            var lineStation = GetLineStation(lineNumber, stationNumber);
            DataSource.LineStationsList.Remove(lineStation);
        }

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
    }
}
