using System;
using System.Collections;
using System.Collections.Generic;
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

        public void AddBusLine(BusLine toAdd)
        {
            if (toAdd is null)
            {
                throw new ArgumentNullException("Can't add null");
            }

            int counter = 0;
            int index = -1;

            foreach (var line in Eged)
            {
                if (line.BUSLINEID == toAdd.BUSLINEID)
                {
                    counter++;
                    index = Eged.IndexOf(line);
                }
            }

            if (counter == 2 || (counter == 1 && Eged[index].FIRSTSTATION == toAdd.FIRSTSTATION))
            {
                throw new BusLineAlreadyExistsException("Bus line already exist");
            }
            // counter = 0 or 1 (if we add the opposite direction line, regular already exist or vice versa)
            Eged.Add(toAdd);
        }

        public void RemoveBusLine(int id)
        {
            int count = Eged.Count;
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id) Eged.Remove(bus);
            }
            if (count == Eged.Count) throw new BusLineDoesNotExistsException("Bus line already exist for both directions");
        }

        public List<BusLine> BusLinesInStations(int id)
        {
            List<BusLine> tmp = new List<BusLine>();

            foreach (var bus in Eged)
            {
                foreach (var station in bus.Stations)
                {
                    if (station.BUSSTATIONKEY == id) tmp.Add(bus);
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
