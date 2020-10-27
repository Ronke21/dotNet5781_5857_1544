using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using System.Runtime.CompilerServices;

namespace dotNet5781_01_5857_1544
{

    public class Bus
    {
        public int licenseNum { get; set; }
        public DateTime startService { get; set; }

        public double fuel { get; set; }
        private double mileage { get; set; }

        public void addToMileage(int add)
        {
            if (add > 0)
                this.mileage += add;
        }

        public double getMileage()
        {
            return this.mileage;
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
            TimeSpan t = (DateTime.Now - this.lastMaintDate);
            DateTime test = Convert.ToDateTime(t);

            return test.Year >= 1;
        }

        private bool qualifiedFuel(int ride)
        {
            return ride + mileage <= 1200;
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
            Console.WriteLine("Bus number :" + this.stringLicenseNum() + ", mileage since last maintenance: " + (this.mileage - this.lastMaintMileage));
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            List<Bus> DB = new List<Bus>();
            int num;

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
                    //צריך לקלוט פה אם האוטובוס יוצר לפני 2018 ואז מספר ספרות רישוי בהתאם
                    case 1:
                        {
                            int id, fuel, km;
                            DateTime start, lastMaint;
                            do
                            {
                                Console.WriteLine("Please enter the bus license number (7 or 8 digits): ");
                                Int32.TryParse(Console.ReadLine(), out id);
                            } while ((id < 1000000) || (id > 99999999));
                            do
                            {
                                Console.WriteLine("Please enter the bus starting date (dd/mm/yyyy): ");
                                DateTime.TryParse(Console.ReadLine(), out start);
                            } while (start.Year < 1948);
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
                                if (element.licenseNum == id)
                                {
                                    if (element.allQuailified(rideLength))
                                    {
                                        Console.WriteLine("The bus made the ride");
                                        element.fuel -= rideLength;
                                        element.addToMileage(rideLength);
                                    }
                                    else
                                    {
                                        Console.WriteLine("The chosen bus is not qualified for your ride");
                                        break;
                                    }
                                }
                            }
                            Console.WriteLine("Bus license num is not found! ");
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
                                if (element.licenseNum == id)
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
                                Console.WriteLine("Fuel updated!");
                            }
                            else
                            {
                                temp.lastMaintDate = DateTime.Now;
                                temp.lastMaintMileage = temp.getMileage();
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
