using System;
using DO;

namespace BO
{
    public class LineExit
    {
        public int LineNum { get; set; }
        public DateTime StartTime { get; set; }
        public int Freq { get; set; }
        public DateTime EndTime { get; set; }
        public bool Active { get; set; } //
    }
}
