using System;

namespace DO
{
    /// <summary>
    /// This class represents the exits of a bus line.
    /// primary key is buslineID and start time - because each line can have different exits during a day time.
    /// the line starts at the start time and sends another line in the freqeuncy written, 
    /// until the end time.
    /// </summary>
    public class LineExit
    {
        public int BusLineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Freq { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool Active { get; set; }
    }
}
