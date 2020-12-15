using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using dotNet5781_PR_5857_1544;
using PR_DS;

namespace PR_DalApi
{

    class KeyGenerator
    {
        private static int Key = 0;
        public static int IdGenerator()
        {
            return Key++;
        }
    }


    class IDal
    {
        #region Bus

        #region Create

        public void CreateBus(int number, DateTime start, double km, double gas, Status s)
        {
            if (PR_DS.DataSource.BusesList.Any(bus => bus.LicenseNum == number))
            {
                if (PR_DS.DataSource.BusesList.Any(bus => bus.Active == false))
                {
                    throw new BusLineNotActiveException("This bus is not active");
                }

                throw new BusLineAlreadyExistsException("This bus already exist");
            }

            PR_DS.DataSource.BusesList.Add(new Bus(number, start, km, gas, s));
        }

        #endregion

        #region Request



        #endregion

        #endregion
    }
}
