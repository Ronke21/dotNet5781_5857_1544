using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BL;
using DalApi;
using BLApi;
using BO;


namespace BL
{
    class BLImp : IBL // internal
    {
        private readonly IDal dal = DalFactory.GetDal();

        #region Bus

        private static BO.Bus BusDoToBoAdapter(DO.Bus bus)
        {
            var boBus = new BO.Bus();
            bus.CopyPropertiesTo(boBus);
            return boBus;
        }

        private bool BusIsFit(Bus bus)
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

        public void AddBus(Bus bus)
        {

            var BusDO = new DO.Bus();

            bus.CopyPropertiesTo(BusDO);

            try
            {
                if (BusIsFit(bus))
                {
                    // mileage only under 250000 and over 0
                    if (bus.Mileage < 0 || bus.Mileage > 250000)
                    {
                        throw new NotValidFuelAmountException("Added bus must have 0 - 250,000 KM mileage");
                    }
                    dal.AddBus(BusDO);
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
                return from bus in dal.GetAllBuses()
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
        
        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            try
            {
                IEnumerable<BO.Bus> buses = null;
                IEnumerable<DO.Bus> b = from bus in dal.GetAllBuses()
                                        where predicate(GetBus(bus.LicenseNum))
                                        select bus;
                b.CopyPropertiesTo(buses);
                return buses;
            }

            catch (DO.EmptyListException ex)
            {
                throw new BO.EmptyListException("No buses in the list", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetAllBusesBy");
            }
        }

        public Bus GetBus(int licenseNum)
        {
            var BusBO = new BO.Bus();

            try
            {
                var BusDO = dal.GetBus(licenseNum);
                BusDO.CopyPropertiesTo(BusBO);
            }
            catch (DO.BusDoesNotExistsException ex)
            {
                throw new BO.DoesNotExistException("Bus license number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error GetBus");
            }

            return BusBO;
        }

        public void UpdateBus(Bus bus)
        {
            try
            {
                if (BusIsFit(bus))
                {
                    var b = new DO.Bus();
                    bus.CopyPropertiesTo(b);
                    dal.UpdateBus(b);
                }
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
                dal.DeleteBus(licenseNum);
            }
            catch (DO.BusDoesNotExistsException ex)
            {
                throw new BO.DoesNotExistException("Bus license number does not exist", ex);
            }
            catch (Exception)
            {
                throw new Exception("Unknown error");
            }
        }
        #endregion
    }
}
