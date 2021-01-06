using System;

namespace DO
{
    public class LineStation
    {
        public int BusLineId { get; set; }
        public int StationNumber { get; set; }
        public int StationIndex { get; set; }
        public bool Active { get; set; }

        //public TimeSpan TimeToNext { get; set; }
    }
}

