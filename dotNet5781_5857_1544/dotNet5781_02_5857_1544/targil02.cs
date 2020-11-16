/*
 * Course mini project in .Net framework
 * Exercise number 2
 * Lecturer - David kidron
 * Student - Amihay Hassan, Ron Keinan
 * 
 * this programs shows a managing system for bus lines and thier stations - and aloows to add, delete and change them
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace dotNet5781_02_5857_1544
{

    class Program
    {
        public static Random r = new Random(DateTime.Now.Millisecond);
        static void Main(string[] args)
        {
            int num;

            BusLineCollection Eged = new BusLineCollection(); //the bus lines in the system

            //add a few bus lines to the current collection - with random details
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    int line = r.Next(1, 300);
                    Eged.AddBusLine(line);
                    int num_of_stat = r.Next(3, 7);
                    for (int j = 0; j < num_of_stat; j++)
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
                Console.WriteLine("5. To print all lines in a station");
                Console.WriteLine("6. To print ride opportunities between 2 stations");
                Console.WriteLine("7. To print all bus lines in system");
                Console.WriteLine("8. To print all stations and lines going through them");
                Console.WriteLine("9. Exit");
                Int32.TryParse(Console.ReadLine(), out num); //get the choice
                switch (num)
                {
                    case 1: // add empty bus line, adding one line will automatically add the reversed one 
                        {
                            try
                            {
                                int id; // bus line id
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1); //3 digit number

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
                            catch (Exception ex)
                            {
                                Console.WriteLine("couldn't identify specific problem" + ex);
                            }
                            Console.WriteLine();
                            break;
                        }


                    case 2: // add station to bus line, adding one line will automatically add the reversed one
                        {
                            try
                            {

                                int id; // line id
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1 || !Eged.ExistLine(id));


                                int stat; // station id
                                do //3 digit uniqe bus line number
                                {
                                    Console.WriteLine("Enter station number:");
                                    stat = Convert.ToInt32(Console.ReadLine());
                                } while (stat > 999999 || stat < 1); //6 digit number

                                int index; //place in station list to add the station
                                do
                                {
                                    Console.WriteLine("Enter station index:");
                                    index = Convert.ToInt32(Console.ReadLine());
                                } while (index < 0); //real place in lisy must be possitive


                                Eged.AddStationToBusLine(id, index, new BusLineStation(stat));
                            }
                            catch (BusLineDoesNotExistsException e)
                            {
                                Console.WriteLine(e);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("couldn't identify specific problem" + e);
                            }
                            Console.WriteLine();
                            break;
                        }

                    case 3: // remove bus line, will also remove the reversed one (because they have the same id)
                        {
                            try
                            {
                                int id; // line id
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1 || !Eged.ExistLine(id)); //3 digit uniqe bus line number

                                Eged.RemoveBusLine(id);
                                Console.WriteLine();
                            }
                            catch (BusLineDoesNotExistsException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("couldn't identify specific problem" + ex);
                            }
                            break;

                        }

                    case 4: // delete a station from a bus line, will also delete from reversed
                        {
                            try
                            {

                                int id; // bus line id
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1); //3 digit bus line number


                                int stat; // station id
                                do
                                {
                                    Console.WriteLine("Enter station number:");
                                    stat = Convert.ToInt32(Console.ReadLine());
                                } while (stat > 999999 || stat < 1); //6 digit number

                                Eged.DelStationFromBusLine(id, new BusLineStation(stat));
                            }
                            catch (StationDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("couldn't identify specific problem" + e);
                            }
                            Console.WriteLine();
                            break;
                        }

                    case 5: // print all the lines passes in a station
                        {
                            try
                            {
                                int stat; // station id
                                do
                                {
                                    Console.WriteLine("Enter station number:");
                                    stat = Convert.ToInt32(Console.ReadLine());
                                } while (stat > 999999 || stat < 1); //6 digit number

                                List<BusLine> lines = new List<BusLine>(); //list of lines to print

                                foreach (var line in Eged) //if the line contains the station, add to list
                                {
                                    if (line.Stations.Contains(new BusLineStation(stat)))
                                    {
                                        lines.Add(line);
                                    }
                                }

                                Console.WriteLine("Buse lines in station " + stat + " : \n");
                                foreach (var line in lines) //print
                                {
                                    Console.Write(line.BUSLINEID + " " + (line.reverse ? "reverse, " : "regular, "));
                                }

                                Console.WriteLine();
                            }
                            catch (StationDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("couldn't identify specific problem" + e);
                            }

                            break;
                        }

                    case 6: //print all possible routes between 2 stations
                        {
                            try
                            {
                                List<BusLine> answer = new List<BusLine>();
                                int stat1, stat2;
                                do
                                {
                                    Console.WriteLine("Enter first station number:");
                                    stat1 = Convert.ToInt32(Console.ReadLine());
                                } while (stat1 > 999999 || stat1 < 1); //6 digit

                                do
                                {
                                    Console.WriteLine("Enter last station number:");
                                    stat2 = Convert.ToInt32(Console.ReadLine());
                                } while (stat2 > 999999 || stat2 < 1 || stat2 == stat1); //6 digit and uniqe

                                foreach (var line in Eged)
                                {
                                    if (line.ExistStation(stat1) && line.ExistStation(stat2))
                                    {
                                        if (line.IndexStation(stat1) > line.IndexStation(stat2))
                                        {
                                            answer.Add(line.Route(stat1, stat2)); //creating list of sub bus lines with the route
                                        }
                                    }
                                }

                                answer.Sort(); //sorting the list by time of ride
                                foreach (var line in answer)
                                {
                                    Console.WriteLine("Line number: " + line.BUSLINEID + (line.reverse ? " reverse " : " regular ") + "Route time: " +
                                                      line.interval + " " + line.AREA); //print
                                }
                            }
                            catch (StationDoesNotExistException ex)
                            {
                                Console.WriteLine(ex);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("couldn't identify specific problem" + e);
                            }

                            break;
                        }

                    case 7: //print all buses in the system and thier details
                        {
                            foreach (var bus in Eged)
                            {
                                Console.WriteLine(bus);
                            }
                            break;
                        }

                    case 8: //print all station numbers and the lines going through them
                        {
                            List<int> arr = new List<int>(); //list of all stations
                            foreach (var line in Eged)
                            {
                                foreach (var stat in line.Stations)
                                {
                                    if (!(arr.Contains(stat.BUSSTATIONKEY)))
                                        arr.Add(stat.BUSSTATIONKEY); // enter stations to list
                                }
                            }

                            foreach (int statID in arr) //check kines in every station
                            {
                                List<BusLine> tmp = Eged.BusLinesInStations(statID);
                                Console.WriteLine("\nBusLines in Station number - " + statID + " : ");
                                foreach (var line in tmp)
                                {
                                    string rev = line.reverse ? " reverse " : " regular ";

                                    Console.Write(line.BUSLINEID + " " + rev + ", ");
                                }
                            }
                            break;
                        }

                    case 9: //get out from switch loop
                        {
                            break;
                        }

                    default: //choice different  then 1-9
                        {
                            Console.WriteLine("Error, invalid input, choose again: ");
                            break;
                        }
                }
            } while (num != 9);

        }
    }
}