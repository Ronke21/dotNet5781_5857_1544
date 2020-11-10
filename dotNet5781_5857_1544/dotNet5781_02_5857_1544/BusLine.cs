using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dotNet5781_02_5857_1544
{
    class BusLine : IComparable<BusLine>
    {
        public readonly bool reverse;

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

        private void SetArea()
        {
            if (BusLineID > 99) area = Area.General;
            else
            {
                if (Stations[0].Latitude >= 32.25) area = Area.North;
                else if (Stations[0].Latitude <= 31.5) area = Area.South;
                else if (Stations[0].Latitude >= 31.4 &&
                         Stations[0].Latitude <= 31.5 &&
                         Stations[0].Longitude >= 35.05) area = Area.Jerusalem;
                else area = Area.Jerusalem;
            }
        }


        public BusLine()
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
            lastStation = Stations[^1];

            SetArea();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lst"></param>
        /// <param name="ar"></param>
        public BusLine(int id, List<BusLineStation> lst, bool rev)
        {
            reverse = rev;

            Stations = new List<BusLineStation>();
            if (BusLineID < 0 || BusLineID > 999)
            {
                throw new OutOfRangeException("Bus line ID must be between 1 and 999");
            }
            this.BusLineID = id;

            foreach (var item in lst)
            {
                this.Stations.Add(item);
            }

            firstStation = Stations[0];
            lastStation = Stations[^1];

            SetArea();
        }
        public override string ToString()
        {
            string str = "";

            foreach (var BLS in Stations)
            {
                str += BLS + ", ";
            }

            return "Bus Line Number: " + BusLineID +
                   ", Area: " + area +
                   "\n Stations regular side:  " + str;
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
            if (other is null)
            {
                throw new FormatException("Object sent to compare function is not bus line type!");
            }
            return this.TimeBetween2(this.firstStation, this.lastStation)
                .CompareTo(TimeBetween2(other.firstStation, other.lastStation));
        }

        public void AddStation(int index, BusLineStation station)
        {
            // וידוא אי כפילויות בראשי
            Stations.Insert(index, station);
            if (Stations.Contains(station))
            {
                throw new StationAlreadyExistsException("Station already exists in this bus line");
            }
            if (index == 1)
                firstStation = station;
            if (index == Stations.Count - 1)
                lastStation = station;
        }

        public void DelStation(BusLineStation stat)
        {
            int count = Stations.Count;
            Stations.Remove(stat);
            if (Stations.Count == count)
            {
                throw new StationDoesNotExistException("Couldn't find station in the list, can't delete");
            }
            firstStation = Stations[0];
            //if (lastStation.BUSSTATIONKEY == stat.BUSSTATIONKEY)
            lastStation = Stations[^1];
        }

        public bool ExistStation(BusLineStation stat)
        {
            if (stat is null)
            {
                throw new ArgumentNullException("Not valid station");
            }
            return Stations.Contains(stat);
        }

        public double DistanceBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            if (stat1 is null || stat2 is null)
            {
                throw new ArgumentNullException("Not valid stations");
            }

            double sum = 0;
            int i = 0;
            for (; Stations[i] != stat1; i++) { }
            for (int j = i; Stations[j] != stat2; ++j)
                sum += Stations[j].DISTANCEFROMLAST;
            return sum;
        }

        public TimeSpan TimeBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            if (stat1 is null || stat2 is null)
            {
                throw new ArgumentNullException("Not valid stations");
            }

            TimeSpan sum = new TimeSpan(0, 0, 0);
            int i = 0;
            for (; Stations[i] != stat1; i++) ;
            for (int j = i; Stations[j] != stat2; ++j)
                sum.Add((Stations[j].INTERVAL));
            return sum;
        }

        public BusLine Route(BusLineStation s1, BusLineStation s2)
        {

            if (s1 is null || s2 is null)
            {
                throw new ArgumentNullException("Not valid stations");
            }

            List<BusLineStation> lst = new List<BusLineStation>();
            int start = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s1.BUSSTATIONKEY);
            int end = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s2.BUSSTATIONKEY);

            if (start == -1 || end == -1)
            {
                throw new StationDoesNotExistException("The line does not contain one or more of the requested stations");
            }

            for (int i = start; i < end; i++)
            {
                lst.Add(Stations[i]);
            }

            return new BusLine(this.BusLineID, lst, false);
        }
    }
}