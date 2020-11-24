/*
 * Course mini project in .Net framework
 * Exercise number 1
 * Lecturer - David kidron
 * Student - Amihay Hassan, Ron Keinan
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;

namespace dotNet5781_01_5857_1544
{


    class targil01
    {
        public static DateTime getStartingDate()
        {
            DateTime start;
            do
            {
                Console.WriteLine("Please enter the bus starting date (dd/mm/yyyy): ");
                DateTime.TryParse(Console.ReadLine(), out start);
            }
            while (start.Year < 1948); //recieve a valid starting date since the establishment of Israel
            return start;
        }
        public static DateTime getLastMaint()
        {
            DateTime lastMaint;
            do
            {
                Console.WriteLine("Please enter the last maintenance date (dd/mm/yyyy): ");
                DateTime.TryParse(Console.ReadLine(), out lastMaint);
            } while (lastMaint.Year < 1948);
            return lastMaint;
        }
        public static int get8DigitsLineID(List<Bus> DB)
        {
            int id;
            bool exist = false;
            do
            {
                exist = false;
                Console.WriteLine("Please enter the bus license number (8 digits): ");
                Int32.TryParse(Console.ReadLine(), out id);
                foreach (var bus in DB)
                {
                    if (id == bus.LICENSENUM)
                    {
                        exist = true;
                        break;
                    }
                }
            } while ((id < 10000000) || (id > 99999999) || exist); //valid id 8 digits
            return id;
        }

        public static int get7DigitsLineID(List<Bus> DB)
        {
            int id;
            bool exist = false;
            do
            {
                Console.WriteLine("Please enter the bus license number (7 digits): ");
                Int32.TryParse(Console.ReadLine(), out id);
                foreach (var bus in DB)
                {
                    if (id == bus.LICENSENUM)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            while ((id < 1000000) || (id > 9999999) || exist); //valid id 7 digits
            return id;
        }

        public static int getFuelAmount()
        {
            int fuel;
            do
            {
                Console.WriteLine("Please enter the bus fuel amount (how many KM left in tank): ");
                Int32.TryParse(Console.ReadLine(), out fuel);
            } while (fuel < 0 || fuel > 1200); //valid fuel amount according to the tank size
            return fuel;
        }

        public static int getMileage()
        {
            int km;
            do
            {
                Console.WriteLine("Please enter the bus milage amount (how many KM he drived): ");
                Int32.TryParse(Console.ReadLine(), out km);
            } while (km < 0); //valid km is positive
            return km;
        }

        public static int chooseForRide(int id, int rideLength, List<Bus> DB)
        {
            foreach (Bus element in DB) //search the bus in the DB
            {
                if (element.LICENSENUM == id)
                {
                    if (element.allQuailified(rideLength)) //if can make the ride, update the fuel and km
                    {
                        Console.WriteLine("The bus made the ride");
                        element.Fuel -= rideLength;
                        element.addToMileage(rideLength);
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("The chosen bus is not qualified for your ride"); // add error report?
                        return 2;
                    }
                }
                //else if (element.Equals(DB.Last())) //if the foreach loop passed all the DB
            }
            return 3;
        }
         public static int existBus(List<Bus> DB, int id)
        {
            int index = 0;
            foreach (Bus element in DB)
            {
                if (element.LICENSENUM == id) //find the bus in the DB and make a "pointer" to it
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static void updateBus(List<Bus> DB, int idx)
        {
            Console.WriteLine("Enter 1 -> refuel, otherwise -> maintenance");
            Int32.TryParse(Console.ReadLine(), out int action);
            if (action == 1) //if want to refuel - fill the tank
            {
                DB[idx].Fuel = 1200;
                Console.WriteLine("Fuel updated to 1200!");
            }
            else //if want maintenance - update last maint for today
            {
                DB[idx].lastMaintDate = DateTime.Now;
                DB[idx].lastMaintMileage = DB[idx].MILEAGE;
                Console.WriteLine("Last maintenance date updated for today!");
            }
        }

        public static Random r = new Random(DateTime.Now.Millisecond);
        public static void Main(string[] args)
        {
            List<Bus> DB = new List<Bus>(); // a list of buses - our data base!
            int num;

            //adding 2 buses to DB in order to check the func
            DB.Add(new Bus(1234567, new DateTime(21 / 08 / 1992), 111, 19000, DateTime.Now.AddYears(-2)));
            DB.Add(new Bus(87654321, new DateTime(21 / 4 / 2019), 888, 10000, DateTime.Now));

            // the main is based on a choosing option that repeat itself and provide the request of the user
            do
            {
                Console.WriteLine("\nEnter your choice please:");
                Console.WriteLine("1. To enter new bus");
                Console.WriteLine("2. Choose existing bus");
                Console.WriteLine("3. Refuel or maintenance update");
                Console.WriteLine("4. Print mileage for all buses");
                Console.WriteLine("5. Exit");
                Int32.TryParse(Console.ReadLine(), out num);
                switch (num)
                {
                    case 1:
                        {
                            DateTime start = getStartingDate();

                            int id;
                            if (start.Year > 2017) //so the ID is 8 digit
                            {
                                id = get8DigitsLineID(DB);
                            }
                            else //7 digit ID
                            {
                                id = get7DigitsLineID(DB);
                            }

                            int fuel = getFuelAmount();

                            int km = getMileage();

                            DateTime lastMaint = getLastMaint();

                            DB.Add(new Bus(id, start, fuel, km, lastMaint)); //send details to constructor
                            break;
                        }

                    case 2:
                        {
                            Console.WriteLine("Please enter the bus license number: ");
                            Int32.TryParse(Console.ReadLine(), out int id);
                            int rideLength = r.Next(1200); // a random number of km for the ride 
                            Console.WriteLine("Ride length in KM: " + rideLength);
                            int test = chooseForRide(id, rideLength, DB);
                            if (test==1)
                                Console.WriteLine("The bus made the ride");
                            else if (test == 2)
                                Console.WriteLine("The chosen bus is not qualified for your ride"); // add error report?
                            else
                                Console.WriteLine("Bus license num is not found! ");
                            break;
                        }

                    case 3:
                        {
                            DateTime start = getStartingDate();

                            int id;
                            if (start.Year > 2017) //so the ID is 8 digit
                            {
                                id = get8DigitsLineID(DB);
                            }
                            else //7 digit ID
                            {
                                id = get7DigitsLineID(DB);
                            }

                            int idx = existBus(DB, id);

                            if (idx==-1) //"pointer" is null
                            {
                                Console.WriteLine("Bus is not in the DB!");
                                break;
                            }
                            updateBus(DB, idx);

                            break;
                        }

                    case 4:
                        {
                            foreach (Bus element in DB) //print every bus in the DB
                                element.printMilageSinceLastMaint();
                            break;
                        }

                    case 5: //get out from switch loop
                        {
                            break;
                        }

                    default: //choice different  then 1-5
                        {
                            Console.WriteLine("Error, invalid input, choose again: ");
                            break;
                        }
                }
            } while (num != 5);

        }
    }
}
