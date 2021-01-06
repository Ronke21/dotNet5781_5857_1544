using System;
using System.Collections.Generic;
using System.Device;
using System.Linq;
using System.Security;
using DalApi;
using BLApi;
using BO;
using DO;
using Bus = BO.Bus;
using BusStation = BO.BusStation;
using LineStation = DO.LineStation;


namespace BL
{
    class BLImp : IBL // internal
    {
        private readonly IDal _dal = DalFactory.GetDal();

        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        #region Bus

        private static BO.Bus BusDoToBoAdapter(DO.Bus bus)
        {
            var boBus = new BO.Bus();
            bus.CopyPropertiesTo(boBus);
            return boBus;
        }

        private static bool BusIsFit(Bus bus)
        {
            // year and number
            if (bus.StartTime.Year > 2017 && (bus.LicenseNum < 10000000 || bus.LicenseNum > 99999999))
            {
                throw new NotValidLicenseNumberException("Buses made after 2017 must have 8 digits number");
            }
            // number 7 or 8 only
            if (bus.LicenseNum < 1000000 || bus.LicenseNum > 99999999)
            {
                throw new NotValidLicenseNumberException("Buses can only have 8 or 7 digits number");
            }
            // fuel over 1200 under 0
            if (bus.Fuel < 0 || bus.Fuel > 1200)
            {
                throw new NotValidFuelAmountException("Bus must have 0 - 1200");
            }

            return true;
        }

        #region Set Status and other funcs
        private static bool QualifiedMileage(Bus bus, double ride = 0)
        {
            return bus.MileageFromLast + ride <= 20000;
        }

        private static bool QualifiedDate(Bus bus)
        {
            return bus.LastMaint.AddYears(1).CompareTo(DateTime.Now) > 0;
        }

        private static bool QualifiedFuel(Bus bus, double ride = 0)
        {
            return bus.Fuel - ride > 0;
        }
        private void SetStatus(int licenseNum, double ride = 0)
        {
            var bus = GetBus(licenseNum);

            if (!QualifiedDate(bus) || !QualifiedMileage(bus, ride) || !QualifiedFuel(bus, ride)) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
            {
                bus.Stat = Status.Unfit;
            }
            //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
            else if (bus.MileageFromLast + 1200 >= 20000 || bus.LastMaint < DateTime.Today.AddMonths(-11))
            {
                bus.Stat = Status.MaintainSoon;
            }
            else
            {
                bus.Stat = Status.Ready; //ready for ride!
            }
        }

        private void SetNewStatus(Bus bus)
        {
            if (!QualifiedDate(bus) || !QualifiedMileage(bus) || !QualifiedFuel(bus)) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
            {
                bus.Stat = Status.Unfit;
            }
            //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
            else if (bus.MileageFromLast + 1200 >= 20000 || bus.LastMaint < DateTime.Today.AddMonths(-11))
            {
                bus.Stat = Status.MaintainSoon;
            }
            else
            {
                bus.Stat = Status.Ready; //ready for ride!
            }
        }

        #endregion

        public void AddBus(Bus bus)
        {
            var busDo = new DO.Bus();

            SetNewStatus(bus);

            bus.CopyPropertiesTo(busDo);

            try
            {
                if (BusIsFit(bus))
                {
                    // mileage only under 250000 and over 0
                    if (bus.Mileage < 0 || bus.Mileage > 250000)
                    {
                        throw new NotValidFuelAmountException("Added bus must have 0 - 250,000 KM mileage");
                    }
                    _dal.AddBus(busDo);
                }
            }
            catch (DO.BusAlreadyExistsException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error AddBus");
            }
        }

        public IEnumerable<Bus> GetAllBuses()
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

