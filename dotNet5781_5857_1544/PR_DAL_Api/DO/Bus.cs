using System;

namespace DO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        //public string LicenseNumStr
        //{
        //    get
        //    {
        //        var num = LicenseNum.ToString();
        //        return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
        //    }
        //    set { }
        //}
        public DateTime StartTime { get; set; }
        public double Fuel { get; set; }
        public double Mileage { get; set; }
        public DateTime LastMaint { get; set; }
        public double MileageFromLast { get; set; }
        public Status Stat { get; set; }
        public bool Active { get; set; }
    }
}
