using System.Collections.Generic;

namespace BO
{
    public class BusLine
    {
        public int BusLineId { get; set; }
        public int LineNumber { get; set; }
        public Area BusArea { get; set; }
        public int FirstStation { get; set; }    
        public int LastStation { get; set; }     
        public bool AllAccessible { get; set; }
        public bool Active { get; set; }
        public IEnumerable<LineStation> ListOfLineStations { get; set; }
    }
}
