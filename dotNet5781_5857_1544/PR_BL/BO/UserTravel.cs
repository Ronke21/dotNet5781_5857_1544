using System;

namespace BO
{
    public class UserTravel
    {
        public int TravelId { get; set; }
        public string Username { get; set; }
        public int LineNum { get; set; }
        public int StartStation { get; set; }
        public DateTime StartTime { get; set; }
        public int EndStation { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Active { get; set; } //
    }
}
