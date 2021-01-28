using System;

namespace BO
{
   public class LineArrivalToStation
    {
        public int BusLineId { get; set; }
        public int StationCode { get; set; }
        public TimeSpan ArrivalTime{ get; set; }
    }
}
