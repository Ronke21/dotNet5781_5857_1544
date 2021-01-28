using System;
using System.Collections.Generic;

namespace BO
{
    public class LineTravel
    {
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public IEnumerable<TimeSpan> TimeIntervals { get; set; }
    }
}