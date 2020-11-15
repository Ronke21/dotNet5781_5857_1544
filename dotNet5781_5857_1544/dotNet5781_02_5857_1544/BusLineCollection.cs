using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace dotNet5781_02_5857_1544
{
    class BusLineCollection : IEnumerable<BusLine>
    {
        public static List<BusLine> Eged;

        public BusLineCollection()
        {
            Eged = new List<BusLine>();
        }

        public bool ExistLine(int num)
        {
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == num) return true;
            }

            return false;
        }
        public void AddBusLine(int id)
        {
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    throw new BusLineAlreadyExistsException("Line already exist");
                }
            }

            BusLine reg = new BusLine(id, new List<BusLineStation>(), false);
            BusLine rev = new BusLine(id, new List<BusLineStation>(), true);


            Eged.Add(reg);
            Eged.Add(rev);
        }

        public void RemoveBusLine(int id)
        {
            int index = -1;
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    index = Eged.IndexOf(bus);
                    break;
                }
            }

            if (index == -1)
            {
                throw new BusLineDoesNotExistsException("Line" + id + "does not exist");
            }

            Eged.RemoveAt(index);
            Eged.RemoveAt(index);

            // throw exception if no bus was deleted
        }

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

        public List<BusLine> SortedList()
        {
            List<BusLine> tmp = new List<BusLine>(Eged);
            //foreach (var bus in Eged)
            //{
            //    tmp.Add(bus);
            //}
            tmp.Sort();
            return tmp;
        }

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
                    else
                    {
                        bus.AddStation(bus.Stations.Count - index, stat);
                    }
                }
            }
            if (!found) throw new BusLineDoesNotExistsException("Line" + id + "does not exist");
        }

        public void DelStationFromBusLine(int id, BusLineStation stat)
        {
            bool found = false;
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id)
                {
                    found = true;
                    bus.DelStation(stat);
                }
            }
            if (!found) throw new StationDoesNotExistException("Station" + id + "does not exist");
        }

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

        //public static int counter(int id)
        //{
        //    int count = 0;
        //    foreach (var line in Eged)
        //    {
        //        if (line.BUSLINEID == id)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        //public IEnumerator<BusLine> GetEnumerator()
        //{
        //    for (int i = 0; i < Eged.Count; i++)
        //    {
        //        yield return Eged[i];
        //    }
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
        public IEnumerator<BusLine> GetEnumerator()
        {
            return Eged.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
