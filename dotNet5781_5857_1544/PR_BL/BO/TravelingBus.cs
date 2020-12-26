using System;
using DO;

namespace BO
{
    public class TravelingBus
    {
        public int Id { get; set; }
        public int LicenseNum { get; set; }
        public string LicenseNumStr
        {
            get
            {
                string num = LicenseNum.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
        }
        public int LineNum { get; set; }
        public DateTime FormalTime { get; set; }
        public DateTime ActualTime { get; set; }
        public int LastStationNum { get; set; }
        public DateTime LastStationTime { get; set; }
        public TimeSpan TimeToNext { get; set; }
        public int DriverId { get; set; }
        public bool Active { get; set; } //
    }
}
