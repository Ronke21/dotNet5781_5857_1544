using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineNumberAndFinalDestination
    {
        public int LineNumber { get; set; }
        public string FinalDestination { get; set; }
        public TimeSpan TimeToArrival { get; set; }
    }
}