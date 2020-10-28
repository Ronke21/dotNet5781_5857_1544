/*
 * Course mini project in .Net framework
 * Exercise number 1
 * Lecturer - David kidron
 * Student - Amihay Hassan, Ron Keinan
 * 
 */


using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;

namespace dotNet5781_01_5857_1544
{

    /// <summary>
    /// class representing a bus unit
    /// </summary>
    public class Bus
    {
        private int licenseNum; // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUM
        {
            get { return licenseNum; }
        }
        public DateTime startService { get; set; } //the day that the bus started riding
        public double fuel { get; set; } //amount of fuel in tank
        private double mileage; //the total kilometers the bus drived - private so it won't be changed to less, can be only added.
        public double MILEAGE
        {
            get { return mileage; }
        }
        public void addToMileage(int add)
        {
            if (add > 0)
                this.mileage += add;
        }

        public double lastMaintMileage { get; set; } = 0;
        public DateTime lastMaintDate { get; set; }

        public Bus(int ID, DateTime begin, int delek, int km, DateTime maint)
        {
            this.licenseNum = ID;
            this.startService = begin;
            this.fuel = delek;
            this.mileage = km;
            this.lastMaintDate = maint;
        }

        private bool qualifiedMilage()
        {
            return this.mileage - this.lastMaintMileage <= 20000;
        }

        private bool qualifiedDate()
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan t = (DateTime.Now - this.lastMaintDate);
         //  DateTime test = Convert.ToDateTime(DateTime.Now - this.lastMaintDate);
           int year = (zeroTime + t).Year - 1;
           return year <= 1;
        }

        private bool qualifiedFuel(int ride)
        {
            return ride + fuel <= 1200;
        }

        public bool allQuailified(int ride)
        {
            return qualifiedFuel(ride) && qualifiedDate() && qualifiedMilage();
        }

        public string stringLicenseNum()
        {

            if (licenseNum > 9999999)
            {
                return this.licenseNum / 100000 + "-" + (this.licenseNum / 1000) % 100 + "-" + this.licenseNum % 1000;
            }
            else
            {
                return this.licenseNum / 100000 + "-" + (this.licenseNum / 100) % 1000 + "-" + this.licenseNum % 100;
            }
        }
        public void printMilageSinceLastMaint()
        {
            Console.WriteLine("\nBus number: " + this.stringLicenseNum() + "\t\tMileage since last maintenance: " + (this.mileage - this.lastMaintMileage) + "\t\tFuel amount: " + this.fuel + "\t\tMileage: " + this.mileage + "\t\tDate of last maintenance: " + this.lastMaintDate);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            List<Bus> DB = new List<Bus>();
            int num;
            DB.Add(new Bus(1234567, new DateTime(21 / 08 / 1992), 111, 19000, DateTime.Now ));
            DB.Add(new Bus(87654321, new DateTime(21 / 4 / 2019), 888, 10000, DateTime.Now ));

            do
            {
                Console.WriteLine("\nEnter your choice please:");
                Console.WriteLine("1. To enter new bus");
                Console.WriteLine("2. Choose existing bus");
                Console.WriteLine("3. Refuel or maintenance update");
                Console.WriteLine("4. Print milage for all buses");
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
                            } while (start.Year < 1948);
                            if (start.Year >2017)
                            {
                                do
                                {
                                    Console.WriteLine("Please enter the bus license number (8 digits): ");
                                    Int32.TryParse(Console.ReadLine(), out id);
                                } while ((id < 10000000) || (id > 99999999));
                            }
                            else
                            {
                                do
                                {
                                    Console.WriteLine("Please enter the bus license number (7 digits): ");
                                    Int32.TryParse(Console.ReadLine(), out id);
                                } while ((id < 1000000) || (id > 9999999));
                            }

                            do
                            {
                                Console.WriteLine("Please enter the bus fuel amount (how many KM left in tank): ");
                                Int32.TryParse(Console.ReadLine(), out fuel);
                            } while (fuel < 0 || fuel > 1200);
                            do
                            {
                                Console.WriteLine("Please enter the bus milage amount (how many KM he drived): ");
                                Int32.TryParse(Console.ReadLine(), out km);
                            } while (km < 0);
                            do
                            {
                                Console.WriteLine("Please enter the last maintenance date (dd/mm/yyyy): ");
                                DateTime.TryParse(Console.ReadLine(), out lastMaint);
                            } while (lastMaint.Year < 1948);

                            DB.Add(new Bus(id, start, fuel, km, lastMaint));
                            break;
                        }

                    case 2:
                        {
                            Console.WriteLine("Please enter the bus license number: ");
                            Int32.TryParse(Console.ReadLine(), out int id);
                            Random r = new Random(DateTime.Now.Millisecond);
                            int rideLength = r.Next(1200);
                            Console.WriteLine("Ride length in KM: " + rideLength);
                            foreach (Bus element in DB)
                            {
                                if (element.LICENSENUM == id)
                                {
                                    if (element.allQuailified(rideLength))
                                    {
                                        Console.WriteLine("The bus made the ride");
                                        element.fuel += rideLength;
                                        element.addToMileage(rideLength);
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("The chosen bus is not qualified for your ride");
                                        break;
                                    }
                                }
                                else if (element.Equals(DB.Last()))
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
                            } while ((id < 1000000) || (id > 99999999));
                            foreach (Bus element in DB)
                            {
                                if (element.LICENSENUM == id)
                                {
                                    temp = element;
                                    break;
                                }
                            }
                            if (temp == null)
                            {
                                Console.WriteLine("Bus is not in the DB!");
                                break;
                            }
                            Console.WriteLine("Enter 1 -> refuel, otherwise -> maintenance");
                            Int32.TryParse(Console.ReadLine(), out int action);
                            if (action == 1)
                            {
                                temp.fuel = 1200;
                                Console.WriteLine("Fuel updated to 1200!");
                            }
                            else
                            {
                                temp.lastMaintDate = DateTime.Now;
                                temp.lastMaintMileage = temp.MILEAGE;
                                Console.WriteLine("Last maintenance date updated for today!");
                            }
                            break;
                        }

                    case 4:
                        {
                            foreach (Bus element in DB)
                                element.printMilageSinceLastMaint();
                            break;
                        }

                    case 5:
                        {
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Error, invalid input, choose again: ");
                            break;
                        }
                }
            } while (num != 5);

        }
    }
}
