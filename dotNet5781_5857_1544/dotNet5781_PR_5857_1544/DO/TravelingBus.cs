using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class TravelingBus
    {
        #region ID

        private readonly int id;

        public int ID
        {
            get { return id; }
        }

        #endregion

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

        #region LineNum

        private readonly int lineNum;

        public int LineNum
        {
            get { return lineNum; }
        }

        #endregion

        #region FormalTime

        private DateTime formalTime;

        public DateTime FormalTime
        {
            get { return formalTime; }
        }

        #endregion

        #region ActualTime

        private DateTime actualTime;

        public DateTime ActualTime
        {
            get { return actualTime; }
        }

        #endregion

        #region LastStationNum

        private int lastStationNum;

        public int LastStationNum
        {
            get { return lastStationNum; }
        }

        #endregion

        #region LastStationlTime

        private DateTime lastStationlTime;

        public DateTime LastStationlTime
        {
            get { return lastStationlTime; }
        }

        #endregion

        #region TimeToNext

        private TimeSpan timeToNext;

        public TimeSpan TimeToNext
        {
            get { return timeToNext; }
        }

        #endregion

        #region DriverID

        private int driverID;

        public int DriverID
        {
            get { return driverID; }
        }

        #endregion

        #region Active

        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        #endregion


        #region CTOR

        public TravelingBus(int id, int license, int num, DateTime ft, DateTime at, int last, DateTime lt,
            TimeSpan toNext, int dID)
        {
            this.id = id;
            this.licenseNum = license;
            this.lineNum = num;
            this.formalTime = ft;
            this.actualTime = at;
            this.lastStationNum = last;
            this.lastStationlTime = lt;
            this.active = true;
        }

        #endregion
    }
}
