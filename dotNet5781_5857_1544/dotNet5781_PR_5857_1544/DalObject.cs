using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DS;
using DalObject;
using DO;
using PR_DalApi;

namespace Dal
{
    sealed class DalObject : IDal
    {
        static DalObject() { }
        private DalObject() { }
        public static DalObject Instance { get; } = new DalObject();


        #region Bus

        public void AddBus(Bus bus)
        {
            if (DataSource.BusesList.Find(b => b.LicenseNum == bus.LicenseNum) == null)
            {
                DataSource.BusesList.Add(bus.Clone());
            }
            else throw new BusAlreadyExistsException($"Bus number {bus.LicenseNum} already exists");
        }

        public IEnumerable<DO.Bus> GetAllBuses()
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

            return from LineStation in DataSource.LineStationsList
                   select LineStation.Clone();
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

        }

        public IEnumerable<TravelingBus> GetAllTravelingBuses()
        {

        }

        public IEnumerable<TravelingBus> GetAllTravelingBusesBy(Predicate<TravelingBus> predicate)
        {

        }

        public TravelingBus GetTravelingBus(int travelId)
        {

        }

        public void DeleteTravelingBus(int travelId)
        {

        }

        #endregion

        #region User
        public void AddUser(User user);
        public IEnumerable<User> GetAllUsers();
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        public User GetUser(string username);
        public void UpdateUser(User user);
        public void UpdateUsername(string username);
        public void UpdatePassword(string password);
        public void DeleteUser(string username, string password);

        #endregion

        #region UserTravel
        public void AddUserTravel(UserTravel userTravel);
        public IEnumerable<UserTravel> GetAllUserTravels();
        public IEnumerable<UserTravel> GetAllUserTravelsBy(Predicate<UserTravel> predicate);
        public UserTravel GetUserTravel(int travelId);
        public void DeleteUserTravel(int travelId);

        #endregion
    }
}
