using System;
using System.Collections.Generic;
using DO;

namespace BO
{
    public class LineExit
    {
        public int LineNumber { get; set; }
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Freq { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool Active { get; set; }
        public IEnumerable<TimeSpan> Times { get; set; }
    }
}
