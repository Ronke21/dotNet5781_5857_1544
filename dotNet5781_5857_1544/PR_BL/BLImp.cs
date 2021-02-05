using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region Bus

        private BO.Bus BusDoToBoAdapter(DO.Bus bus)
        {
            var boBus = new BO.Bus();
            bus.CopyPropertiesTo(boBus);
            return boBus;
        }

        /// <summary>
        /// check validity of a bus instance before added / updated.
        /// if not valid throws exception.
        /// license num - length fit to starting date
        /// fuel and mileage - not negative and not too big
        /// all dates - logical
        /// </summary>
        /// <param name="bus">bus to check</param>
        /// <returns>tru if bus is valid</returns>
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

        /// <summary>
        /// check validity of a bus station instance before added / updated.
        /// if not valid throws exception.
        /// check location in state of israel.
        /// </summary>
        /// <param name="bs">station to check</param>
        /// <returns>true if station is valid</returns>
        private bool BusStationIsFit(BO.BusStation bs) //boolean id true for update, false for add. checks if already exist
        {
            // if out of Israel
            if (bs.Location.Longitude < 31 ||
                bs.Location.Longitude > 33.3 ||
                bs.Location.Latitude < 34.3 ||
                bs.Location.Latitude > 35.5)
            {
                throw new BO.NotInIsraelException("Stations can be only in Israel!");
            }

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
        }
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
        #endregion

        #region Bus lines
        public IEnumerable<BO.BusStation> GetAllMatchingBusStations(string text, IEnumerable<BO.BusStation> collection)
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
                var busLineDo = _dal.GetBusLine(busLineId);
                busLineDo.CopyPropertiesTo(busLineBO);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus line id does not exist", ex);
            }
            catch (BusLineDoesNotExistsException)
            {
                return null;
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

            if (chosen.Count() < 2) //bus must have at least 2 stations
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

        /// <summary>
        /// help method - compares 2 buslines - details and stations
        /// </summary>
        /// <param name="b1">first bus line</param>
        /// <param name="b2">second bus line</param>
        /// <param name="bs1">first station list</param>
        /// <param name="bs2">second station list</param>
        /// <returns>true if equal</returns>
        private bool CompareLines(BO.BusLine b1, BO.BusLine b2, IEnumerable<BO.BusStation> bs1, IEnumerable<BO.BusStation> bs2)
        {
            var eq = b1.Active == b2.Active &&
                     b1.BusArea == b2.BusArea &&
                     b1.LineNumber == b2.LineNumber &&
                     bs1.Count() == bs2.Count();

            if (!eq) return false;

            var s1 = bs1.ToList();
            var s2 = bs2.ToList();

            return !s1.Where((t, i) => t.Code != s2[i].Code).Any(); //if 1 of stations is different - return false

        }
        public IEnumerable<string> groupLineByAreas()
        {
            var linesGroupedByArea = (GetAllActiveBusLines()).GroupBy(busLine => busLine.BusArea);

            //count the lines grouped by area and create ienumarable af strings with the results
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

        /// <summary>
        /// calculate distance factor - that effect air distance between 2 consecutive stations
        /// </summary>
        private double DistanceFactor()
        {
            return 1.25 + Rand.NextDouble() * 0.88;
        }

        /// <summary>
        /// calculate speed factor - that effect time between 2 consecutive stations
        /// </summary>
        private double SpeedFactor(double dist)
        {
            // interurban speed
            if (dist <= 1000)
            {
                return Rand.Next(30, 50) / 3.6; //30-50 kmh
            }

            return Rand.Next(50, 80) / 3.6; //50-80 kmh
        }

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

                var distance = (stat1.Location.GetDistanceTo(stat2.Location)) * DistanceFactor(); //the air distance is not rellevant to ride so use distance factor

                var time = new TimeSpan(0, 0, (int)(distance / SpeedFactor(distance))); //time between stations according to random speed

                _dal.AddConsecutiveStations(statCode1, statCode2, time, Math.Round(distance / 1000, 2));
            }
            catch (Exception ex)
            {
                throw new BO.BadAdditionException("Unknown Error!");
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
            catch (Exception ex)
            {
                throw new BO.BadUpdateException("Unknown Error!");
            }
        }
        public IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStations()
        {
            try
            {
                foreach (var bl in _dal.GetAllActiveBusLines())
                {
                    UpdateAndReturnLineStationList(bl.BusLineId); //make shore all times are calculated and updated
                }
            }
            catch (Exception ex)
            {
                throw new BO.GeneralErrorException("Unknown Error!");
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
            catch (Exception ex)
            {
                throw new BO.GeneralErrorException("Unknown Error!");
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
            catch (Exception ex)
            {
                throw new BO.GeneralErrorException("Unknown Error!");
            }
        }

        public IEnumerable<BO.ConsecutiveStations> GetAllConsecutiveStationsByCode(string code)
        {
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
                throw new BO.GeneralErrorException("Unknown error Get all consecutive stations");
            }
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

            for (var i = 0; i < lineStations.Count() - 1; i++) //update consecutive stations time and distance
            {
                AddConsecutiveStations(lineStations[i].Code, lineStations[i + 1].Code);
            }

            var toReturn = lineStations.Select(ls => LineStationDoToBoAdapter(ls)).ToList();

            foreach (var stat in toReturn)
            {
                var copyFrom = _dal.GetBusStation(stat.Code);
                copyFrom.CopyPropertiesTo(stat);
            }

            for (var i = 0; i < toReturn.Count() - 1; i++)
            {
                var current = _dal.GetConsecutiveStations(toReturn[i].Code, toReturn[i + 1].Code);
                toReturn[i].TimeToNext = current.AverageTravelTime;
            }

            return toReturn;
        }
        public void AddLineStation(BO.LineStation lineStation)
        {
            var lineStat = new DO.LineStation();

            lineStation.CopyPropertiesTo(lineStat);

            try
            {
                _dal.AddLineStation(lineStat);
            }

            catch (Exception ex)
            {
                throw new BO.GeneralErrorException("Unknown error AddBusLine");
            }
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
            catch (Exception)
            {
                throw new BO.BadAdditionException("Unknoen error adding line exit!");
            }
        }
        public IEnumerable<BO.LineExit> GetAllLineExits()
        {
            try
            {
                var exits = _dal.GetAllLineExits();

                IEnumerable<BO.LineExit> toReturn = new List<BO.LineExit>();

                //recives the flat details from dal and fills the times list of each instance
                return exits.Aggregate(toReturn, (current, x) => current.
                    Append(GetLineExit(x.BusLineId, x.StartTime)));

                /*equal to:
                foreach(var x in exits)
{
                    toReturn = toReturn.Append(getLineExit(x.BusLineId, x.StartTime));
                }

                return toReturn;
                */
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No lineExits in the list", ex);
            }

            catch (Exception)
            {
                throw new GeneralErrorException("Unknown error getAllLineExits");
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
                throw new GeneralErrorException("Unknown error getLineExit");
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
                throw new BO.GeneralErrorException("Unknown error getLineExit");
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
            Clock.Instance.ClockObserver += updateTime; //sign to event - the action that updates the presentation layer clock
            var timer = Clock.Instance.Timer;

            Simulator.Instance.StatCode = -1;

            StartLines();

            while (Clock.Instance.IsRunning) //update PL every 0.01 seconds
            {
                Clock.Instance.Time = startTime + TimeSpan.FromTicks(timer.Elapsed.Ticks * rate);
                Clock.Instance.Time = Clock.Instance.Time.TimeOnly(); //in case 1 day had passed - ignore the days
                Thread.Sleep(10);
            }
        }
        public void StopSimulator()
        {
            Clock.Instance.IsRunning = false;
        }
        public bool IsSimulatorRunning() 
        { 
            return Clock.Instance.IsRunning; 
        }
        public void SetStationPanel(int statCode, Action<LineTiming> updateBus)
        {
            Simulator.Instance.StatCode = statCode;

            Simulator.Instance.SetDigitalPanelObserver += updateBus; //sign to event - the action that updates the presentation digital panel
        }
        public void StopStationPanel(Action<LineTiming> updateBus)
        {
            Simulator.Instance.StatCode = -1;

            Simulator.Instance.SetDigitalPanelObserver -= updateBus;
        }

        private void StartLines()
        {
            var allConStats = GetAllConsecutiveStations().ToList(); //get all the details before - to be done only once for all lines

            // SendLines:
            foreach (var lineExit in GetAllLineExits().OrderBy(le => le.StartTime))
            {
                //get all the details before - to be done only once for all line exits
                var currentLine = GetBusLine(lineExit.BusLineId);
                var lastStationName = GetBusStation(currentLine.LastStation).Name;

                //for each line exit - opens a new thread that activates the line exit. waits for a time that a line needs to go and sends it in a new thread
                new Thread(() =>
                {
                    while (Clock.Instance.IsRunning)
                    {
                        var nextExitTime = (from time in lineExit.Times
                                            where time > Clock.Instance.Time.TimeOnly()
                                            select time).FirstOrDefault();

                        if (nextExitTime == TimeSpan.Zero) //default - so wait for first exit
                        {
                            nextExitTime = lineExit.StartTime;
                        }

                        // wait for nearest exit time
                        var t = nextExitTime - Clock.Instance.Time.TimeOnly();
                        if (t.Hours < 0)//after a day passed it turnes to negative so add 24 hours
                        {
                            t = new TimeSpan(t.Hours + 24, t.Minutes, t.Seconds);
                        }

                        var wait = t.TotalMilliseconds / Clock.Instance.Rate;
                        Thread.Sleep((int)wait); //sleep until it is time to start ride!

                        // start line exit - every time in the line exit get a different thread
                        new Thread(() =>
                        {
                            var stations = currentLine.ListOfLineStations.ToList();

                            for (var i = 0; i < stations.Count - 1; i++) //go through every stations. in each station check the time to all other stations
                            {
                                Simulator.Instance.LineTiming = new LineTiming()//first station - already arrived, send with arrival 0
                                {
                                    BusLineId = lineExit.BusLineId,
                                    StartTime = nextExitTime,
                                    LastStationName = lastStationName,
                                    LineNumber = lineExit.LineNumber,
                                    ArrivalTime = TimeSpan.Zero,
                                    StatCode = stations[i].Code,
                                };

                                //calculate for time for the rest of the stations from the current one
                                var lineTimings = ArrivalTimesToNextStations(currentLine, nextExitTime, i, allConStats, lastStationName).ToList();

                                //if one of stations is opend in PL - send the update to digital panel
                                var lineTiming = (from lt in lineTimings
                                                  where lt.StatCode == Simulator.Instance.StatCode
                                                  select lt).FirstOrDefault();

                                if (!(lineTiming is null))
                                {
                                    Simulator.Instance.LineTiming = lineTiming;
                                }

                                //sleep until the next station - according to time to it with a random factor - early or delay
                                var sleep = lineTimings[0].ArrivalTime.TotalMilliseconds / Clock.Instance.Rate;
                                sleep *= (double)Rand.Next(9, 20) / 10;
                                Thread.Sleep((int)sleep);
                            }

                            Simulator.Instance.LineTiming = new LineTiming() //update arrival for the last station
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

        /// <summary>
        /// help method - recives the current bus line that is in ride,
        /// its starting time and index of cuurent station,
        /// and calculates the arrivale time to all left stations
        /// </summary>
        /// <param name="busLine">line in ride</param>
        /// <param name="start">this ride start time</param>
        /// <param name="index">index of current station in line</param>
        /// <param name="conStats">list of all consecutive stations</param>
        /// <param name="lastStatName">this line last station name</param>
        /// <returns>ienumarable of the arrivale times</returns>
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
    }
}