﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public void DeleteBus(int licenseNum)
        {
            var bus = DataSource.BusesList.Find(b => b.LicenseNum == licenseNum);
            if (bus is null) throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            bus.Active = false;
        }
        public void ActivateBus(int licenseNum)
        {
            var bus = DataSource.BusesList.Find(b => b.LicenseNum == licenseNum);
            if (bus is null) throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            bus.Active = true;
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

        public void ActivateBusStation(int code)
        {
            var busStation = DataSource.BusStationsList.Find(l => l.Code == code); 
            if (busStation is null) throw new StationDoesNotExistException($"Bus line number {code} does not exist");
            busStation.Active = true;
        }
        public IEnumerable<BusStation> GetAllActiveBusStations()
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException("Active Bus Station List is Empty!");
            }

            return from bs in DataSource.BusStationsList
                   where bs.Active is true
                   select bs.Clone();
        }
        public IEnumerable<BusStation> GetAllInActiveBusStations()
        {
            if (DataSource.BusStationsList.Count == 0)
            {
                throw new EmptyListException("In Active Bus Station List is Empty!");
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
        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
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

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return new List<LineStation>();
            //throw new NotImplementedException();
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

        #region Line exit

        public void AddLineExit(LineExit lineExit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineExit> GetAllLineExits()
        {
            throw new NotImplementedException();
        }

        public LineExit GetLineExit(int busLineId, TimeSpan startTime)
        {
            throw new NotImplementedException();
        }

        public LineExit GetGeneralLineExit(int busLineId)
        {
            throw new NotImplementedException();
        }


        public void DeleteLineExit(int busLineId, TimeSpan startTime)
        {
            throw new NotImplementedException();
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
        public int GetKey()
        {
            return DS.KeyGenerator.IdGenerator();
        }
    }
}