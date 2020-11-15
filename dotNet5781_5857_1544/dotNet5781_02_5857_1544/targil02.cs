using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace dotNet5781_02_5857_1544
{
    //מסעיף 6 ואילך

    class Program
    {
        static void Main(string[] args)
        {
            int num;
            Random r = new Random(DateTime.Now.Millisecond);

            BusLineCollection Eged = new BusLineCollection();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    int line = r.Next(1, 300);
                    Eged.AddBusLine(line);
                    for (int j = 0; j < r.Next(10); j++)
                    {
                        Eged.AddStationToBusLine(line, j, new BusLineStation());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            // the main is based on a choosing option that repeat itself and provide the request of the user
            do
            {
                Console.WriteLine("\nEnter your choice please:");
                Console.WriteLine("1. To add a new bus line");
                Console.WriteLine("2. To add a new station to a bus line");
                Console.WriteLine("3. To delete a bus line");
                Console.WriteLine("4. To delete a station from a bus line");
                Console.WriteLine("5. To search a line by a station number");
                Console.WriteLine("6. To print ride opportunities between 2 stations");
                Console.WriteLine("7. To print all bus lines in system");
                Console.WriteLine("8. To print all stations and lines going through them");
                Console.WriteLine("9. Exit");
                Int32.TryParse(Console.ReadLine(), out num);
                switch (num)
                {
                    case 1: // add empty bus line, adding one line will automatically add the reversed one 
                        {
                            try
                            {
                                // bus line id
                                int id;
                                List<BusLineStation> tmp = new List<BusLineStation>();
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1);

                                Eged.AddBusLine(id);
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (BusLineAlreadyExistsException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        }


                    case 2: // add station to bus line, adding one line will automatically add the reversed one
                        {
                            try
                            {
                                // line id
                                int id;
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1 || !Eged.ExistLine(id));

                                // station id
                                int stat;
                                do
                                {
                                    Console.WriteLine("Enter station number:");
                                    stat = Convert.ToInt32(Console.ReadLine());
                                } while (stat > 999999 || stat < 1);

                                int index;
                                do
                                {
                                    Console.WriteLine("Enter station index:");
                                    index = Convert.ToInt32(Console.ReadLine());
                                } while (index < 1);


                                Eged.AddStationToBusLine(id, index, new BusLineStation(stat));
                            }
                            catch (BusLineDoesNotExistsException e)
                            {
                                Console.WriteLine(e);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            break;
                        }

                    case 3: // remove bus line, will also remove the reversed one (because they have the same id)
                        {
                            // line id
                            int id;
                            do
                            {
                                Console.WriteLine("Please enter the bus line number: ");
                                id = Convert.ToInt32(Console.ReadLine());
                            } while (id > 999 || id < 1 || !Eged.ExistLine(id));

                            Eged.RemoveBusLine(id);

                            break;
                        }

                    case 4: // delete a station from a bus line, will also delete from reversed
                        {
                            try
                            {
                                // bus line id
                                int id;
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1);

                                // station id
                                int stat;
                                do
                                {
                                    Console.WriteLine("Enter station number:");
                                    stat = Convert.ToInt32(Console.ReadLine());
                                } while (stat > 999999 || stat < 1);

                                Eged.DelStationFromBusLine(id, new BusLineStation(stat));
                            }
                            catch (StationDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            break;
                        }
                        // לוודא שזה עובד כי קונטיינס יקבל אובייקט תחנה חדש רנדומלי
                    case 5: // add all the lines their stations list includes a station, then print the list
                        {

                            // station id
                            int stat;
                            do
                            {
                                Console.WriteLine("Enter station number:");
                                stat = Convert.ToInt32(Console.ReadLine());
                            } while (stat > 999999 || stat < 1);

                            List<BusLine> lines = new List<BusLine>();

                            foreach (var line in Eged)
                            {
                                if (line.Stations.Contains(new BusLineStation(stat)))
                                {
                                    lines.Add(line);
                                }
                            }

                            foreach (var line in lines)
                            {
                                Console.WriteLine(line);
                            }

                            break;
                        }

                    case 6:
                        {
                            List<BusLine> answer = new List<BusLine>();
                            int stat1, stat2;
                            do
                            {
                                Console.WriteLine("Enter first station number:");
                                stat1 = Convert.ToInt32(Console.ReadLine());
                            } while (stat1 > 999999 || stat1 < 1);
                            do
                            {
                                Console.WriteLine("Enter last station number:");
                                stat2 = Convert.ToInt32(Console.ReadLine());
                            } while (stat2 > 999999 || stat2 < 1);
                            foreach (var line in Eged)
                            {
                                if (line.ExistStation(stat1) && line.ExistStation(stat2))
                                {
                                    answer.Add(line.Route(stat1, stat2));
                                }
                            }

                            answer.Sort((x, y) => x.TimeBetween2(x.FIRSTSTATION, x.LASTSTATION).CompareTo(y.TimeBetween2(y.FIRSTSTATION, y.LASTSTATION)));
                            foreach (var line in answer)
                            {
                                Console.WriteLine(line + "Route time: " + line.TimeBetween2(line.FIRSTSTATION, line.LASTSTATION));
                            }
                            break;
                        }

                    case 7:
                        {
                            foreach (var bus in Eged)
                            {
                                Console.WriteLine(bus);
                            }
                            break;
                        }

                    case 8:
                        {
                            List<int> arr = new List<int>(); //list of all stations
                            foreach (var line in Eged)
                            {
                                foreach(var stat in line.Stations)
                                {
                                    if (!(arr.Contains(stat.BUSSTATIONKEY)))
                                        arr.Add(stat.BUSSTATIONKEY);
                                }
                            }

                            foreach (int statID in arr)
                            {
                                List<BusLine> tmp = Eged.BusLinesInStations(statID);
                                Console.WriteLine("BusLines in Station number - " + statID + " : ");
                                foreach(var line in tmp)
                                {
                                    Console.Write(line.BUSLINEID + ", ");
                                }
                            }
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