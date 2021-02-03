﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DalApi;
using BLApi;
using BO;


namespace BL
{
    internal class BLImp : IBL
    {
        #region singleton

        private static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        private BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

        private readonly IDal _dal = DalFactory.GetDal();

        private readonly Random Rand = new Random(DateTime.Now.Millisecond);

        //private IEnumerable<LineArrivalToStation> MasterList = new List<LineArrivalToStation>();

        //private Thread _fill = new Thread(() => { }); // empty
        //public bool IsFillRunning() { return _fill.IsAlive; }

        #region Bus

        private BO.Bus BusDoToBoAdapter(DO.Bus bus)
        {
            var boBus = new BO.Bus();
            bus.CopyPropertiesTo(boBus);
            return boBus;
        }
        private bool BusIsFit(BO.Bus bus)
        {
            // new buses must have 8 digit number
            if (bus.StartTime.Year > 2017 && (bus.LicenseNum < 10000000 || bus.LicenseNum > 99999999))
            {
                throw new NotValidLicenseNumberException("Buses made after 2017 must have 8 digits number");
            }

            // number 7 or 8 only
            if (bus.LicenseNum < 1000000 || bus.LicenseNum > 99999999)
            {
                throw new NotValidLicenseNumberException("Buses can only have 8 or 7 digits number");
            }

            // bus cannot be maintained before starting service
            if (bus.StartTime > bus.LastMaint)
            {
                throw new BadDatesException("Maintenance time cannot be before start time");
            }

            // start/last maint time can't be in the future
            if (bus.LastMaint > DateTime.Today || bus.StartTime > DateTime.Today)
            {
                throw new BadDatesException("Can you see the future?");
            }

            // fuel over 1200 under 0
            if (bus.Fuel < 0 || bus.Fuel > 1200)
            {
                throw new NotValidFuelAmountException("Bus must have 0 - 1200 FUEL");
            }

            // mileage only under 250000 and over 0
            if (bus.Mileage < 0 || bus.Mileage > 250000)
            {
                throw new BO.NotValidMileageException("Added bus must have 0 - 250,000 KM mileage");
            }

            // mileage only under 250000 and over 0
            if (bus.MileageFromLast > bus.Mileage)
            {
                throw new BO.NotValidMileageException("Mileage from last maintenance cannot be more then the overall mileage");
            }

            return true;
        }

        #region Set Status and other funcs
        private bool QualifiedMileage(BO.Bus bus, double ride = 0)
        {
            return bus.MileageFromLast + ride <= 20000;
        }
        private bool QualifiedDate(BO.Bus bus)
        {
            return bus.LastMaint.AddYears(1).CompareTo(DateTime.Now) > 0;
        }
        private bool QualifiedFuel(BO.Bus bus, double ride = 0)
        {
            return bus.Fuel - ride > 0;
        }
        private void SetStatus(int licenseNum, double ride = 0)
        {
            var bus = GetBus(licenseNum);

            if (!QualifiedDate(bus) || !QualifiedMileage(bus, ride) || !QualifiedFuel(bus, ride)
            ) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
            {
                bus.Stat = BO.Status.Unfit;
            }
            //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
            else if (bus.MileageFromLast + 1200 >= 20000 || bus.LastMaint < DateTime.Today.AddMonths(-11))
            {
                bus.Stat = BO.Status.MaintainSoon;
            }
            else
            {
                bus.Stat = BO.Status.Ready; //ready for ride!
            }
        }
        private void SetNewStatus(BO.Bus bus)
        {
            if (!QualifiedDate(bus) || !QualifiedMileage(bus) || !QualifiedFuel(bus)
            ) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
            {
                bus.Stat = BO.Status.Unfit;
            }
            //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
            else if (bus.MileageFromLast + 1200 >= 20000 || bus.LastMaint < DateTime.Today.AddMonths(-11))
            {
                bus.Stat = BO.Status.MaintainSoon;
            }
            else
            {
                bus.Stat = BO.Status.Ready; //ready for ride!
            }
        }

        #endregion

