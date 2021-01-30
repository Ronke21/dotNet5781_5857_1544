using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
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