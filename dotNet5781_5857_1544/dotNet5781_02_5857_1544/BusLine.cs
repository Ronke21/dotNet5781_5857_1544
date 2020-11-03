using System.Collections.Generic;

namespace dotNet5781_02_5857_1544
{
    class BusLine
    {
        private int BusLineNumber;
        private List<BusLineStation> Stations;
        private BusLineStation FirstStation;
        private BusLineStation LastStation;
        private string Area;

        public BusLine(int num)
        {

        }
        public override string ToString()
        {
            //לייצר 2 מחרוזות עם איטרטורים שמכילות כל התחנות
            return "Bus Line Number: " + BusLineNumber + ", Area: " + Area + "\n Stations regular side:  " + "\n Stations reverse side:  ";
        }

        public void addStation(BusLineStation stat, int place)
        {
            Stations.Insert(place, stat);
            if (place == 1)
                FirstStation = stat;
            if (place == Stations.Count)
                LastStation = stat;
        }

        public void delStation(BusLineStation stat)
        {
            Stations.Remove(stat);
            //לבדוק אם זה מתייחס כמצביע או ערך
            if (FirstStation == stat)
                FirstStation = Stations[0];
            if (LastStation == stat)
                LastStation = Stations[-1];
        }

        public bool existStation(BusLineStation stat)
        {
            return Stations.Exists(stat);
        }
    }

}
