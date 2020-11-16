/*
  this class represents a single bus line with its number and stations, and a variable refering the direction (regular or reverse)
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace dotNet5781_02_5857_1544
{
    class BusLine : IComparable<BusLine> //implements the interface icomparable in order to make a collection from it and use foreach
    {
        public static Random r = new Random(DateTime.Now.Millisecond);

        public readonly bool reverse; //is the bus lin direction is reverse (true) or regular (false)

        private int BusLineID;
        public int BUSLINEID //uniqe number: 3 digit between towns, 2 digits in town
        {
            get { return BusLineID; }
        }

        public List<BusLineStation> Stations; //list of stations in the line

        private BusLineStation firstStation;
        public BusLineStation FIRSTSTATION //first station in the list
        {
            get { return firstStation; }
        }

        private BusLineStation lastStation;
        public BusLineStation LASTSTATION //last station in the list
        {
            get { return lastStation; }
        }

        private Area area; //enum type - the area in Israel of the line

        /// <summary>
        /// private method - sets the area according to loaction in israel
        /// </summary>
        private void SetArea()
        {
            if (this.Stations.Count > 0)
            {
                if (BusLineID > 99) area = Area.General; //all outside town lines arr general
                else //inside town lines get a specific area - according to landmarks
                {
                    if (Stations[0].Latitude >= 32.25) area = Area.North;
                    else if (Stations[0].Latitude <= 31.5) area = Area.South;
                    else if (Stations[0].Latitude >= 31.4 &&
                             Stations[0].Latitude <= 31.5 &&
                             Stations[0].Longitude >= 35.05) area = Area.Jerusalem;
                    else area = Area.Jerusalem;
                }
            }
        }

        /// <summary>
        /// this ctor sets a random busline number of 3 digits, and sets a random number of random stations
        /// </summary>
        public BusLine()
        {
            BusLineID = r.Next(1, 999);

            Stations = new List<BusLineStation>();

            int to = r.Next(5, 15);
            for (int i = 0; i < to; i++)
            {
                Stations.Add(new BusLineStation());
            }

            firstStation = Stations[0];
            lastStation = Stations[^1];

            SetArea();
        }

        /// <summary>
        /// this ctor sets a bus line cy a user input of line number, list of stations and regular/reverse
        /// </summary>
        /// <param name="id">bus line number</param>
        /// <param name="lst">list of buslinestations to be added to bus line</param>
        /// <param name="rev"> bool param - false if regular line, true if reverse line </param>
        public BusLine(int id, List<BusLineStation> lst, bool rev)
        {
            reverse = rev;
            Stations = new List<BusLineStation>();

            if (BusLineID < 0 || BusLineID > 999) //3 digit line number
            {
                throw new OutOfRangeException("Bus line ID must be between 1 and 999");
            }

            this.BusLineID = id;

            foreach (var item in lst) // copy all given stations. copying the list only do a reference
            {
                this.Stations.Add(item);
            }

            if (lst.Count > 0) //not empty - there is stations to be referd
            {
                firstStation = Stations[0];
                lastStation = Stations[^1];
            }

            SetArea(); //choose an area for bus
        }

        /// <summary>
        /// implementing toString in order to print a bus line detailes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";

            foreach (var BLS in Stations)
            {
                str += "\n" + BLS + ", ";
            }

            string rev = reverse ? " reverse " : " regular ";
            // Enum.GetName(typeof(Area), area)
            return "Bus Line Number: " + BusLineID +
                   ", Area: " + area +
                   "\n Stations" + rev +  "side:  " + str;
        }

        /// <summary>
        /// compare bus lines by total travel time
        /// </summary>
        /// <param name="other"> bus line object to be compared </param>
        /// <returns>
        /// 0 if equal, 1 (or higher) if other is longer, -1 (or lower) if other is shorter
        /// </returns>
        public int CompareTo(BusLine other) //used to sorting
        {
            if (other is null)
            {
                throw new FormatException("Object sent to compare function is not bus line type!");
            }
            return this.TimeBetween2(this.firstStation, this.lastStation)
                .CompareTo(TimeBetween2(other.firstStation, other.lastStation));
        }

        /// <summary>
        /// adds a new station to bus
        /// </summary>
        /// <param name="index">index in station list to be added</param>
        /// <param name="station">station object to be added</param>
        public void AddStation(int index, BusLineStation station)
        {
            if (Stations.Contains(station)) // prevent duplicates

            {
                throw new StationAlreadyExistsException("Station already exists in this bus line");
            }
            if (index > Stations.Count) //prevent wring index
            {
                throw new IndexOutOfRangeException("Index is bigger than the station lost size");
            }
            Stations.Insert(index, station);

            //update first/last station if changed
            firstStation = Stations[0];
            lastStation = Stations[^1];
            SetArea(); //update area
        }

        /// <summary>
        /// delets a station from list
        /// </summary>
        /// <param name="stat">station to be deleted</param>
        public void DelStation(BusLineStation stat)
        {
            int count = Stations.Count;
            Stations.Remove(stat);
            if (Stations.Count == count)
            {
                throw new StationDoesNotExistException("Couldn't find station in the list, can't delete");
            }

            //update first/last station if changed
            firstStation = Stations[0];
            lastStation = Stations[^1];
        }

        /// <summary>
        /// checks if a station exist in this busline
        /// </summary>
        /// <param name="stat">station object to be searched</param>
        /// <returns>true if exist in line, else false</returns>
        public bool ExistStation(BusLineStation stat)
        {
            if (stat is null)
            {
                throw new ArgumentNullException("Not valid station");
            }
            return Stations.Contains(stat);
        }

        /// <summary>
        /// checks if a station exist in this busline
        /// </summary>
        /// <param name="stat">station id to be searched</param>
        /// <returns>true if exist in line, else false</returns>
        public bool ExistStation(int id)
        {
            foreach (var stat in Stations)
            {
                if (stat.BUSSTATIONKEY == id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// reutrns distance between 2 stations
        /// </summary>
        /// <param name="stat1"> first station - starting the ride</param>
        /// <param name="stat2"> second station - ending the ride</param>
        /// <returns> the distance in meters</returns>
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

        /// <summary>
        /// reutrns riding time between 2 stations
        /// </summary>
        /// <param name="stat1"> first station - starting the ride</param>
        /// <param name="stat2"> second station - ending the ride</param>
        /// <returns> the time in seconds (timespan object)</returns>
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

        /// <summary>
        /// returns a bus line with a sub route of this bus line station list
        /// </summary>
        /// <param name="s1"> first station in the sub route </param>
        /// <param name="s2">last station in the sub route </param>
        /// <returns>bus line object with the sub station list and same id</returns>
        public BusLine Route(BusLineStation s1, BusLineStation s2)
        {

            if (s1 is null || s2 is null)
            {
                throw new ArgumentNullException("Not valid stations");
            }

            List<BusLineStation> lst = new List<BusLineStation>(); //station list to be returnes
            //indexes of the given stations
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

        /// <summary>
        /// returns a bus line with a sub route of this bus line station list
        /// </summary>
        /// <param name="s1"> first station id in the sub route </param>
        /// <param name="s2">last station id in the sub route </param>
        /// <returns>bus line object with the sub station list and same id</returns>
        public BusLine Route(int s1, int s2)
        {

            if (!ExistStation(s1) || !ExistStation(s2))
            {
                throw new StationDoesNotExistException("Not valid stations");
            }

            List<BusLineStation> lst = new List<BusLineStation>();
            int start = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s1);
            int end = this.Stations.FindIndex(x => x.BUSSTATIONKEY == s2);

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