        public void AddBus(BO.Bus bus)
        {
            var busDo = new DO.Bus();

            SetNewStatus(bus);

            bus.CopyPropertiesTo(busDo);

            try
            {
                if (BusIsFit(bus))
                {
                    _dal.AddBus(busDo);
                }
            }
            catch (DO.BusAlreadyExistsException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (BO.NotValidLicenseNumberException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (BO.BadDatesException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (BO.NotValidFuelAmountException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (BO.NotValidMileageException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (Exception)
            {
                throw new BO.BadAdditionException("Can't add bus, unknown error");
            }
        }
        public IEnumerable<BO.Bus> GetAllBuses()
        {
            try
            {
                return from bus in _dal.GetAllActiveBuses()
                       select BusDoToBoAdapter(bus);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public IEnumerable<BO.Bus> GetAllBusesByCode(string cod)
        {
            try
            {
                return from bus in _dal.GetAllActiveBuses()
                       where bus.LicenseNum.ToString().Contains(cod)
                       select BusDoToBoAdapter(bus);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public IEnumerable<BO.Bus> GetAllInActiveBuses()
        {
            try
            {
                return from bus in _dal.GetAllInActiveBuses()
                       select BusDoToBoAdapter(bus);
            }
            catch (Exception)
            {
                throw new GeneralErrorException("Unknown error");
            }

            //catch (DO.EmptyListException ex)
            //{
            //    throw new BO.EmptyListException("No buses in the list", ex);
            //}
        }
        public IEnumerable<BO.Bus> GetAllInActiveBusesByCode(string cod)
        {
            try
            {
                return from bus in _dal.GetAllInActiveBuses()
                       where bus.LicenseNum.ToString().Contains(cod)
                       select BusDoToBoAdapter(bus);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
        }
        public BO.Bus GetBus(int licenseNum)
        {
            var busBo = new BO.Bus();

            try
            {
                var busDo = _dal.GetBus(licenseNum);
                busDo.CopyPropertiesTo(busBo);
            }
            catch (DO.BusDoesNotExistsException ex)
            {
                throw new BO.DoesNotExistException("Bus license number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBus");
            }

            return busBo;
        }
        public void ActivateBus(int licenseNum)
        {
            try
            {
                _dal.ActivateBus(licenseNum);
            }

            catch (DO.BusDoesNotExistsException ex)
            {
                throw new BO.BusDoesNotExistsException("Can't activate the bus!", ex);
            }
        }
        public void UpdateBus(BO.Bus busBo)
        {
            try
            {
                BusIsFit(busBo);
                var busDo = new DO.Bus();
                busBo.CopyPropertiesTo(busDo);
                _dal.UpdateBus(busDo);
            }
            catch (BO.NotValidLicenseNumberException ex)
            {
                throw new BO.BadUpdateException("Can't update bus", ex);
            }
            catch (BO.BadDatesException ex)
            {
                throw new BO.BadUpdateException("Can't update bus", ex);
            }
            catch (BO.NotValidFuelAmountException ex)
            {
                throw new BO.BadUpdateException("Can't update bus", ex);
            }
            catch (BO.NotValidMileageException ex)
            {
                throw new BO.BadUpdateException("Can't update bus", ex);
            }
            catch (Exception)
            {
                throw new BO.GeneralErrorException("Can't update bus, unknown error");
            }
        }
        public void DeleteBus(int licenseNum)
        {
            try
            {
                _dal.DeleteBus(licenseNum);
            }
            catch (DO.BusDoesNotExistsException ex)
            {
                throw new BO.DoesNotExistException("Can't load the bus!", ex);
            }
        }

        #endregion

        #region Bus Station
        private BO.BusStation BusStationDoToBoAdapter(DO.BusStation stat)
        {
            var boStat = new BO.BusStation();
            stat.CopyPropertiesTo(boStat);
            return boStat;
        }
        private bool BusStationIsFit(BO.BusStation bs) //boolean id true for update, false for add. checks if already exist
        {
            //code, location, address, accessible, active, BusLines

            //if (bs.Code < 0)
            //{
            //    throw new NotValidIDException("Station Code must be positive!");
            //}

            // if out of Israel
            if (bs.Location.Longitude < 31 ||
                bs.Location.Longitude > 33.3 ||
                bs.Location.Latitude < 34.3 ||
                bs.Location.Latitude > 35.5)
            {
                throw new BO.NotInIsraelException("Stations can be only in Israel!");
            }

            //if (isUpdate == false)
            //{
            //    var stations = _dal.GetAllActiveBusStations();

            //    if (!((from stat in stations
            //           where (stat.Code == bs.Code)
            //           select stat).Any()))
            //    {
            //        throw new BO.StationAlreadyExistsException("Bus station with this code already exist!");
            //    }

            //    if (!((from stat in stations
            //           where (stat.Location == bs.Location)
            //           select stat).Any()))
            //    {
            //        throw new BO.StationAlreadyExistsException("Bus station with this Location already exist!");
            //    }
            //}

            return true;
        }
        public void AddStation(BO.BusStation bs)
        {
            try
            {
                BusStationIsFit(bs);

                var busStationDo = new DO.BusStation();
                bs.CopyPropertiesTo(busStationDo);
                _dal.AddBusStation(busStationDo);
            }
            catch (DO.StationAlreadyExistsException ex)
            {
                throw new BO.BadAdditionException(ex.Message);
            }
            catch (BO.NotInIsraelException ex)
            {
                throw new BO.BadAdditionException(ex.Message);
            }
            catch (BO.BadAdditionException ex)
            {
                throw new BO.BadAdditionException(ex.Message);
            }
            catch (Exception)
            {
                throw new BO.GeneralErrorException("Unknown error");
            }
        }
        public void ActivateBusStation(int code)
        {
            try
            {
                _dal.ActivateBusStation(code);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.StationDoesNotExistException(ex.Message);
            }
        }
        public IEnumerable<BO.BusStation> GetAllBusStations()
        {
            //if (IsFillRunning()) throw new ThreadInterruptedException("still filling the list");
            try
            {
                var stationsList = from bs in _dal.GetAllActiveBusStations()
                                   select BusStationDoToBoAdapter(bs);

                var lineStations = _dal.GetAllLineStations();

                //attach lines to station
                foreach (var stat in stationsList)
                {
                    stat.BusLines = from lineStat in lineStations
                                    where lineStat.Code == stat.Code
                                    select GetSimpleBusLine(lineStat.BusLineId);
                }

                return stationsList;
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No stations in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBusStations");
            }
        }
        public IEnumerable<BO.BusStation> GetAllBusStationsByCodeOrName(string code)
        {
            try
            {
                var stationsList = from bs in _dal.GetAllActiveBusStations()
                                   where (bs.Code.ToString()).Contains(code) || (bs.Name).Contains(code)
                                   select BusStationDoToBoAdapter(bs);
                var lineStations = _dal.GetAllLineStations();

                //attach lines to station
                foreach (var stat in stationsList)
                {
                    stat.BusLines = from lineStat in lineStations
                                    where lineStat.Code == stat.Code
                                    select GetSimpleBusLine(lineStat.BusLineId);
                }

                return stationsList;
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No stations in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBusStations");
            }
        }
        public IEnumerable<BO.BusStation> GetAllInActiveBusStations()
        {
            try
            {
                return from bs in _dal.GetAllInActiveBusStations()
                       select BusStationDoToBoAdapter(bs);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No Stations in the list", ex);
            }
        }
        public IEnumerable<BO.BusStation> GetAllInActiveBusStationsByCodeOrName(string code)
        {
            try
            {
                return from bs in _dal.GetAllInActiveBusStations()
                       where (bs.Code.ToString()).Contains(code) || (bs.Name).Contains(code)
                       select BusStationDoToBoAdapter(bs);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No Stations in the list", ex);
            }
        }
        public BO.BusStation GetBusStation(int code)
        {
            var bsBO = new BO.BusStation();

            try
            {
                var bsDO = _dal.GetBusStation(code);
                bsDO.CopyPropertiesTo(bsBO);

                //attach lines to station
                var lineStations = _dal.GetAllLineStations();
                bsBO.BusLines = from lineStat in lineStations
                                where lineStat.Code == bsBO.Code
                                select GetSimpleBusLine(lineStat.BusLineId);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus Station number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBus");
            }

            return bsBO;
        }
        public IEnumerable<BO.BusLine> LinesInStation(int statCode)
        {
            var lineStations = _dal.GetAllLineStations();

            return from lineStat in lineStations
                   where lineStat.Code == statCode
                   select GetSimpleBusLine(lineStat.BusLineId);
        }
        public IEnumerable<LineNumberAndFinalDestination> ListForYellowSign(int statCode)
        {
            IEnumerable<LineNumberAndFinalDestination> toReturn = new List<LineNumberAndFinalDestination>();

            return GetBusStation(statCode).BusLines.Aggregate(toReturn, (current, line)
                => current.Append(new LineNumberAndFinalDestination()
                {
                    LineNumber = line.LineNumber,
                    FinalDestination = GetBusStation(line.LastStation).Name

                }));

            //var x = GetBusStation(statCode);

            //var b = x.BusLines;

            //foreach (var line in b)
            //{
            //    toReturn = toReturn.Append(new LineNumberAndFinalDestination()
            //    {
            //        LineNumber = line.LineNumber,
            //        FinalDestination = GetBusStation(line.LastStation).Name
            //    });
            //}
            //return toReturn;
        }
        //private void FillListOfArrivalToStations()
        //{
        //    IEnumerable<LineArrivalToStation> lats = new List<LineArrivalToStation>();

        //    var lineExits = GetAllLineExits();

        //    foreach (var lineExit in lineExits)
        //    {
        //        foreach (var time in lineExit.Times)
        //        {
        //            foreach (var station in GetBusLine(lineExit.BusLineId).ListOfLineStations)
        //            {
        //                var index = (from ls in UpdateAndReturnLineStationList(lineExit.BusLineId)
        //                             where ls.Code == station.Code
        //                             select ls.StationIndex).FirstOrDefault();

        //                var a = ReturnLineTravel(lineExit.BusLineId);
        //                var b = a.TimeIntervals.ToList();
        //                b.Add(new TimeSpan(0, 0, 0));
        //                var c = b[index];
        //                var d = time + c;

        //                lats = lats.Append(new LineArrivalToStation()
        //                {
        //                    BusLineId = lineExit.BusLineId,
        //                    StationCode = station.Code,
        //                    ArrivalTime = d
        //                    //ArrivalTime = time + ReturnLineTravel(lineExit.BusLineId).TimeIntervals.ToList()[index]
        //                });
        //            }
        //        }
        //    }

        //    MasterList = lats;
        //}
        //public IEnumerable<LineNumberAndFinalDestination> GetListForDigitalSign(int statCode)
        //{
        //    IEnumerable<LineNumberAndFinalDestination> l = new List<LineNumberAndFinalDestination>();

        //    #region comment
        //    foreach (var a in MasterList)
        //    {
        //        var timeToArrival = (a.ArrivalTime - Clock.Instance.Time);
        //        if (statCode == a.StationCode && timeToArrival.Hours == 0 && timeToArrival.Minutes < 30)
        //        {
        //            l = l.Append(new LineNumberAndFinalDestination()
        //            {
        //                LineNumber = GetBusLine(a.BusLineId).LineNumber,
        //                FinalDestination = GetBusStation(GetBusLine(a.BusLineId).LastStation).Name,
        //                TimeToArrival = timeToArrival
        //            });
        //        }
        //    }

        //    l = l.OrderBy(arr => arr.TimeToArrival);
        //    return l;

        //    #endregion

        //    //return x.Where(a => statCode == a.StationCode).
        //    //    Aggregate(l, (current, a) => current.
        //    //        Append(new LineNumberAndFinalDestination()
        //    //        {
        //    //            LineNumber = GetBusLine(a.BusLineId).LineNumber,
        //    //            FinalDestination = GetBusStation(GetBusLine(a.BusLineId).LastStation).Name,
        //    //            TimeToArrival = a.ArrivalTime - Clock.Instance.Time
        //    //        }));
        //}

        #endregion

        #region Bus lines
        public void UpdateBusStation(BO.BusStation bs)
        {
            try
            {
                BusStationIsFit(bs);
                var b = new DO.BusStation();
                bs.CopyPropertiesTo(b);
                _dal.UpdateBusStation(b);
            }
            catch (BO.NotInIsraelException ex)
            {
                throw new BO.BadUpdateException(ex.Message);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.BadUpdateException(ex.Message);
            }
            catch (Exception)
            {
                throw new BO.GeneralErrorException("Unknown error");
            }
        }
        public void DeleteBusStation(int code)
        {
            if (GetBusStation(code).BusLines != null && GetBusStation(code).BusLines.Any())
            {
                throw new BO.StationBelongsToActiveBusLine("Can't delete the station! it belongs to active lines!");
            }

            try
            {
                _dal.DeleteBusStation(code);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus station number does not exist", ex);
            }

        }
        public IEnumerable<BO.BusStation> GetAllMatches(string text, IEnumerable<BO.BusStation> collection)
        {
            return from station in collection
                   where station.ToString().Contains(text)
                   select station;
        }
        public IEnumerable<BO.BusStation> GetLineBusStations(int BusLineID)
        {
            var lineStations = _dal.GetAllLineStationsByLineID(BusLineID);


            return from station in lineStations
                   select GetBusStation(station.Code);
        }
        private BO.BusLine BusLineDoToBoAdapter(DO.BusLine busLine)
        {
            var boBusLine = new BO.BusLine();
            busLine.CopyPropertiesTo(boBusLine);
            return boBusLine;
        }
        public void AddBusLine(BO.BusLine busLine, IEnumerable<BO.BusStation> busStations)
        {
            var busLineDo = new DO.BusLine();

            busLine.CopyPropertiesTo(busLineDo);

            busLineDo.FirstStation = busStations.ToList()[0].Code;
            busLineDo.LastStation = busStations.ToList()[busStations.Count() > 1 ? busStations.Count() - 1 : 0].Code;

            busLineDo.BusLineId = _dal.GetKey();

            var maxIndex = busStations.Count();

            if (maxIndex < 2) throw new TooShortException("Bus line can't have less then 2 stations");

            try
            {
                _dal.AddBusLine(busLineDo);

                for (var i = 0; i < maxIndex; i++)
                {
                    var bs = busStations.ToList()[i];

                    var toAdd = new DO.LineStation()
                    {
                        StationIndex = i,
                        Code = bs.Code,
                        Active = true,
                        BusLineId = busLineDo.BusLineId
                    };

                    _dal.AddLineStation(toAdd);
                }
            }
            catch (DO.BusLineAlreadyExistsException ex)
            {
                throw new BO.BadAdditionException("Can't add bus line", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error AddBusLine");
            }
        }
        public IEnumerable<BO.BusLine> GetAllActiveBusLines()
        {
            try
            {
                return from busLines in _dal.GetAllActiveBusLines()
                       select BusLineDoToBoAdapter(busLines);
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public IEnumerable<BO.BusLine> GetAllInActiveBusLines()
        {
            try
            {
                return from busLines in _dal.GetAllInActiveBusLines()
                       select BusLineDoToBoAdapter(busLines);
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public BO.BusLine GetBusLine(int busLineId)
        {
            var busLineBO = new BO.BusLine();

            try
            {
                var busLineDO = _dal.GetBusLine(busLineId);
                busLineDO.CopyPropertiesTo(busLineBO);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus line id does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBusLine");
            }

            busLineBO.ListOfLineStations = UpdateAndReturnLineStationList(busLineBO.BusLineId);

            return busLineBO;
        }
        private BO.BusLine GetSimpleBusLine(int busLineId)
        {
            var busLineBO = new BO.BusLine();

            try
            {
                _dal.GetBusLine(busLineId).CopyPropertiesTo(busLineBO);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus line id does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBusLine");
            }

            return busLineBO;
        }
        public void ActivateBusLine(int busLineId)
        {
            try
            {
                _dal.ActivateBusLine(busLineId);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error UpdateBusLine");
            }
        }
        public void UpdateBusLine(BO.BusLine update, IEnumerable<BO.BusStation> chosen)
        {

            if (chosen.Count() < 2)
            {
                return;
            }

            var bline = GetBusLine(update.BusLineId);

            if (CompareLines(bline, update, GetLineBusStations(bline.BusLineId), chosen))
            {
                return;
            }

            try
            {
                //if (!BusIsFit(bus)) return;
                var bl = new DO.BusLine();
                update.CopyPropertiesTo(bl);

                bl.FirstStation = chosen.ToList()[0].Code;
                bl.LastStation = chosen.ToList()[chosen.Count() > 1 ? chosen.Count() - 1 : 0].Code;

                _dal.UpdateBusLine(bl);

                foreach (var ls in _dal.GetAllLineStationsByLineID(update.BusLineId))
                {
                    _dal.DeleteLineStation(ls.BusLineId, ls.Code);
                }

                var index = 0;

                foreach (var ls in chosen)
                {
                    _dal.AddLineStation(new DO.LineStation()
                    {
                        Code = ls.Code,
                        Active = ls.Active,
                        BusLineId = update.BusLineId,
                        StationIndex = index++
                    });
                }
            }

            catch (Exception)
            {
                throw new Exception("Unknown error");
            }
        }
        public void DeleteBusLine(int busLineId)
        {
            try
            {
                _dal.DeleteBusLine(busLineId);
            }
            catch (DO.BusLineDoesNotExistsException ex)
            {
                throw new BO.BusLineDoesNotExistsException("Bus Line number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error DeleteBusLine");
            }
        }
        private bool CompareLines(BO.BusLine b1, BO.BusLine b2, IEnumerable<BO.BusStation> bs1, IEnumerable<BO.BusStation> bs2)
        {
            var eq = b1.Active == b2.Active &&
                     b1.BusArea == b2.BusArea &&
                     b1.LineNumber == b2.LineNumber &&
                     bs1.Count() == bs2.Count();

            if (!eq) return false;

            var s1 = bs1.ToList();
            var s2 = bs2.ToList();

            for (var i = 0; i < s1.Count; i++)
            {
                if (s1[i].Code != s2[i].Code) return false;
            }

            return true;
        }

        public IEnumerable<string> groupLineByAreas()
        {
            var linesGroupedByArea = (GetAllActiveBusLines()).GroupBy(busLine => busLine.BusArea);

            var counts = linesGroupedByArea.Select(busLines => new
            {
                area = busLines.Key,
                count = busLines.Count()
            }).OrderBy(busLines => busLines.count);
            return from c in counts
                   select c.area + ": " + c.count;
        }
        #endregion

        #region Consecutive stations
        private double DistanceFactor()
        {
            return 1.25 + Rand.NextDouble() * 0.88;
        }
        private double SpeedFactor(double dist)
        {
            // interurban speed
            if (dist <= 1000)
            {
                return Rand.Next(30, 50) / 3.6;
            }

            return Rand.Next(50, 80) / 3.6;
        }

        //public double SpeedFactor(double dist)
        //{
        //    double speed = 50;

        //    var chance = Rand.Next(10000);

        //    switch (chance)
        //    {
        //        // flat tire
        //        case 1234:
        //            speed *= 0.4;
        //            break;
        //        // engine overheat
        //        case 770:
        //            speed *= 0.01;
        //            break;
        //    }

        //    return speed;
        //}
        private BO.ConsecutiveStations ConsecutiveStationDoToBoAdapter(DO.ConsecutiveStations DOConStation)
        {
            var BOConStation = new BO.ConsecutiveStations();
            DOConStation.CopyPropertiesTo(BOConStation);
            return BOConStation;
        }
        public void AddConsecutiveStations(int statCode1, int statCode2)
        {
            if (!_dal.CheckConsecutiveStationsNotExist(statCode1, statCode2)) return;

            try
            {
                var stat1 = GetBusStation(statCode1);
                var stat2 = GetBusStation(statCode2);

                var distance = (stat1.Location.GetDistanceTo(stat2.Location)) * DistanceFactor();

                var time = new TimeSpan(0, 0, (int)(distance / SpeedFactor(distance)));

                _dal.AddConsecutiveStations(statCode1, statCode2, time, Math.Round(distance / 1000, 2));
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public void UpdateConsecutiveStations(BO.ConsecutiveStations conStat)
        {
            try
            {
                var cs = new DO.ConsecutiveStations();
                conStat.CopyPropertiesTo(cs);
                _dal.UpdateConsecutiveStations(cs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStations()
        {
            try
            {
                foreach (var bl in _dal.GetAllActiveBusLines())
                {
                    UpdateAndReturnLineStationList(bl.BusLineId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            try
            {
                return from conStat in _dal.GetAllConsecutiveStations()
                       select ConsecutiveStationDoToBoAdapter(conStat);
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public IEnumerable<BO.ConsecutiveStations> GetAllSimpleConsecutiveStations()
        {
            try
            {
                return from conStat in _dal.GetAllConsecutiveStations()
                       select ConsecutiveStationDoToBoAdapter(conStat);
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }

        public IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStationsByCode(string code)
        {
            //try
            //{
            //    foreach (var bl in _dal.GetAllActiveBusLines())
            //    {
            //        UpdateAndReturnLineStationList(bl.BusLineId);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            try
            {
                return from conStat in _dal.GetAllConsecutiveStations()
                       where conStat.StatCode1.ToString().Contains(code) || conStat.StatCode2.ToString().Contains(code)
                       select ConsecutiveStationDoToBoAdapter(conStat);
            }
            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No Stations in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error Get all consecutive stations");
            }
        }
        private BO.ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2)
        {
            return (from conStats in GetAllConsecutiveStations()
                    where conStats.StatCode1 == statCode1 && conStats.StatCode2 == statCode2
                    select conStats).FirstOrDefault();
        }
        #endregion

        #region Line Station
        private BO.LineStation LineStationDoToBoAdapter(DO.LineStation DOLineStation)
        {
            var BOLineStation = new BO.LineStation();
            DOLineStation.CopyPropertiesTo(BOLineStation);
            return BOLineStation;
        }
        public IEnumerable<BO.LineStation> UpdateAndReturnLineStationList(int BusLineID)
        {
            var lineStations = _dal.GetAllLineStationsByLineID(BusLineID).ToList();

            lineStations.Sort((s1, s2) => s1.StationIndex.CompareTo(s2.StationIndex));

            for (var i = 0; i < lineStations.Count() - 1; i++)
            {
                AddConsecutiveStations(lineStations[i].Code, lineStations[i + 1].Code);
            }

            var toReturn = new List<BO.LineStation>();

            foreach (var ls in lineStations)
            {
                toReturn.Add(LineStationDoToBoAdapter(ls));
            }

            foreach (var stat in toReturn)
            {
                var a = _dal.GetBusStation(stat.Code);
                a.CopyPropertiesTo(stat);
                //  stat.Name = a.Name;
                // stat.Address = a.Address;
                //   stat.Code = a.Code;

            }

            for (var i = 0; i < toReturn.Count() - 1; i++)
            {
                var current = _dal.GetConsecutiveStations(toReturn[i].Code, toReturn[i + 1].Code);
                ((BO.LineStation)toReturn[i]).TimeToNext = (TimeSpan)current.AverageTravelTime;
            }

            return toReturn;
        }
        public void AddLineStation(BO.LineStation lineStation)
        {
            var lineStat = new DO.LineStation();

            lineStation.CopyPropertiesTo(lineStat);

            //lineStat.BusLineId = _dal.GetKey();

            try
            {
                _dal.AddLineStation(lineStat);
            }
            //catch (DO.BusLineAlreadyExistsException ex)
            //{
            //    throw new BO.BadAdditionException("Can't add bus line", ex);
            //}
            catch (Exception)
            {
                throw new Exception("Unknown error AddBusLine");
            }
        }
        private LineStation GetLineStation(int lineNumber, int stationNumber)
        {
            try
            {
                return LineStationDoToBoAdapter(_dal.GetLineStation(lineNumber, stationNumber));
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Simulator
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> updateTime)
        {
            Clock.Instance.IsRunning = true;
            Clock.Instance.Time = startTime;
            Clock.Instance.Rate = rate;

            Clock.Instance.Timer.Restart();
            Clock.Instance.ClockObserver += updateTime;
            var timer = Clock.Instance.Timer;

            Simulator.Instance.StatCode = -1;

            StartLines();

            while (Clock.Instance.IsRunning)
            {
                Clock.Instance.Time = startTime + TimeSpan.FromTicks(timer.Elapsed.Ticks * rate);
                Thread.Sleep(10);
            }
        }
        public void StopSimulator()
        {
            Clock.Instance.IsRunning = false;
        }
        public bool IsSimulatorRunning() { return Clock.Instance.IsRunning; }
        //public void UpdateStationDigitalSign(int statCode, Action<IEnumerable<LineNumberAndFinalDestination>> update)
        //{
        //    while (Clock.Instance.IsRunning)
        //    {
        //        var x = GetListForDigitalSign(statCode);
        //        update(x);
        //        Thread.Sleep(2000);
        //    }
        //}
        //public void SetStationPanel(int statCode, Action<LineTiming> updateBus)
        //{
        //    if (Simulator.Instance.StatCode == -1)
        //    {
        //        Simulator.Instance.StatCode = statCode;
        //    }
        //    else return;

        //    var exits = _dal.GetAllLineExits().OrderBy(ex => ex.StartTime);

        //    while (IsSimulatorRunning())
        //    {
        //        foreach (var line in exits)
        //        {
        //            var start = line.StartTime;

        //            while (start != line.EndTime)
        //            {
        //                if (start >= Clock.Instance.Time)
        //                {
        //                    var currentLine = GetBusLine(line.BusLineId);

        //                    var stat = (from st in currentLine.ListOfLineStations
        //                                where st.Code == statCode
        //                                select st).FirstOrDefault();

        //                    if (stat is null) { } // do nothing

        //                    else
        //                    {
        //                        new Thread(() =>
        //                        {
        //                            var index = (currentLine.ListOfLineStations).ToList().FindIndex
        //                            (st => st.BusLineId == line.BusLineId);

        //                            var conStats = GetConsecutiveStations(statCode,
        //                                (currentLine.ListOfLineStations).ToList()[index + 1].Code);

        //                            var lineTiming = new LineTiming()
        //                            {
        //                                StartTime = start,
        //                                ArrivalTime = start + conStats.AverageTravelTime,
        //                                BusLineId = currentLine.BusLineId,
        //                                LineNumber = currentLine.LineNumber,
        //                                LastStationName = GetBusStation(currentLine.LastStation).Name,
        //                            };

        //                            updateBus(lineTiming);

        //                        }).Start();

        //                    }
        //                }

        //                start += line.Freq;
        //            }
        //        }
        //    }
        //}
        public void SetStationPanel(int statCode, Action<LineTiming> updateBus)
        {
            Simulator.Instance.StatCode = statCode;

            Simulator.Instance.SetDigitalPanelObserver += updateBus;
        }
        public void StopStationPanel(Action<LineTiming> updateBus)
        {
            Simulator.Instance.StatCode = -1;

            Simulator.Instance.SetDigitalPanelObserver -= updateBus;
        }
        private void StartLines()
        {
            foreach (var lineExit in GetAllLineExits().OrderBy(le => le.StartTime))
            {
                var currentLine = GetBusLine(lineExit.BusLineId);
                var allConStats = GetAllConsecutiveStations().ToList();
                var lastStationName = GetBusStation(currentLine.LastStation).Name;

                new Thread(() =>
                {
                    while (Clock.Instance.IsRunning)
                    {
                        //if (lineExit.StartTime.TimeOnly() < Clock.Instance.Time.TimeOnly() ||
                        //    lineExit.EndTime.TimeOnly() >= Clock.Instance.Time.TimeOnly()) continue;

                        var nextExitTime = (from time in lineExit.Times
                                            where time > Clock.Instance.Time.TimeOnly()
                                            select time).FirstOrDefault();

                        // wait for nearest exit time
                        var t = nextExitTime - Clock.Instance.Time.TimeOnly();
                        var wait = t.TotalMilliseconds / Clock.Instance.Rate;
                        if (wait < 0) wait *= -1;
                        Thread.Sleep((int)wait);

                        // start line exit
                        new Thread(() =>
                        {
                            var stations = currentLine.ListOfLineStations.ToList();

                            // var lineTimings = new List<LineTiming>();

                            for (var i = 0; i < stations.Count - 1; i++)
                            {
                                Simulator.Instance.LineTiming = new LineTiming()
                                {
                                    BusLineId = lineExit.BusLineId,
                                    StartTime = nextExitTime,
                                    LastStationName = lastStationName,
                                    LineNumber = lineExit.LineNumber,
                                    ArrivalTime = TimeSpan.Zero,
                                    StatCode = stations[i].Code,
                                };

                                var lineTimings = ArrivalTimesToNextStations(currentLine, nextExitTime, i, allConStats, lastStationName).ToList();

                                var lineTiming = (from lt in lineTimings
                                                  where lt.StatCode == Simulator.Instance.StatCode
                                                  select lt).FirstOrDefault();

                                if (!(lineTiming is null))
                                {
                                    Simulator.Instance.LineTiming = lineTiming;
                                }

                                var sleep = lineTimings[0].ArrivalTime.TotalMilliseconds / Clock.Instance.Rate;

                                Thread.Sleep((int)sleep);
                            }

                            Simulator.Instance.LineTiming = new LineTiming()
                            {
                                BusLineId = lineExit.BusLineId,
                                StartTime = nextExitTime,
                                LastStationName = lastStationName,
                                LineNumber = lineExit.LineNumber,
                                ArrivalTime = TimeSpan.Zero,
                                StatCode = stations[stations.Count - 1].Code,
                            };
                        }).Start();
                    }
                }).Start();
            }
        }
        private IEnumerable<LineTiming> ArrivalTimesToNextStations(BusLine busLine, TimeSpan start, int index, IReadOnlyCollection<ConsecutiveStations> conStats, string lastStatName)
        {
            var toReturn = new List<LineTiming>();
            var interval = TimeSpan.Zero;

            for (var i = index; i < busLine.ListOfLineStations.Count() - 1; i++)
            {
                interval += (from s in conStats
                             where s.StatCode1 == busLine.ListOfLineStations.ToList()[i].Code &&
                                   s.StatCode2 == busLine.ListOfLineStations.ToList()[i + 1].Code
                             select s).FirstOrDefault().AverageTravelTime;

                toReturn.Add(new LineTiming()
                {
                    BusLineId = busLine.BusLineId,
                    StartTime = start,
                    LastStationName = lastStatName,
                    LineNumber = busLine.LineNumber,
                    ArrivalTime = interval,
                    StatCode = busLine.ListOfLineStations.ToList()[i + 1].Code,
                });

            }

            return toReturn;
        }

        #endregion

        #region Line exit
        private BO.LineExit LineExitDoToBoAdapter(DO.LineExit lineExit)
        {
            var le = new BO.LineExit();
            lineExit.CopyPropertiesTo(le);
            return le;
        }
        public void AddLineExit(BO.LineExit lineExit)
        {
            var lineExitDo = new DO.LineExit();

            lineExit.CopyPropertiesTo(lineExitDo);

            try
            {
                _dal.AddLineExit(lineExitDo);
            }
            catch (Exception e)
            {

            }
        }
        public IEnumerable<BO.LineExit> GetAllLineExits()
        {
            try
            {
                var exits = _dal.GetAllLineExits();

                IEnumerable<BO.LineExit> toReturn = new List<BO.LineExit>();

                //foreach (var x in exits)
                //{
                //    toReturn = toReturn.Append(getLineExit(x.BusLineId, x.StartTime));
                //}

                //return toReturn;
                return exits.Aggregate(toReturn, (current, x) => current.
                    Append(GetLineExit(x.BusLineId, x.StartTime)));
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No lineExits in the list", ex);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error getAllLineExits");
            }
        }

        public IEnumerable<BO.LineExit> GetAllLineExitsByLine(int bslID)
        {
            return from z in GetAllLineExits()
                   where z.BusLineId == bslID
                   select z;
        }

        public void DeleteLineExit(int busLineId, TimeSpan startTime)
        {
            try
            {

                _dal.DeleteLineExit(busLineId, startTime);
            }

            catch (DO.LineExitDoesNotExistsException ex)
            {
                throw new BO.BadUpdateException(ex.Message);
            }
        }
        public BO.LineExit GetLineExit(int busLineId, TimeSpan startTime)
        {
            try
            {
                var singleLineExit = LineExitDoToBoAdapter(_dal.GetLineExit(busLineId, startTime));

                singleLineExit.LineNumber = _dal.GetBusLine(busLineId).LineNumber;

                var statTime = singleLineExit.StartTime;

                singleLineExit.Times = new List<TimeSpan>();

                for (; singleLineExit.EndTime > statTime; statTime += singleLineExit.Freq)
                {
                    singleLineExit.Times = singleLineExit.Times.Append(statTime);
                }

                singleLineExit.Times = singleLineExit.Times.Append(singleLineExit.EndTime).Distinct();

                return singleLineExit;
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No lineExits in the list", ex);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error getLineExit");
            }
        }

        public BO.LineExit GetGeneralLineExit(int busLineId)
        {
            try
            {
                var x = LineExitDoToBoAdapter(_dal.GetGeneralLineExit(busLineId));

                x.LineNumber = _dal.GetBusLine(busLineId).LineNumber;

                var a = x.StartTime;

                x.Times = new List<TimeSpan>();

                for (var i = 0; x.EndTime > a; i++, a += x.Freq)
                {
                    x.Times = x.Times.Append(a);
                }

                x.Times = x.Times.Append(x.EndTime);

                x.Times.Distinct();

                return x;
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No lineExits in the list", ex);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error getLineExit");
            }
        }
        private LineTravel ReturnLineTravel(int busLineId)
        {
            var line = GetBusLine(busLineId);
            var lineExit = GetGeneralLineExit(busLineId);

            var lt = new LineTravel()
            {
                BusLineId = busLineId,
                StartTime = lineExit.StartTime,
                TimeIntervals = new List<TimeSpan>()
            };

            var abcd = new TimeSpan(0, 0, 0);

            for (var i = 0; i < line.ListOfLineStations.Count() - 1; i++)
            {
                var a = GetConsecutiveStations
                ((line.ListOfLineStations.ToList()[i]).Code,
                    (line.ListOfLineStations.ToList()[i + 1]).Code).AverageTravelTime;

                abcd = abcd.Add(a);

                lt.TimeIntervals = lt.TimeIntervals.Append(abcd);
            }

            return lt;
        }
        #endregion
    }
}