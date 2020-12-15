using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNet5781_PR_5857_1544;

namespace DO
{
    class BusLine
    {
        #region id

        private int busLineID;

        public int BusLineID
        {
            get { return busLineID; }
        }

        #endregion

        #region lineNumber

        private int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
        }

        #endregion

        #region BusArea

        private Area busArea;

        public Area BusArea
        {
            get { return busArea; }
        }

        #endregion

        #region FirstStation

        private int firstStation;

        public int FirstStation
        {
            get { return firstStation; }
        }

        #endregion

        #region LastStation

        private int lastStation;

        public int LastStation
        {
            get { return lastStation; }
        }

        #endregion

        #region AllAccessible

        private bool allAccessible;
        public bool AllAccessible
        {
            get { return allAccessible; }
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

        public BusLine(int key, int num, Area ar, int first, int last, bool acc)
        {
            this.busLineID = key;
            this.lineNumber = num;
            this.busArea = ar;
            this.firstStation = first;
            this.lastStation = last;
            this.allAccessible = acc;
            this.active = true;
        }

        #endregion
    }
}
