using System;

namespace DO
{
    /// <summary>
    /// This class represents a physical bus.
    /// primary key - license num.
    /// license num is according to start date - the day the bus got on road:
    /// buses befor 2018 is 7 digits, after 2018 is 8 digits.
    /// fuel is between 1200-0.
    ///in every maintenance - the current milage is updated.
    ///contains status - ready to ride, needs refuel/maintaining etc.
    /// </summary>
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime StartTime { get; set; }
        public double Fuel { get; set; }
        public double Mileage { get; set; }
        public DateTime LastMaint { get; set; }
        public double MileageFromLast { get; set; }
        public Status Stat { get; set; }
        public bool Active { get; set; }
    }
}
