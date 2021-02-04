using System;
using System.Collections.Generic;
using BO;

namespace BLApi
{
    // ReSharper disable once InconsistentNaming
    public interface IBL
    {
        #region Bus

        /// <summary>
        /// adds a new bus to the system, if not exist already a bus with this license num.
        /// if not active - activates it!
        /// addes the bus only if has logical property values
        /// </summary>
        /// <param name="bus">bus to be added</param>
        void AddBus(BO.Bus bus);

        /// <summary>
        /// get all active buses in the system.
        /// </summary>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<BO.Bus> GetAllBuses();

        /// <summary>
        /// get all active buses in the system which contains a pararmeter - used to search in the screen
        /// </summary>
        /// <param name="cod">part of the license number to be searched</param>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<BO.Bus> GetAllBusesByCode(string cod);

        /// <summary>
        /// get all in active buses in the system.
        /// </summary>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<BO.Bus> GetAllInActiveBuses();

        /// <summary>
        /// get all in active buses in the system which contains a pararmeter - used to search in the screen
        /// </summary>
        /// <param name="cod">part of the license number to be searched</param>
        /// <returns>Ienumarable of the buses</returns>
        IEnumerable<BO.Bus> GetAllInActiveBusesByCode(string cod);

        /// <summary>
        /// gets a single bus object - according to the key license number
        /// </summary>
        /// <param name="licenseNum">the uniqe number of the bus</param>
        /// <returns>the bus</returns>
        BO.Bus GetBus(int licenseNum);

        /// <summary>
        /// updates a bus details - recives an updated show and replaces with the existing one (according to license number).
        /// </summary>
        /// <param name="busBo">the updated bus</param>
        void UpdateBus(BO.Bus busBo);

        /// <summary>
        /// activates a deletes bus - return it to be used!
        /// </summary>
        /// <param name="licenseNum">bus to be updated license number.</param>
        void ActivateBus(int licenseNum);

        /// <summary>
        /// delets a bus - turns its active property to false.
        /// </summary>
        /// <param name="licenseNum">to be deleted bus license number</param>
        void DeleteBus(int licenseNum);

        #endregion

        #region BusStation

        /// <summary>
        /// adds a new bus station to the system.
        /// </summary>
        /// <param name="busStation">the instance to be added</param>
        void AddStation(BO.BusStation bs);

        /// <summary>
        /// turns an existing deleted station to be active - can be used!
        /// </summary>
        /// <param name="code">the station key to activate</param>
        void ActivateBusStation(int code);

        /// <summary>
        /// return all active bus stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.BusStation> GetAllBusStations();

        /// <summary>
        /// return all active bus stations in the system which contains a pararmeter - used to search in the screen
        /// </summary>
        /// <param name="code">string refering code or name of station</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.BusStation> GetAllBusStationsByCodeOrName(string code);
        
        /// <summary>
        /// return all in active bus stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.BusStation> GetAllInActiveBusStations();


        /// <summary>
        /// return all in active bus stations in the system which contains a pararmeter - used to search in the screen
        /// </summary>
        /// <param name="code">string refering code or name of station</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.BusStation> GetAllInActiveBusStationsByCodeOrName(string code);

        /// <summary>
        /// returns an instance of existing active station
        /// </summary>
        /// <param name="code">key for wanted station</param>
        /// <returns>the full show of station</returns>
        BO.BusStation GetBusStation(int code);

        /// <summary>
        /// return all the bus stations that a certain line goes through.
        /// </summary>
        /// <param name="BusLineID">line ID </param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.BusStation> GetLineBusStations(int BusLineID);
        
        /// <summary>
        /// returns a list of lines going through a given station, and for each line name of final station.
        /// used to fill the yellow sign on station
        /// </summary>
        /// <param name="statCode">station code</param>
        /// <returns>ienumarable of the lines and last stations</returns>
        IEnumerable<LineNumberAndFinalDestination> ListForYellowSign(int statCode);

        /// <summary>
        /// returns all lines that go through a given station
        /// </summary>
        /// <param name="statCode">station code</param>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BO.BusLine> LinesInStation(int statCode);

        /// <summary>
        /// updates an existing station - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="bus">the updated station show</param>
        void UpdateBusStation(BO.BusStation bs);

        /// <summary>
        /// deletes a bus line - turns to in active! (still in system).
        /// </summary>
        /// <param name="code">key of station</param>
        void DeleteBusStation(int code);

        /// <summary>
        /// used for search in list of station - return all stations that contain the code given in the search box text
        /// </summary>
        /// <param name="text">text containg station code</param>
        /// <param name="collection">list of stations</param>
        /// <returns>ienumarable of matching stations to text</returns>
        IEnumerable<BO.BusStation> GetAllMatchingBusStations(string text, IEnumerable<BO.BusStation> collection);

        #endregion

        #region BusLine

        /// <summary>
        /// adds a new bus line to the system, recives a full instance of a line.
        /// </summary>
        /// <param name="busLine">line to be added</param>
        void AddBusLine(BO.BusLine busLine, IEnumerable<BO.BusStation> busStations);

        /// <summary>
        /// return all active bus line in the system.
        /// </summary>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BO.BusLine> GetAllActiveBusLines();

