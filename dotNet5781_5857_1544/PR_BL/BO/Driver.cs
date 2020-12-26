using System;
using System.Collections.Generic;
using DO;

namespace BO
{
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public DateTime Started { get; set; }
        public DateTime Birthday { get; set; }
        public string PictureLink { get; set; }
        public bool Active { get; set; } //
        public IEnumerable<BusLine> BusLines { get; set; }
    }
}
