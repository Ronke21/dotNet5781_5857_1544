using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_BL.BO
{
   public class LineArrivalToStation
    {
        public int BusLineId { get; set; }
        public int StationCode { get; set; }
        public TimeSpan ArrivalTime{ get; set; }
    }
}
