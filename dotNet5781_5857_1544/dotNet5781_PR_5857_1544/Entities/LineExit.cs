using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class LineExit
    {      
        #region LineNum

        private readonly int lineNum;

        public int LineNum
        {
            get { return lineNum; }
        }

        #endregion

        #region StartTime

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
        }

        #endregion

        #region Freq

        private int freq;

        public int Freq
        {
            get { return freq; }
        }

        #endregion

        #region EndTime

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
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

        public LineExit(int num, DateTime start, int frequency, DateTime end)
        {
            this.lineNum = num;
            this.startTime = start;
            this.freq = frequency;
            this.endTime = end;
            this.active = true;
        }

        #endregion
    }
}
