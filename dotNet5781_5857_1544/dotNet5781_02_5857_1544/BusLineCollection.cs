/*
 
A class representing a collection of bus lines - creating a list of them and different methods refering the list

 */

using System.Collections;
using System.Collections.Generic;

namespace dotNet5781_02_5857_1544
{
    class BusLineCollection : IEnumerable<BusLine> //implements ienumarable in order to use foreach
    {
        public static List<BusLine> Eged; //the bus line list

        /// <summary>
        /// this ctor creats an empty list of bus lines
        /// </summary>
        public BusLineCollection()
        {
            Eged = new List<BusLine>();
        }

        /// <summary>
        /// checks if a busline number is in the list
        /// </summary>
        /// <param name="num">bus line number to be searched</param>
        /// <returns>true if exists, else false </returns>
        public bool ExistLine(int num)
        {
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == num) return true;
            }

            return false;
        }

        /// <summary>
        /// gets a busline id and addes a new bus line with the id to the list - twice for regular and reverse line
        /// </summary>
        /// <param name="id">number of bus line to be added</param>
        public void AddBusLine(int id)
        {
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    throw new BusLineAlreadyExistsException("Line already exist");
                }
            }

            //add twice - the line and an opposite version for the reverse line
            BusLine reg = new BusLine(id, new List<BusLineStation>(), false);
            BusLine rev = new BusLine(id, new List<BusLineStation>(), true);

            Eged.Add(reg);
            Eged.Add(rev);
        }

        /// <summary>
        /// gets a bus line id and removes from list
        /// </summary>
        /// <param name="id">bus line number to be deleted</param>
        public void RemoveBusLine(int id)
        {
            int index = -1; //flag
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    index = Eged.IndexOf(bus);
                    break;
                }
            }

            if (index == -1) //bus not found
            {
                throw new BusLineDoesNotExistsException("Line" + id + "does not exist");
            }

            Eged.RemoveAt(index);
            Eged.RemoveAt(index); //the reverse line is following. has the same index after deleting the regular
        }

        /// <summary>
        /// returns all the bus lines in a station 
        /// </summary>
        /// <param name="id">the station to be searched</param>
        /// <returns> bus line list</returns>
        public List<BusLine> BusLinesInStations(int id)
        {
            List<BusLine> tmp = new List<BusLine>();

            foreach (var bus in Eged)
            {
                foreach (var station in bus.Stations)
                {
                    if (station.BUSSTATIONKEY == id)
                    {
                        tmp.Add(bus);
                    }
                }
            }

            if (tmp.Count == 0)
            {
                throw new StationDoesNotExistException("Station Does not exist or unused");
            }

            return tmp;
        }

        /// <summary>
        /// returns a sorted list of all buslines - from the shortest riding time to the lpngest
        /// </summary>
        public List<BusLine> SortedList()
        {
            List<BusLine> tmp = new List<BusLine>(Eged);
            tmp.Sort();
            return tmp;
        }

        /// <summary>
        /// addes a station to an existing busline
        /// </summary>
        /// <param name="id">bus line number to be added to</param>
        /// <param name="index"> place of new station</param>
        /// <param name="stat">station to be added</param>
        public void AddStationToBusLine(int id, int index, BusLineStation stat)
        {
            bool found = false;
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    found = true;
                    if (bus.reverse == false)
                    {
                        bus.AddStation(index, stat);
                    }
                    else //reverse - opposite index
                    {
                        bus.AddStation(bus.Stations.Count - index, stat);
                    }
                }
            } //bus line number doesnt exist in the list

                if (!found) throw new BusLineDoesNotExistsException("Line" + id + "does not exist");
        }

        /// <summary>
        /// deletes a station from bus line chosen by user input
        /// </summary>
        /// <param name="id">bus line number to be deleted from</param>
        /// <param name="stat">station to be deleted</param>
        public void DelStationFromBusLine(int id, BusLineStation stat)
        {
            bool found = false; //flag
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    found = true;
                    bus.DelStation(stat);
                }
            } //nu such bus line number in the list
            if (!found) throw new StationDoesNotExistException("Station" + id + "does not exist");
        }

        /// <summary>
        /// indexer - returns a value by index in the list
        /// </summary>
        /// <param name="num">index number in the bus list</param>
        /// <returns> the object of busline in the index from list</returns>
        public BusLine this[int num]
        {
            get
            {
                if (num < 0 || num >= Eged.Count)
                {
                    throw new OutOfRangeException("Index out of range");
                }
                foreach (var bus in Eged)
                {
                    if (bus.BUSLINEID == num) return bus;
                }
                return null;
            }
        }

        /// <summary>
        /// inumarator - toe make a collection and use foreach
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BusLine> GetEnumerator()
        {
            return Eged.GetEnumerator();
        }

        /// <summary>
        /// inumarator - toe make a collection and use foreach
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
