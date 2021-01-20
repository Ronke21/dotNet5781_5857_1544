using System;
using System.Collections.Generic;

namespace BO
{
    public class LineExit
    {
        public int LineNumber { get; set; }
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Freq { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool Active { get; set; }
        public IEnumerable<TimeSpan> Times { get; set; }
    }
}
