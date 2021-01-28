using System;

namespace DO
{
    public class ConsecutiveStations
    {
        public int StatCode1 { get; set; }
        public int StatCode2 { get; set; }
        public double Distance { get; set; }

        //[XmlIgnore]
        public TimeSpan AverageTravelTime { get; set; }

        //[XmlElement("AverageTravelTime", DataType = "duration")]
        //[DefaultValue("PT10M")]
        //public string XmlTime
        //{
        //    get => XmlConvert.ToString(AverageTravelTime);
        //    set => AverageTravelTime = XmlConvert.ToTimeSpan(value);
        //}
        public bool Active { get; set; }
    }
}

//private TimeSpan averageTravelTime;

//[XmlIgnore]
//public TimeSpan AverageTravelTime
//{
//    get => averageTravelTime;
//    set => averageTravelTime = value;
//}