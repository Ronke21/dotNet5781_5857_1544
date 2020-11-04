using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02_5857_1544
{
    class BusLineCollection : IEnumerable<BusLine>
    {
        public List<BusLine> Eged;

        public BusLineCollection()
        {
            Eged = new List<BusLine>();
        }

        public void addBusLine(BusLine toAdd)
        {
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

            if (counter == 2) return;
            if (counter == 1 && Eged[index].FIRSTSTATION == toAdd.FIRSTSTATION) return;
            // counter = 0 or 1 (the opposite direction line)
            Eged.Add(toAdd);
        }

        public void removeBusLine(int id)
        {
            foreach (var bus in Eged)
            {
                if (bus.BUSLINEID == id) Eged.Remove(bus);
            }
        }

        public List<BusLine> BusLinesInStations(int id)
        {
            List<BusLine> tmp = new List<BusLine>();

            foreach (var bus in Eged)
            {
                foreach (var station in bus.Stations)
                {
                    if(station.BUSSTATIONKEY == id) tmp.Add(bus);
                }
            }

            return tmp;
        }

        public List<BusLine> SortedList()
        {
            List<BusLine> tmp = new List<BusLine>();
            foreach (var bus in Eged)
            {
                tmp.Add(bus);
            }
            tmp.Sort();
            return tmp;
        }

        public BusLine this[int num]
        {
            get
            {

                BusLine temp = null;
                foreach (var bus in Eged)
                {
                    if (bus.BUSLINEID == num) return bus;
                }

                return null;
                //Exception ex = new Exception("bus not in the list");
                //throw ex;
            }
        }

        public IEnumerator<BusLine> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
