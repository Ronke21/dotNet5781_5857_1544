using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Bus
        void AddBus(Bus bus);                                      
        IEnumerable<Bus> GetAllActiveBuses();                      
        IEnumerable<Bus> GetAllInActiveBuses();                    
        //IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);
        
        Bus GetBus(int licenseNum);                                
        void UpdateBus(Bus bus);                                   
        //void UpdateBus(int licenseNum, Action<Bus> update);      
        void DeleteBus(int licenseNum);
        void ActivateBus(int licenseNum);

        #endregion

        #region BusLine
        void AddBusLine(BusLine busLine);                                      
        IEnumerable<BusLine> GetAllActiveBusLines();                           
        IEnumerable<BusLine> GetAllInActiveBusLines();                         
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);   
        BusLine GetBusLine(int busLineId);                                     
        void ActivateBusLine(int busLineId);
        void UpdateBusLine(BusLine update);
        void DeleteBusLine(int busLineId);                                     

        #endregion

        #region BusStation
        void AddBusStation(BusStation busStation);                                  // C
        void ActivateBusStation(int code);
        IEnumerable<BusStation> GetAllActiveBusStations();
        IEnumerable<BusStation> GetAllInActiveBusStations();                    // R
        BusStation GetBusStation(int code);                                         // R
        void UpdateBusStation(BusStation bus);                                      // U
        //void UpdateBusStation(int code, Action<BusStation> update);               // U
        void DeleteBusStation(int code);                                            // D

        #endregion

        #region ConsecutiveStations
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        void AddConsecutiveStations(int statCode1, int statCode2, TimeSpan toNext, double distance);
        bool CheckConsecutiveStationsNotExist(int statCode1, int statCode2);
        ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2);
        void UpdateConsecutiveStations(ConsecutiveStations conStat);

        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        IEnumerable<LineStation> GetAllLineStationsByLineID(int ID);
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);
        IEnumerable<LineStation> GetAllLineStations();
        LineStation GetLineStation(int lineNumber, int stationNumber);
        void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex);
        void DeleteLineStation(int lineNumber, int stationNumber);

        #endregion

        #region Line exit

        void AddLineExit(LineExit lineExit);
        IEnumerable<LineExit> GetAllLineExits();
        LineExit GetLineExit(int busLineId, TimeSpan startTime);
        LineExit GetGeneralLineExit(int busLineId);
        void UpdateLineExit(TimeSpan startTime, int freq, TimeSpan endTime);
        void ActivateLineExit(int busLineId, TimeSpan startTime);
        void DeleteLineExit(int busLineId, TimeSpan startTime);

        #endregion


        #region Comment

        //#region Driver

        //void AddDriver(Driver driver);
        //IEnumerable<Driver> GetAllDrivers();
        //IEnumerable<Driver> GetAllDriversBy(Predicate<Driver> predicate);
        //Driver GetDriver(int id);
        //void UpdateDriver(Driver driver);
        ////void UpdateDriver(int id, Action<Driver> update);
        //void DeleteDriver(int id);

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

        #endregion


        int GetKey();
    }
}
