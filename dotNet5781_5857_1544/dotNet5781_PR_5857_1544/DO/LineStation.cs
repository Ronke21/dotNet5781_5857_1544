using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DO
{
    class LineStation
    {
        #region LineNumber

        private readonly int lineNumber;

        public int LineNumber
        {
            get { return lineNumber; }
        }

        #endregion

        #region StationNumber

        private readonly int stationNumber;

        public int StationNumber
        {
            get { return stationNumber; }
        }

        #endregion

        #region StationIndex

        private int stationIndex;

        public int StationIndex
        {
            get { return stationIndex; }
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

        public LineStation(int line, int station, int index)
        {
            this.lineNumber = line;
            this.stationNumber = station;
            this.stationIndex = index;
            this.active = true;
        }

        #endregion
    }
}
