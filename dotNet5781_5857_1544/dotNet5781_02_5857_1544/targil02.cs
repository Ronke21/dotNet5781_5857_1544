using System;
using System.Collections.Generic;
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



            for (int i = 0; i < 20; i++)
            {
                List<BusLineStation> station = new List<BusLineStation>();
                for (int j = 0; j < r.Next(5, 15); j++)
                {
                    station.Add(new BusLineStation());
                }
                Eged.AddBusLine(new BusLine(r.Next(200), station));
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
                Console.WriteLine("6. To print ride oportunities between 2 stations");
                Console.WriteLine("7. To print all bus lines in system");
                Console.WriteLine("8. To print all stations and lines going through them");
                Console.WriteLine("9. Exit");
                Int32.TryParse(Console.ReadLine(), out num);
                switch (num)
                {
                    case 1:
                        {
                            try
                            {
                                int id;
                                List<BusLineStation> tmp = new List<BusLineStation>();
                                do
                                {
                                    Console.WriteLine("Please enter the bus line number: ");
                                    id = Convert.ToInt32(Console.ReadLine());
                                } while (id > 999 || id < 1);

                                Console.WriteLine("Choose how many stations");
                                int numOfStations = Convert.ToInt32(Console.ReadLine());
                                for (int j = 0; j < numOfStations; j++)
                                {
                                    tmp.Add(new BusLineStation());
                                }

                                Eged.AddBusLine(new BusLine(id, tmp));
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