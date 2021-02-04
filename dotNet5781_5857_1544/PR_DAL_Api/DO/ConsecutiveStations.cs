using System;

namespace DO
{
    /// <summary>
    /// This represents a connection between 2 stations - distance and time in a ride.
    /// primary key - 2 station codes.
    /// </summary>
    public class ConsecutiveStations
    {
        public int StatCode1 { get; set; }
        public int StatCode2 { get; set; }
        public double Distance { get; set; }
        public TimeSpan AverageTravelTime { get; set; }
        public bool Active { get; set; }
    }
}
