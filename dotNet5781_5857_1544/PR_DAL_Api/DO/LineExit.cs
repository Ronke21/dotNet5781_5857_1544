using System;

namespace DO
{
    public class LineExit
    {
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Freq { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool Active { get; set; }
    }
}
