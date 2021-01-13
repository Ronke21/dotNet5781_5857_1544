using System;
using DO;

namespace BO
{
    public class LineStation : BusStation
    {
        public int BusLineId { get; set; }
        public int StationIndex { get; set; }

        //public bool Active { get; set; } //
        public TimeSpan TimeToNext { get; set; }
    }
}
