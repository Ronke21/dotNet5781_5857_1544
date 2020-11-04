using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dotNet5781_02_5857_1544
{

    enum Area { General = 1, North, South, Center, Jerusalem }
    class BusLine : IComparable
    {
        private int BusLineID;
        private List<BusLineStation> Stations;
        private BusLineStation FirstStation;
        private BusLineStation LastStation;
        private Area area;

        public BusLine()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            BusLineID = r.Next(1, 999);

            Stations = new List<BusLineStation>();

            for (int i = 0; i < r.Next(5, 15); i++)
            {
                Stations.Add(new BusLineStation());
            }

            FirstStation = Stations[0];
            LastStation = Stations[-1];

            area = (Area)r.Next(1, 5);
        }

        public BusLine(int id, List<BusLineStation> lst, Area ar)
        {
            this.BusLineID = id;
            this.area = ar;
            foreach (var item in lst)
            {
                this.Stations.Add(item);
            }

            FirstStation = Stations[0];
            LastStation = Stations[-1];
        }
        public override string ToString()
        {
            string regular = "";
            string reverse = "";

            foreach (var BLS in Stations)
            {
                regular += BLS + ", ";
            }

            Stations.Reverse();

            foreach (var BLS in Stations)
            {
                reverse += BLS + ", ";
            }

            Stations.Reverse();

            return "Bus Line Number: " + BusLineID +
                   ", Area: " + area +
                   "\n Stations regular side:  " + regular +
                   "\n Stations reverse side:  " + reverse;
        }

        public int CompareTo(object other)
        {
            return this.timeBetween2(this.FirstStation, this.LastStation)
                .CompareTo(timeBetween2(((BusLine)other).FirstStation, ((BusLine)other).LastStation));
        }

        public void addStation(int index, BusLineStation station)
        {
            // וידוא אי כפילויות בראשי
            Stations.Insert(index, station);
            if (index == 1)
                FirstStation = station;
            if (index == Stations.Count)
                LastStation = station;
        }

        public void delStation(BusLineStation stat)
        {
            // אין צורך לבדוק קיום לפני מחיקה
            Stations.Remove(stat);
            // לבדוק אם זה מתייחס כמצביע או ערך
            //if (FirstStation.BUSSTATIONKEY == stat.BUSSTATIONKEY)
            FirstStation = Stations[0];
            //if (LastStation.BUSSTATIONKEY == stat.BUSSTATIONKEY)
            LastStation = Stations[-1];
        }

        public bool existStation(BusLineStation stat)
        {
            return Stations.Contains(stat);
        }

        public double distanceBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            double sum = 0;
            int i = 0;
            for (; Stations[i] != stat1; i++) { }
            for (int j = i; Stations[j] != stat2; ++j)
                sum += Stations[j].DISTANCEFROMLAST;
            return sum;
        }

        public TimeSpan timeBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            TimeSpan sum = new TimeSpan(0, 0, 0);
            int i = 0;
            for (; Stations[i] != stat1; i++) ;
            for (int j = i; Stations[j] != stat2; ++j)
                sum.Add((Stations[j].INTERVAL));
            return sum;
        }

        public BusLine route(BusLineStation s1, BusLineStation s2)
        {
            List<BusLineStation> lst = new List<BusLineStation>();
            int start = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s1.BUSSTATIONKEY);
            int end = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s2.BUSSTATIONKEY);
            for (int i = start; i < end; i++)
            {
                lst.Add(Stations[i]);
            }

            return new BusLine(this.BusLineID, lst, this.area);
        }
    }
}
