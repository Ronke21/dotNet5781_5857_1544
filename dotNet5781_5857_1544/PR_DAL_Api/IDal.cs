using System;
using System.Collections.Generic;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Bus

        /// <summary>
        /// adds a new bus to the system, if not exist already a bus with this license num.
        /// if not active - activates it!
        /// </summary>
        /// <param name="bus">bus to be added</param>
        void AddBus(Bus bus);

        /// <summary>
        /// get all active buses in the system.
        /// </summary>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<Bus> GetAllActiveBuses();

        /// <summary>
        /// get all in active buses in the system.
        /// </summary>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<Bus> GetAllInActiveBuses();
        
        /// <summary>
        /// gets a single bus object - according to the key license number
        /// </summary>
        /// <param name="licenseNum">the uniqe number of the bus</param>
        /// <returns>the bus</returns>
        Bus GetBus(int licenseNum);     
        
        /// <summary>
        /// updates a bus details - recives an updated show and replaces with the existing one (according to license number).
        /// </summary>
        /// <param name="bus">the updated bus</param>
        void UpdateBus(Bus bus);                                   

        /// <summary>
        /// delets a bus - turns its active property to false.
        /// </summary>
        /// <param name="licenseNum">to be deleted bus license number</param>
        void DeleteBus(int licenseNum);

        /// <summary>
        /// activates a deletes bus - return it to be used!
        /// </summary>
        /// <param name="licenseNum">bus to be updated license number.</param>
        void ActivateBus(int licenseNum);

        #endregion

        #region BusStation

        /// <summary>
        /// adds a new bus station to the system.
        /// </summary>
        /// <param name="busStation">the instance to be added</param>
        void AddBusStation(BusStation busStation);      
        
        /// <summary>
        /// turns an existing deleted station to be active - can be used!
        /// </summary>
        /// <param name="code">the station key to activate</param>
        void ActivateBusStation(int code);

        /// <summary>
        /// return all active bus stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BusStation> GetAllActiveBusStations();

        /// <summary>
        /// return all in active bus stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BusStation> GetAllInActiveBusStations();     
        
        /// <summary>
        /// returns an instance of existing active station
        /// </summary>
        /// <param name="code">key for wanted station</param>
        /// <returns>the full show of station</returns>
        BusStation GetBusStation(int code);

        /// <summary>
        /// updates an existing station - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="bus">the updated station show</param>
        void UpdateBusStation(BusStation bus);

        /// <summary>
        /// deletes a bus line - turns to in active! (still in system).
        /// </summary>
        /// <param name="code">key of station</param>
        void DeleteBusStation(int code);

        #endregion

        #region BusLine

        /// <summary>
        /// adds a new bus line to the system, recives a full instance of a line.
        /// </summary>
        /// <param name="busLine">line to be added</param>
        void AddBusLine(BusLine busLine);    
        
        /// <summary>
        /// return all active bus line in the system.
        /// </summary>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BusLine> GetAllActiveBusLines();

        /// <summary>
        /// return all in active bus line in the system.
        /// </summary>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BusLine> GetAllInActiveBusLines();

        /// <summary>
        /// return all active bus line in the system that fit a condition.
        /// </summary>
        /// <param name="predicate">the condition for lines</param>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);  
        
        /// <summary>
        /// returns a specific line according to key - bus line ID.
        /// </summary>
        /// <param name="busLineId">key for wanted line</param>
        /// <returns>a bus line show</returns>
        BusLine GetBusLine(int busLineId);   
        
        /// <summary>
        /// activates a deleted bus line - back to work!
        /// </summary>
        /// <param name="busLineId">line key to return</param>
        void ActivateBusLine(int busLineId);

        /// <summary>
        /// updates an existing busline - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="update">the updates line</param>
        void UpdateBusLine(BusLine update);

        /// <summary>
        /// deletes a bus line - turns to in active! (still in system).
        /// </summary>
        /// <param name="busLineId">line key to delete</param>
        void DeleteBusLine(int busLineId);                                     

        #endregion

        #region ConsecutiveStations

        /// <summary>
        /// return all active consecutive stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();

        /// <summary>
        /// adds a new set of connsecutive stations, recives all details.
        /// </summary>
        /// <param name="statCode1">first station code</param>
        /// <param name="statCode2">second station code</param>
        /// <param name="toNext">time between</param>
        /// <param name="distance">distance between</param>
        void AddConsecutiveStations(int statCode1, int statCode2, TimeSpan toNext, double distance);

        /// <summary>
        /// checks if already exist a show of 2 consecutive station
        /// </summary>
        /// <param name="statCode1">first station</param>
        /// <param name="statCode2">second station</param>
        /// <returns>true if exists, else false</returns>
        bool CheckConsecutiveStationsNotExist(int statCode1, int statCode2);

        /// <summary>
        /// returns a single show of consecutive stations.
        /// </summary>
        /// <param name="statCode1">first station</param>
        /// <param name="statCode2">second station</param>
        /// <returns>the show of the consecutive stations, if exist</returns>
        ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2);


        /// <summary>
        /// updates an existing station - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="conStat">the updated station show</param>
        void UpdateConsecutiveStations(ConsecutiveStations conStat);

        #endregion

        #region LineStation

        /// <summary>
        /// adds a new line station to the system.
        /// </summary>
        /// <param name="lineStation">the instance to be added</param>
        void AddLineStation(LineStation lineStation);

        /// <summary>
        /// return all active line stations in the system in a bus line.
        /// </summary>
        /// <param name="ID">the bus line ID</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<LineStation> GetAllLineStationsByLineID(int ID);

        /// <summary>
        /// return all active line stations in the system and fit a condition.
        /// </summary>
        /// <param name="predicate">the condition</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate);

        /// <summary>
        /// return all active line stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<LineStation> GetAllLineStations();

        /// <summary>
        /// returns an instance of existing active station
        /// </summary>
        /// <param name="lineNumber">line number of the station</param>
        /// <param name="stationNumber">station code</param>
        /// <returns>show of the line station</returns>
        LineStation GetLineStation(int lineNumber, int stationNumber);

        /// <summary>
        /// updates an existing station - its index (inly thing can be updates).
        /// </summary>
        /// <param name="lineNumber">line number of the station</param>
        /// <param name="stationNumber">station code</param>
        /// <param name="stationIndex">new index in line</param>
        void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex);

        /// <summary>
        /// deletes a bus line - turns to in active! (still in system).
        /// </summary>
        /// <param name="lineNumber">line number of the station</param>
        /// <param name="stationNumber">station code</param>
        void DeleteLineStation(int lineNumber, int stationNumber);

        #endregion

        #region Line exit

        /// <summary>
        /// adds a new line exit to system
        /// </summary>
        /// <param name="lineExit">to be added</param>
        void AddLineExit(LineExit lineExit);

        /// <summary>
        /// return all active line exits in the system.
        /// </summary>
        /// <returns>ienumarable of the exits</returns>
        IEnumerable<LineExit> GetAllLineExits();

        /// <summary>
        /// return a single line exit according to key - start time and line
        /// </summary>
        /// <param name="busLineId">line number</param>
        /// <param name="startTime">start time = first ride</param>
        /// <returns></returns>
        LineExit GetLineExit(int busLineId, TimeSpan startTime);

        /// <summary>
        /// return a single line exit for a line - first one if there are many.
        /// </summary>
        /// <param name="busLineId">line number</param>
        /// <returns></returns>
        LineExit GetGeneralLineExit(int busLineId);

        /// <summary>
        /// delets a line exit - turns to in active.
        /// </summary>
        /// <param name="busLineId">line ID</param>
        /// <param name="startTime">exit first time</param>
        void DeleteLineExit(int busLineId, TimeSpan startTime);

        #endregion

        /// <summary>
        /// a func the returns a uniqe key from the key generator - for classes that need a key.
        /// </summary>
        /// <returns>a number to be the key</returns>
        int GetKey();
    }
}
