using System;
using System.Data.SqlTypes;

namespace DO
{
    public class Bus
    {
        #region LicenseNum
        private readonly int licenseNum;

        public int LicenseNum
        {
            get { return licenseNum; }
        }

        public string LicenseNumStr
        {
            get
            {
                string num = licenseNum.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
        }
        #endregion

        #region StartService

        private readonly DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
        }

        #endregion


        #region Mileage

        private double mileage;

        public double Mileage
        {
            get { return mileage; }
        }

        #endregion

        #region Fuel

        private double fuel;

        public double Fuel
        {
            get { return Math.Round(fuel, 1); }
        }

        #endregion

        #region Stat

        private Status stat;

        public Status Stat
        {
            get { return stat; }
        }

        #endregion



        #region CTOR
        public Bus(int number, DateTime start, double km, double gas, Status s)
        {
            this.licenseNum = number;
            this.startTime = start;
            this.mileage = km;
            this.fuel = gas;
            this.stat = s;
        }

        #endregion
    }
}
