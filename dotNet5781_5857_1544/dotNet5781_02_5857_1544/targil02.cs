using System;
using System.Collections.Generic;

namespace dotNet5781_02_5857_1544
{
    class BusStation
    {
        protected int BusStationKey;
        protected double Latitude;
        protected double Longitude;
        protected string address = "";

        // קונסטרקטור ריק???
        public BusStation(int code, string add = "")
        {
            Random r = new Random(DateTime.Now.Millisecond);
            Latitude = r.NextDouble() * (33.3 - 31) + 31;
            Longitude = r.NextDouble() * 35.5 - 34.3 + 34.3;
            BusStationKey = code;
            address = add;
        }
        public override string ToString()
        {
            return "Bus Station Code: " + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E";
        }
    }


    class BusLineStation : BusStation
    {
        public double distanceFromLast;
        public DateTime rideTimeFromLast;

        public BusLineStation(int code, string add, double distance, DateTime rideTime) : base(code, add)
        {
            distanceFromLast = distance;
            rideTimeFromLast = rideTime;
        }

    }


    class BusLine
    {
        private int BusLineNumber;
        private List<BusLineStation> Stations;
        private BusLineStation FirstStation;
        private BusLineStation LastStation;
        private string Area;

        public BusLine(int num, string reg)
        {
            BusLineNumber = num;
            Area = reg;
        }

        public override string ToString()
        {
            //לייצר 2 מחרוזות עם איטרטורים שמכילות כל התחנות
            return "Bus Line Number: " + BusLineNumber + ", Area: " + Area + "\n Stations regular side:  " + "\n Stations reverse side:  ";
        }

        public void addStation(BusLineStation stat, int place)
        {
            if (place == -1)
                Stations.Add(stat);
            else
                Stations.Insert(place, stat);
            if (place == 1)
                FirstStation = stat;
            else if (place == Stations.Count || place == -1)
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
            //גם פה לבדוק אם בודק מצביע הפניה או ממש אלמנטים . אולי למשל איקוולס
            return Stations.Contains(stat);
        }

        public double distanceBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            double sum = 0;
            int i = 0;
            for (; Stations[i] != stat1; i++) ;
            for (int j = i; Stations[j] != stat2; ++j)
                sum += Stations[j].distanceFromLast;
            return sum;
        }

        public TimeSpan timeBetween2(BusLineStation stat1, BusLineStation stat2)
        {
            TimeSpan sum = new TimeSpan(0, 0, 0);
            int i = 0;
            for (; Stations[i] != stat1; i++) ;
            for (int j = i; Stations[j] != stat2; ++j)
                sum.Add((Stations[j].rideTimeFromLast).TimeOfDay);
            return sum;
        }
    }

    //מסעיף 6 ואילך

    class Program
    {
        static void Main(string[] args)
        {
            //לאתחל רשימה

            int num;

            // the main is based on a choosing option that repeat itself and provide the request of the user
            do
            {
                Console.WriteLine("\nEnter your choice please:");
                Console.WriteLine("1. To add a new bus line");
                Console.WriteLine("2. To add a new station to a bus line");
                Console.WriteLine("3. To delete a bus line");
                Console.WriteLine("4. To delete a station from a bus line");
                Console.WriteLine("5. To search a line by a station number");
                Console.WriteLine("6. To print ride oportunities between 2 stations");
                Console.WriteLine("7. To print all bus lines in system");
                Console.WriteLine("8. To print all stations and lines going through them");
                Console.WriteLine("9. Exit");
                Int32.TryParse(Console.ReadLine(), out num);
                switch (num)
                {
                    case 1:
                        {

                            break;
                        }

                    case 2:
                        {
                            break;
                        }

                    case 3:
                        {
                            break;
                        }

                    case 4:
                        {
                            break;
                        }

                    case 5:
                        {
                            break;
                        }

                    case 6:
                        {
                            break;
                        }

                    case 7:
                        {
                            break;
                        }

                    case 8:
                        {
                            break;
                        }

                    case 9: //get out from switch loop
                        {
                            break;
                        }

                    default: //choice different  then 1-5
                        {
                            Console.WriteLine("Error, invalid input, choose again: ");
                            break;
                        }
                }
            } while (num != 9);

        }
    }
}

