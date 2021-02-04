using System;

namespace BO
{
    /// <summary>
    /// represents a line in a bus line station list.
    /// connect between a bus line and a bus station.
    /// primary key - buslineID, code (of station).
    /// </summary>
    public class LineStation : BusStation
    {
        public int BusLineId { get; set; }
        public int StationIndex { get; set; }

        //public bool Active { get; set; } //
        public TimeSpan TimeToNext { get; set; }
    }
}
