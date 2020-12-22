using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DS;
using Dal;
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
            else throw new BusAlreadyExistsException($"Bus number {bus.LicenseNum} already exist");
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
            else throw new BusLineAlreadyExistsException($"Bus number {busLine.BusLineId} already exist");
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

        public void UpdateBusLine(BusLine bus)
        {
            DeleteBusLine(bus.BusLineId);
            AddBusLine(bus);
        }

        public void DeleteBusLine(int busLineId)
        {
            var busLine = DataSource.BusLinesList.Find(b => b.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            DataSource.BusesList.Remove(GetBus(busLineId));
        }

        #endregion

        #region BusStation
        void AddBusStation(BusStation busStation);
        IEnumerable<BusStation> GetAllBusStations();
        IEnumerable<BusStation> GetAllBusLinesBy(Predicate<BusStation> predicate);
        BusStation GetBusStation(int code);
        void UpdateBusStation(BusStation bus);
        void DeleteBusStation(int code);

        #endregion

        #region ConsecutiveStations
        //IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        void AddConsecutiveStations(int statCode1, int statCode2);
        ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2);
        void DeleteConsecutiveStations(ConsecutiveStations consecutiveStations);

        #endregion

        #region Driver

        void AddDriver(Driver driver);
        IEnumerable<Driver> GetAllDrivers();
        IEnumerable<Driver> GetAllDriversBy(Predicate<Driver> predicate);
        Driver GetDriver(int id);
        void UpdateDriver(Driver driver);
        void DeleteDriver(int id);

        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        IEnumerable<LineStation> GetAlLineStations();
        IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate);
        LineStation GetLineStation(int lineNumber, int stationNumber);
        void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex);
        void DeleteLineStation(int lineNumber, int stationNumber);

        #endregion

        #region TravelingBus
        void AddTravelingBus(TravelingBus travelingBus);
        IEnumerable<TravelingBus> GetAllTravelingBuses();
        IEnumerable<TravelingBus> GetAllTravelingBusesBy(Predicate<TravelingBus> predicate);
        TravelingBus GetTravelingBus(int travelId);
        //void Update(TravelingBus travelingBus);
        void DeleteTravelingBus(int travelId);

        #endregion

        #region User
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string username);
        void UpdateUser(User user);
        void UpdateUsername(string username);
        void UpdatePassword(string password);
        void DeleteUser(string username, string password);

        #endregion

        #region UserTravel
        void AddUserTravel(UserTravel userTravel);
        IEnumerable<UserTravel> GetAllUserTravels();
        IEnumerable<UserTravel> GetAllUserTravelsBy(Predicate<UserTravel> predicate);
        UserTravel GetUserTravel(int travelId);
        void DeleteUserTravel(int travelId);

        #endregion
    }
}