        /// <summary>
        /// return all in active bus line in the system.
        /// </summary>
        /// <returns>ienumarable of the lines</returns>
        IEnumerable<BO.BusLine> GetAllInActiveBusLines();

        /// <summary>
        /// returns a specific line according to key - bus line ID.
        /// fills its list of stations
        /// </summary>
        /// <param name="busLineId">key for wanted line</param>
        /// <returns>a bus line show</returns>
        BO.BusLine GetBusLine(int busLineId);

        /// <summary>
        /// updates an existing busline - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="update">the updates line</param>
        void UpdateBusLine(BO.BusLine update, IEnumerable<BO.BusStation> chosen);

        /// <summary>
        /// activates a deleted bus line - back to work!
        /// </summary>
        /// <param name="busLineId">line key to return</param>
        void ActivateBusLine(int busLineId);

        /// <summary>
        /// deletes a bus line - turns to in active! (still in system).
        /// </summary>
        /// <param name="busLineId">line key to delete</param>
        void DeleteBusLine(int busLineId);

        /// <summary>
        /// function that counts how many lines in each area and return a list to be presentd - of area and count.
        /// </summary>
        /// <returns>list to be presented in main lines view</returns>
        IEnumerable<string> groupLineByAreas();

        #endregion

        #region ConsecutiveStations

        /// <summary>
        /// return all active consecutive stations in the system.
        /// vefore the return - make shore all stations has propper time according to distance.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStations();

        /// <summary>
        /// return all active consecutive stations in the system.
        /// </summary>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.ConsecutiveStations> GetAllSimpleConsecutiveStations();

        /// <summary>
        /// return all active consecutive stations in the system and contains part of the given code - used to search in screen.
        /// </summary>
        /// <param name="code">string containg code - to be searched</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStationsByCode(string code);

        /// <summary>
        /// adds a new set of connsecutive stations, recives 2 station code, calculate distance with location and then time between.
        /// </summary>
        /// <param name="statCode1">first station code</param>
        /// <param name="statCode2">second station code</param>
        void AddConsecutiveStations(int statCode1, int statCode2);

        /// <summary>
        /// updates an existing station - recievs an updates instance and replaces the old one.
        /// </summary>
        /// <param name="conStat">the updated station show</param>
        void UpdateConsecutiveStations(BO.ConsecutiveStations conStat);

        #endregion

        #region Simulator

        /// <summary>
        /// starts the simulator - activated when user pushes the play button.
        /// starts the clock and sends all lines to travel according to line exits.
        /// </summary>
        /// <param name="startTime">recived time from PL</param>
        /// <param name="rate">recived rate from PL</param>
        /// <param name="updateTime">action to write to clock event and update the clock in screen on change</param>
        void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime);

        /// <summary>
        /// stops the clock and the line traveling.
        /// </summary>
        void StopSimulator();

        /// <summary>
        /// returns true if simulator is running. else false.
        /// </summary>
        bool IsSimulatorRunning();

        /// <summary>
        /// activates the ride simulator - writes the station ide and the updating observer action to the event of the class.
        /// </summary>
        /// <param name="statCode">station code to be updated in line timing</param>
        /// <param name="updateBus">the action to write to event</param>
        void SetStationPanel(int statCode, Action<LineTiming> updateBus);

        /// <summary>
        /// stops the ride simulator.
        /// </summary>
        /// <param name="updateBus">the action of updating the PL - to delete from event</param>
        void StopStationPanel(Action<LineTiming> updateBus);

        #endregion

        #region LineStation

        /// <summary>
        /// adds a new line station to the system.
        /// </summary>
        /// <param name="lineStation">the instance to be added</param>
        void AddLineStation(BO.LineStation lineStation);

        /// <summary>
        /// return all active line stations in the system in a bus line.
        /// make shore that all consecutive stations of the line is updated.
        /// </summary>
        /// <param name="BusLineID">the bus line ID</param>
        /// <returns>ienumarable of the stations</returns>
        IEnumerable<BO.LineStation> UpdateAndReturnLineStationList(int BusLineID);


        #endregion

        #region Line exit

        /// <summary>
        /// adds a new line exit to system
        /// </summary>
        /// <param name="lineExit">to be added</param>
        void AddLineExit(BO.LineExit lineExit);


        /// <summary>
        /// return all active line exits in the system.
        /// </summary>
        /// <returns>ienumarable of the exits</returns>
        IEnumerable<BO.LineExit> GetAllLineExits();


        /// <summary>
        /// return all active line exits of a certain line
        /// </summary>
        /// <param name="bbslID">bus line ID</param>
        /// <returns>ienumarable of the exits</returns>
        IEnumerable<BO.LineExit> GetAllLineExitsByLine(int bbslID);

        /// <summary>
        /// return a single line exit according to key - start time and line
        /// </summary>
        /// <param name="busLineId">line number</param>
        /// <param name="startTime">start time = first ride</param>
        /// <returns></returns>
        BO.LineExit GetLineExit(int busLineId, TimeSpan startTime);

        /// <summary>
        /// delets a line exit - turns to in active.
        /// </summary>
        /// <param name="busLineId">line ID</param>
        /// <param name="startTime">exit first time</param>
        void DeleteLineExit(int bslID, TimeSpan start);

        #endregion

    }
}
