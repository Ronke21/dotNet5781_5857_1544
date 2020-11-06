using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dotNet5781_02_5857_1544
{
    class BusLine : IComparable<BusLine>
    {
        private int BusLineID;
        public int BUSLINEID
        {
            get { return BusLineID; }
        }

        //public readonly bool Regular = false;

        public List<BusLineStation> Stations;

        private BusLineStation firstStation;
        public BusLineStation FIRSTSTATION
        {
            get { return firstStation; }
        }

        private BusLineStation lastStation;
        public BusLineStation LASTSTATION
        {
            get { return lastStation; }
        }

        private Area area;

        public BusLine(/*bool regular*/)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            BusLineID = r.Next(1, 999);
            if (BusLineID < 0 || BusLineID > 999)
            {
                throw new OutOfRangeException("Bus line ID must be between 1 and 999");
            }

            Stations = new List<BusLineStation>();

            int to = r.Next(5, 15);
            for (int i = 0; i < to; i++)
            {
                Stations.Add(new BusLineStation());
            }

            //Regular = regular;

            firstStation = Stations[0];
            lastStation = Stations[-1];

            area = (Area)r.Next(1, 5);
            // add exception, correlate between area and bus line id for interurban lines
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lst"></param>
        /// <param name="ar"></param>
        public BusLine(int id, List<BusLineStation> lst, Area ar)
        {
            this.BusLineID = id;
            if (BusLineCollection.Eged.Contains())
            this.area = ar;
            foreach (var item in lst)
            {
                this.Stations.Add(item);
            }

            firstStation = Stations[0];
            lastStation = Stations[-1];
        }
        public override string ToString()
        {
            string regular = "";

            foreach (var BLS in Stations)
            {
                regular += BLS + ", ";
            }

            return "Bus Line Number: " + BusLineID +
                   ", Area: " + area +
                   "\n Stations regular side:  " + regular;
        }

        /// <summary>
        /// compare bus lines by total travel time
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// 0 if equal, 1 (or higher) if other is longer, -1 (or lower) if other is shorter
        /// </returns>
        public int CompareTo(BusLine other)
        {
            return this.TimeBetween2(this.firstStation, this.lastStation)
                .CompareTo(TimeBetween2(other.firstStation, other.lastStation));
        }

        public void AddStation(int index, BusLineStation station)
        {
            // וידוא אי כפילויות בראשי
            Stations.Insert(index, station);
            if (index == 1)
                firstStation = station;
            if (index == Stations.Count)
                lastStation = station;
        }

        public void DelStation(BusLineStation stat)
        {
            // אין צורך לבדוק קיום לפני מחיקה
            Stations.Remove(stat);
            // לבדוק אם זה מתייחס כמצביע או ערך
            //if (firstStation.BUSSTATIONKEY == stat.BUSSTATIONKEY)
            firstStation = Stations[0];
            //if (lastStation.BUSSTATIONKEY == stat.BUSSTATIONKEY)
            lastStation = Stations[-1];
        }

        public bool ExistStation(BusLineStation stat)
        {
            return Stations.Contains(stat);
        }

        public double DistanceBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            double sum = 0;
            int i = 0;
            for (; Stations[i] != stat1; i++) { }
            for (int j = i; Stations[j] != stat2; ++j)
                sum += Stations[j].DISTANCEFROMLAST;
            return sum;
        }

        public TimeSpan TimeBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            TimeSpan sum = new TimeSpan(0, 0, 0);
            int i = 0;
            for (; Stations[i] != stat1; i++) ;
            for (int j = i; Stations[j] != stat2; ++j)
                sum.Add((Stations[j].INTERVAL));
            return sum;
        }

        public BusLine Route(BusLineStation s1, BusLineStation s2)
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