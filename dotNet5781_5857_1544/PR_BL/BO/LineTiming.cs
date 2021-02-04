using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// a special class in order to implement the rides activator in the simulator.
    /// this class represents an bus line ariiving to a station + the time to arrival.
    /// the key is busline id and start time - only one bus like this exists and can arrive.
    /// </summary>
    public class LineTiming
    {
        public int BusLineId { get; set; }
        public int StatCode { get; set; }
        public int LineNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public string LastStationName { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}