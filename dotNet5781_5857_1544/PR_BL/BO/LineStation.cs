using System;
using DO;

namespace BO
{
    public class LineStation : BusStation
    {
        public int LineNumber { get; set; }
        //public int StationNumber { get; set; }
        public int StationIndex { get; set; }
        //public bool Active { get; set; } //
    }
}
