using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class ConsecutiveStations
    {
        #region StationCode1

        private int statCode1;

        public int StatCode1
        {
            get { return statCode1; }
        }

        #endregion

        #region StationCode2

        private int statCode2;

        public int StatCode2
        {
            get { return statCode2; }
        }

        #endregion

        #region Distance

        private double distance;

        public double Distance
        {
            get { return distance; }
        }

        #endregion

        #region AverageTravelTime

        private TimeSpan averageTravelTime;

        public TimeSpan AverageTravelTime
        {
            get { return averageTravelTime; }
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

        public ConsecutiveStations(int code1, int code2, double dist, TimeSpan avg)
        {
            this.statCode1 = code1;
            this.statCode2 = code2;
            this.distance = dist;
            this.averageTravelTime = avg;
            this.active = true;
        }

        #endregion
    }
}
