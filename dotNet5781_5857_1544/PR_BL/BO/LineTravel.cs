using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTravel
    {
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public IEnumerable<TimeSpan> NextStationArrivalTime { get; set; }
    }
}