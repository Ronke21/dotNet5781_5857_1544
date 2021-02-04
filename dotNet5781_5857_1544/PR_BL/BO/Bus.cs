using System;

namespace BO
{
    /// <summary>
    /// This class represents a physical bus.
    /// primary key - license num.
    /// license num is according to start date - the day the bus got on road:
    /// buses befor 2018 is 7 digits, after 2018 is 8 digits. the correct license number to string is calculated here.
    /// fuel is between 1200-0.
    ///contains status - ready to ride, needs refuel/maintaining etc.
    ///in every maintenance - the current milage is updated.
    /// </summary>
    public class Bus
    {
        public int LicenseNum { get; set; }
        public string LicenseNumStr
        {
            get
            {
                var num = LicenseNum.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
            set { }
        }
        public DateTime StartTime { get; set; }
        public double Fuel { get; set; }
        public double Mileage { get; set; }
        public DateTime LastMaint { get; set; }
        public double MileageFromLast { get; set; }
        public Status Stat { get; set; }
        public bool Active { get; set; } //
    }
}
