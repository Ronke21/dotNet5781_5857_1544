using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class UserTravel
    {
        #region TravelID

        private readonly int travelID;

        public int TravelID
        {
            get { return travelID; }
        }

        #endregion

        #region Username

        private string username;

        public string Username
        {
            get { return username; }
        }

        #endregion

        #region LineNum

        private int lineNum;

        public int LineNum
        {
            get { return lineNum; }
        }

        #endregion

        #region StartStation

        private int startStation;

        public int StartStation
        {
            get { return startStation; }
        }

        #endregion

        #region StartTime

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
        }

        #endregion

        #region EndStation

        private int endStation;

        public int EndStation
        {
            get { return endStation; }
        }

        #endregion

        #region EndTime

        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
        }

        #endregion

        #region Duration

        private TimeSpan duration;

        public TimeSpan Duration
        {
            get { return duration; }
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

        public UserTravel(int id, string name, int num, int start, DateTime startT, int end, DateTime endT)
        {
            this.travelID = id;
            this.username = name;
            this.lineNum = num;
            this.startStation = start;
            this.startTime = startT;
            this.endStation = end;
            this.endTime = endT;
            this.duration = endT - startT;
            this.active = true;
        }

        #endregion
    }
}
