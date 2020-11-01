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

    /// <summary>
    /// class representing a bus unit
    /// </summary>
    public class Bus
    {
        private int licenseNum;                      // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUM
        {
            get { return licenseNum; }
        }
        public DateTime startService { get; set; }   //the day that the bus started riding
        public double Fuel { get; set; }             //amount of fuel in tank

        private double mileage;                      //the total kilometers the bus drived - private so it won't be changed to less, can be only added.
        public double MILEAGE
        {
            get { return mileage; }
        }
        public void addToMileage(int add)            //add to milage, public because it is used after rides
        {
            if (add > 0)
            {
                this.mileage += add;
            }
        }
        public double lastMaintMileage { get; set; } = 0;   //the Kilometers level in the last maintenance care. for qualifacation
        public DateTime lastMaintDate { get; set; }         //the last date of maintenance care. for qualifacation

        public Bus(int ID, DateTime begin, int delek, int km, DateTime maint) //constructor, get all the details from user.
        {
            this.licenseNum = ID;
            this.startService = begin;
            this.Fuel = delek;
            this.mileage = km;
            this.lastMaintDate = maint;
        }

        /// <summary>
        ///checks if we passed 20,000 km from last maintenance care -so we can't ride until we do another care
        /// </summary>
        /// <returns></returns>
        private bool qualifiedMilage(int ride = 0)
        {
            return this.mileage + ride - this.lastMaintMileage <= 20000;
        }
        /// <summary>
        /// checks if we passed 1 year from last maintenance care -so we can't ride until we do another one
        /// </summary>
        /// <returns></returns>
        public bool qualifiedDate()
        {
            return this.lastMaintDate.AddYears(1).CompareTo(DateTime.Now) > 0;
        }
        /// <summary>
        ///checks if we passed 1,200 km which means the fuel tank is empty -so we can't ride until we refuel
        /// <param name="ride"></param>
        /// <returns></returns>
        private bool qualifiedFuel(int ride)
        {
            return Fuel - ride >= 0;
        }
        /// <summary>
        ///         a public function that gather all the private qualifaction checks
        /// <param name="ride"></param>
        /// <returns></returns>
        public bool allQuailified(int ride)
        {
            return qualifiedFuel(ride) && qualifiedDate() && qualifiedMilage();
        }
        /// <summary>
        /// a function that receive the bus id as an integer and returns as a string in the correct form (xx-xxx-xx)
        /// <returns></returns>
        public string stringLicenseNum()
        {
            if (licenseNum > 9999999)
            {
                return this.licenseNum / 100000 + "-" + (this.licenseNum / 1000) % 100 + "-" + this.licenseNum % 1000;
            }

            return this.licenseNum / 100000 + "-" + (this.licenseNum / 100) % 1000 + "-" + this.licenseNum % 100;

        }
        /// <summary>
        ///a func that prints a bus details
        /// </summary>
        public void printMilageSinceLastMaint()
        {
            Console.WriteLine("\nBus number: " + this.stringLicenseNum() + "\t\tMileage since last maintenance: " + (this.mileage - this.lastMaintMileage) + "\t\tFuel amount: " + this.Fuel + "\t\tMileage: " + this.mileage + "\t\tDate of last maintenance: " + this.lastMaintDate);
        }
    }

    class Program
    {
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
                            int id, fuel, km;
                            DateTime start, lastMaint;

                            do
                            {
                                Console.WriteLine("Please enter the bus starting date (dd/mm/yyyy): ");
                                DateTime.TryParse(Console.ReadLine(), out start);
                            }
                            while (start.Year < 1948); //recieve a valid starting date since the establishment of Israel

                            if (start.Year > 2017) //so the ID is 8 digit
                            {
                                bool exist = false;
                                do
                                {
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
                            }
                            else //7 digit ID
                            {
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
                            }

                            do
                            {
                                Console.WriteLine("Please enter the bus fuel amount (how many KM left in tank): ");
                                Int32.TryParse(Console.ReadLine(), out fuel);
                            } while (fuel < 0 || fuel > 1200); //valid fuel amount according to the tank size
                            do
                            {
                                Console.WriteLine("Please enter the bus milage amount (how many KM he drived): ");
                                Int32.TryParse(Console.ReadLine(), out km);
                            } while (km < 0); //valid km is positive
                            do
                            {
                                Console.WriteLine("Please enter the last maintenance date (dd/mm/yyyy): ");
                                DateTime.TryParse(Console.ReadLine(), out lastMaint);
                            } while (lastMaint.Year < 1948);

                            DB.Add(new Bus(id, start, fuel, km, lastMaint)); //send details to constructor
                            break;
                        }

                    case 2:
                        {
                            Console.WriteLine("Please enter the bus license number: ");
                            Int32.TryParse(Console.ReadLine(), out int id);
                            Random r = new Random(DateTime.Now.Millisecond);
                            int rideLength = r.Next(1200); // a random number of km for the ride 
                            Console.WriteLine("Ride length in KM: " + rideLength);
                            foreach (Bus element in DB) //search the bus in the DB
                            {
                                if (element.LICENSENUM == id)
                                {
                                    if (element.allQuailified(rideLength)) //if can make the ride, update the fuel and km
                                    {
                                        Console.WriteLine("The bus made the ride");
                                        element.Fuel -= rideLength;
                                        element.addToMileage(rideLength);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The chosen bus is not qualified for your ride");
                                        break;
                                    }
                                }
                                else if (element.Equals(DB.Last())) //if the foreach loop passed all the DB
                                    Console.WriteLine("Bus license num is not found! ");
                            }
                            break;
                        }

                    case 3:
                        {
                            int id;
                            Bus temp = null;
                            do
                            {
                                Console.WriteLine("Please enter the bus license number (7 or 8 digits): ");
                                Int32.TryParse(Console.ReadLine(), out id);
                            }
                            while ((id < 1000000) || (id > 99999999));
                            foreach (Bus element in DB)
                            {
                                if (element.LICENSENUM == id) //find the bus in the DB and make a "pointer" to it
                                {
                                    temp = element;
                                    break;
                                }
                            }
                            if (temp == null) //"pointer" is null
                            {
                                Console.WriteLine("Bus is not in the DB!");
                                break;
                            }
                            Console.WriteLine("Enter 1 -> refuel, otherwise -> maintenance");
                            Int32.TryParse(Console.ReadLine(), out int action);
                            if (action == 1) //if want to refuel - fill the tank
                            {
                                temp.Fuel = 1200;
                                Console.WriteLine("Fuel updated to 1200!");
                            }
                            else //if want maintenance - update last maint for today
                            {
                                temp.lastMaintDate = DateTime.Now;
                                temp.lastMaintMileage = temp.MILEAGE;
                                Console.WriteLine("Last maintenance date updated for today!");
                            }
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
