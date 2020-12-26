using System.Collections.Generic;
using System.Device.Location;
using DO;

namespace BO
{
    public class BusStation
    {
        public int Code { get; set; }
        public GeoCoordinate Location { get; set; } //
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Accessible { get; set; }
        public bool Active { get; set; } //
        public IEnumerable<BusLine> BusLines { get; set; }
    }
}
