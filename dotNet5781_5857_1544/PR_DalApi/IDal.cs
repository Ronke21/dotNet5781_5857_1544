using System;
using System.Collections.Generic;
using DO;

namespace PR_DalApi
{

    class KeyGenerator
    {
        private static int Key = 0;
        public static int IdGenerator()
        {
            return Key++;
        }
    }


    public interface IDal
    {
        #region Bus
        void AddBus(Bus bus);                                       // C
        IEnumerable<Bus> GetAllBuses();                             // R
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);   // R
        Bus GetBus(int licenseNum);                                 // R
        void UpdateBus(Bus bus);                                    // U
        //void UpdateBus(int licenseNum, Action<Bus> update);         // U
        void DeleteBus(int licenseNum);                             // D

        #endregion

        #region BusLine
        void AddBusLine(BusLine busLine);                                      // C
        IEnumerable<BusLine> GetAllBusLines();                                 // R
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);   // R
        BusLine GetBusLine(int busLineId);                                         // R
        void UpdateBusLine(BusLine bus);                                       // U
        //void UpdateBusLine(int busLineId, Action<BusLine> update);             // U
        void DeleteBusLine(int busLineId);                                     // D

        #endregion

        #region BusStation
        void AddBusStation(BusStation busStation);                                  // C
        IEnumerable<BusStation> GetAllBusStations();                                // R
        IEnumerable<BusStation> GetAllBusLinesBy(Predicate<BusStation> predicate);  // R
        BusStation GetBusStation(int code);                                         // R
        void UpdateBusStation(BusStation bus);                                      // U
        //void UpdateBusStation(int code, Action<BusStation> update);                 // U
        void DeleteBusStation(int code);                                            // D

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
        //void UpdateDriver(int id, Action<Driver> update);
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