        public IEnumerable<Bus> GetAllInActiveBuses()
        {
            try
            {
                return from bus in _dal.GetAllInActiveBuses()
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

        //public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        //{
        //    try
        //    {
        //        IEnumerable<BO.Bus> buses = null;
        //        IEnumerable<DO.Bus> b = from bus in dal.GetAllActiveBuses()
        //                                where predicate(GetBus(bus.LicenseNum))
        //                                select bus;
        //        b.CopyPropertiesTo(buses);
        //        return buses;
        //    }

        //    catch (DO.EmptyListException ex)
        //    {
        //        throw new BO.EmptyListException("No buses in the list", ex);
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Unknown error GetAllBusesBy");
        //    }
        //}

        public Bus GetBus(int licenseNum)
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

        public void UpdateBus(Bus bus)
        {
            try
            {
                if (!BusIsFit(bus)) return;
                var b = new DO.Bus();
                bus.CopyPropertiesTo(b);
                _dal.UpdateBus(b);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error");
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
                throw new BO.DoesNotExistException("Bus license number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error DeleteBus");
            }
        }

        #endregion

        #region BusStation

        private static BO.BusStation BusStationDoToBoAdapter(DO.BusStation stat)
        {
            var boStat = new BO.BusStation();
            stat.CopyPropertiesTo(boStat);
            return boStat;
        }

        private static bool BusStationIsFit(BO.BusStation bs)
        {
            // year and number
            if (bs.Code < 0)
            {
                throw new NotValidIDException("Station Code must be positive!");
            }
            //if ((bs.Location.Latitude < 31 || bs.Location.Latitude > 33.3) || ( bs.Location.Longitude < 34.3 || bs.Location.Longitude >35.5))
            //{
            //    throw new BO.NotInIsraelException("Stations can be only in state of Israel!");
            //}
            return true;
        }
        public void AddStation(BO.BusStation bs)
        {
            var busStationDo = new DO.BusStation();

            bs.CopyPropertiesTo(busStationDo);

            try
            {
                if (BusStationIsFit(bs))
                {
                    _dal.AddBusStation(busStationDo);
                }
            }
            catch (DO.BusAlreadyExistsException ex)
            {
                throw new BO.BadAdditionException("Can't add bus", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error AddBus");
            }
        }

        public IEnumerable<BO.BusStation> GetAllBusStations()
        {
            try
            {
                return from bs in _dal.GetAllActiveBusStations()
                       select BusStationDoToBoAdapter(bs);
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
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
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBuses");
            }
        }
        public BO.BusStation GetBusStation(int code)
        {
            var bsBO = new BO.BusStation();

            try
            {
                var bsDO = _dal.GetBusStation(code);
                bsDO.CopyPropertiesTo(bsBO);
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

        public void UpdateBusStation(BO.BusStation bs)
        {
            try
            {
                if (!BusStationIsFit(bs)) return;
                var b = new DO.BusStation();
                bs.CopyPropertiesTo(b);
                _dal.UpdateBusStation(b);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error");
            }
        }

        public void DeleteBusStation(int code)
        {
            try
            {
                _dal.DeleteBusStation(code);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus station number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error");
            }
        }

        #endregion

        #region BusLines
        private static BO.BusLine BusLineDoToBoAdapter(DO.BusLine busLine)
        {
            var boBusLine = new BO.BusLine();
            busLine.CopyPropertiesTo(boBusLine);
            return boBusLine;
        }
        public void AddBusLine(BO.BusLine busLine)
        {
            var busLineDo = new DO.BusLine();

            busLine.CopyPropertiesTo(busLineDo);

            try
            {
                _dal.AddBusLine(busLineDo);
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

        //public IEnumerable<BO.BusLine> GetAllBusLinesBy(Predicate<BO.BusLine> predicate)
        //{
        //    try
        //    {
        //        return from busLines in dal.GetAllActiveBusLines()
        //               where predicate(busLines)
        //            select BusLineDoToBoAdapter(busLines);
        //    }
        //    catch (DO.EmptyListException ex)
        //    {
        //        throw new BO.EmptyListException("No buses in the list", ex);
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Unknown error GetAllBuses");
        //    }
        //}
        public BO.BusLine GetBusLine(int busLineId)
        {
            var blBO = new BO.BusLine();

            var lineStations = _dal.GetAlLineStationsByLineNumber(busLineId).ToList();

            lineStations.Sort((s1,s2)=>s1.StationIndex.CompareTo(s2.StationIndex));

            for (var i = 0; i < lineStations.Count() - 1; i++)
            {
                AddConsecutiveStations(lineStations[i].StationNumber, lineStations[i + 1].StationNumber);
            }
            
            try
            {
                var blDO = _dal.GetBusLine(busLineId);
                blDO.CopyPropertiesTo(blBO);
            }
            catch (DO.StationDoesNotExistException ex)
            {
                throw new BO.DoesNotExistException("Bus line id does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBusLine");
            }

            //StudentBO.ListOfCourses = from sic in dl.GetStudentsInCourseList(sic => sic.PersonId == id)
            //    let course = dl.GetCourse(sic.CourseId)
            //    select course.CopyToStudentCourse(sic);

            //busline.listofstations = from ls in something
            // let blabla = dl.getlinestation(ls.stationnumber)
            // select blabla.copy

            //blBO.ListOfLineStations = from ls in lineStations
            //    select ls.

            //lineStations.CopyPropertiesTo(blBO.ListOfLineStations);

            return blBO;
        }
        public void UpdateBusLine(BO.BusLine busLine)
        {
            try
            {
                var b = new DO.BusLine();
                busLine.CopyPropertiesTo(b);
                _dal.UpdateBusLine(b);
            }

            catch (Exception)
            {
                throw new Exception("Unknown error UpdateBusLine");
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


        #endregion

        #region Consecutive stations

        private static double DistanceFactor()
        {
            return 1.25 + Rand.NextDouble() * 0.88;
        }

        private static double SpeedFactor(double dist)
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

        public void AddConsecutiveStations(int statCode1, int statCode2)
        {
            if (!_dal.CheckConsecutiveStationsNotExist(statCode1, statCode2)) return;

            try
            {
                var stat1 = GetBusStation(statCode1);
                var stat2 = GetBusStation(statCode2);

                var distance = (stat1.Location.GetDistanceTo(stat2.Location)) * DistanceFactor();

                var time = new TimeSpan(0, 0, (int)(distance / SpeedFactor(distance)));

                _dal.AddConsecutiveStations(statCode1, statCode2, time, distance);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

    }

}

