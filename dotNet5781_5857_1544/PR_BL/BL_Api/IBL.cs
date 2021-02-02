using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    // ReSharper disable once InconsistentNaming
    public interface IBL
    {
       // bool IsFillRunning();
        #region Bus
        void AddBus(BO.Bus bus);                                       // C
        IEnumerable<BO.Bus> GetAllBuses();                             // R
        IEnumerable<BO.Bus> GetAllBusesByCode(string cod);
        IEnumerable<BO.Bus> GetAllInActiveBuses();
        IEnumerable<BO.Bus> GetAllInActiveBusesByCode(string cod);
        //IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);   // R
        BO.Bus GetBus(int licenseNum);                                 // R
        void UpdateBus(BO.Bus busBo);                                    // U
        void ActivateBus(int licenseNum);
        //void UpdateBus(int licenseNum, Action<Bus> update);       // U
        void DeleteBus(int licenseNum);                             // D

        #endregion

        #region BusStation
        void AddStation(BO.BusStation bs);
        void ActivateBusStation(int code);
        IEnumerable<BO.BusStation> GetAllBusStations();
        IEnumerable<BO.BusStation> GetAllBusStationsByCodeOrName(string code);
        IEnumerable<BO.BusStation> GetAllInActiveBusStations();
        IEnumerable<BO.BusStation> GetAllInActiveBusStationsByCodeOrName(string code);
        BO.BusStation GetBusStation(int code);
        IEnumerable<BO.BusStation> GetLineBusStations(int BusLineID);
        IEnumerable<LineNumberAndFinalDestination> ListForYellowSign(int statCode);
        IEnumerable<LineNumberAndFinalDestination> GetListForDigitalSign(int statCode);
        IEnumerable<BO.BusLine> LinesInStation(int statCode);
        void UpdateBusStation(BO.BusStation bs);
        void DeleteBusStation(int code);
        IEnumerable<BO.BusStation> GetAllMatches(string text, IEnumerable<BO.BusStation> collection);

        #endregion

        #region BusLine
        void AddBusLine(BO.BusLine busLine, IEnumerable<BO.BusStation> busStations);
        IEnumerable<BO.BusLine> GetAllActiveBusLines();
        IEnumerable<BO.BusLine> GetAllInActiveBusLines();
        //IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate); 
        BO.BusLine GetBusLine(int busLineId);
        void UpdateBusLine(BO.BusLine update, IEnumerable<BO.BusStation> chosen);
        void ActivateBusLine(int busLineId);
        //void UpdateBusLine(BusLine busLine, IEnumerable<BO.BusStation> busStations);
        //void UpdateBusLine(int busLineId, Action<BusLine> update);
        //bool CompareLines(BusLine b1, BusLine b2, IEnumerable<BusStation> bs1, IEnumerable<BusStation> bs2);
        void DeleteBusLine(int busLineId);

        #endregion

        #region ConsecutiveStations
        IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStations();
        IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStationsByCode(string code);

        void AddConsecutiveStations(int statCode1, int statCode2);
        void UpdateConsecutiveStations(BO.ConsecutiveStations conStat);
        //ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2);
        //void DeleteConsecutiveStations(int statCode1, int statCode2);

        #endregion

        #region Simulator
        void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime);
        void StopSimulator();
        bool IsSimulatorRunning();
        void UpdateStationDigitalSign(int statCode, Action<IEnumerable<LineNumberAndFinalDestination>> update);
        void SetStationPanel(int statCode, Action<LineTiming> updateBus);
        void StopStationPanel(Action<LineTiming> updateBus);

        #endregion

        #region LineStation
        void AddLineStation(BO.LineStation lineStation);
        IEnumerable<BO.LineStation> UpdateAndReturnLineStationList(int BusLineID);
        
        //void AddLineStation(LineStation lineStation);
        //IEnumerable<LineStation> GetAlLineStations();
        //IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate);
        //LineStation GetLineStation(int lineNumber, int stationNumber);
        //void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex);
        //void DeleteLineStation(int lineNumber, int stationNumber);

        #endregion

        #region Line exit
        void AddLineExit(BO.LineExit lineExit);
        IEnumerable<BO.LineExit> GetAllLineExits();
        BO.LineExit GetLineExit(int busLineId, TimeSpan startTime);
        

        #endregion

        #region comment



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
    }
}
