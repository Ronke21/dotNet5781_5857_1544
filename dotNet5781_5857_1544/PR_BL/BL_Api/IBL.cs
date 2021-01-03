using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    // ReSharper disable once InconsistentNaming
    public interface IBL
    {
        #region Bus
        void AddBus(Bus bus);                                       // C
        IEnumerable<Bus> GetAllBuses();                             // R
        IEnumerable<Bus> GetAllInActiveBuses();                             // R
        //IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);   // R
        Bus GetBus(int licenseNum);                                 // R
        void UpdateBus(Bus bus);                                    // U
        //void UpdateBus(int licenseNum, Action<Bus> update);       // U
        void DeleteBus(int licenseNum);                             // D

        #endregion

        #region BusLine
        void AddBusLine(BusLine busLine);                                      // C
        IEnumerable<BusLine> GetAllActiveBusLines();                           // R
        IEnumerable<BusLine> GetAllInActiveBusLines();                         // R
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);   // R
        BusLine GetBusLine(int busLineId);                                     // R
        void UpdateBusLine(BusLine busLine);                                   // U
        //void UpdateBusLine(int busLineId, Action<BusLine> update);           // U
        void DeleteBusLine(int busLineId);                                     // D

        #endregion

        #region BusStation
        void AddStation(BO.BusStation bs);
        IEnumerable<BO.BusStation> GetAllBusStations();
        IEnumerable<BO.BusStation> GetAllInActiveBusStations();
        BO.BusStation GetStation(int code);
        void UpdateBusStation(BO.BusStation bs);
        void DeleteBusStation(int code);
        #endregion



        //#region ConsecutiveStations
        ////IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        //void AddConsecutiveStations(int statCode1, int statCode2);
        //ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2);
        //void DeleteConsecutiveStations(int statCode1, int statCode2);

        //#endregion

        //#region Driver

        //void AddDriver(Driver driver);
        //IEnumerable<Driver> GetAllDrivers();
        //IEnumerable<Driver> GetAllDriversBy(Predicate<Driver> predicate);
        //Driver GetDriver(int id);
        //void UpdateDriver(Driver driver);
        ////void UpdateDriver(int id, Action<Driver> update);
        //void DeleteDriver(int id);

        //#endregion

        //#region LineStation
        //void AddLineStation(LineStation lineStation);
        //IEnumerable<LineStation> GetAlLineStations();
        //IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate);
        //LineStation GetLineStation(int lineNumber, int stationNumber);
        //void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex);
        //void DeleteLineStation(int lineNumber, int stationNumber);

        //#endregion

        //#region TravelingBus
        //void AddTravelingBus(TravelingBus travelingBus);
        //IEnumerable<TravelingBus> GetAllTravelingBuses();
        //IEnumerable<TravelingBus> GetAllTravelingBusesBy(Predicate<TravelingBus> predicate);
        //TravelingBus GetTravelingBus(int travelId);
        ////void Update(TravelingBus travelingBus);
        //void DeleteTravelingBus(int travelId);

        //#endregion

        //#region User
        //void AddUser(User user);
        //IEnumerable<User> GetAllUsers();
        //IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        //User GetUser(string username);
        //void UpdateUser(User user);
        //void UpdateUsername(string username);
        //void UpdatePassword(string username, string password);
        //void DeleteUser(string username);

        //#endregion

        //#region UserTravel
        //void AddUserTravel(UserTravel userTravel);
        //IEnumerable<UserTravel> GetAllUserTravels();
        //IEnumerable<UserTravel> GetAllUserTravelsBy(Predicate<UserTravel> predicate);
        //UserTravel GetUserTravel(int travelId);
        //void DeleteUserTravel(int travelId);

        //#endregion

    }
}
