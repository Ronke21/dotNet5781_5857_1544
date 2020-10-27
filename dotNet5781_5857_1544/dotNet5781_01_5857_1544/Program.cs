using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;

namespace dotNet5781_01_5857_1544
{

    public class Bus
    {
        public int licenseNum;
        public DateTime startService;

        public double fuel = 0;
        private double mileage = 0;
        public double lastMaintMileage = 0;
        public DateTime lastMaintDate = DateTime.Now;

        public Bus(int ID, DateTime begin)
        {
            this.licenseNum = ID;
            this.startService = begin;


        }

        public bool qualifiedMilage()
        {
            return this.mileage - this.lastMaintMileage <= 20000;
        }

        public bool qualifiedDate()
        {
            TimeSpan t = (DateTime.Now - this.lastMaintDate);
            DateTime test = Convert.ToDateTime(t);

            return test.Year >= 1;
        }

        public bool qualifiedFuel(int ride)
        {
            return ride + mileage <= 1200;
        }

        public void printLicenseNum()
        {
            //if (this.licenseNum > 99999999 || this.licenseNum < 1000000)
            //{
            //    Console.WriteLine(" ");
            //}
            if (licenseNum > 9999999)
            {
                Console.WriteLine(this.licenseNum / 100000 + "-" + (this.licenseNum / 1000) % 100 + "-" + this.licenseNum % 1000);
            }
            else
            {
                Console.WriteLine(this.licenseNum / 100000 + "-" + (this.licenseNum / 100) % 1000 + "-" + this.licenseNum % 100);
            }
        }
        public void printBus()
        {
            Console.WriteLine(this.licenseNum);
            Console.WriteLine(this.mileage);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            List<Bus> DB = new List<Bus>();
            int num;
            Console.WriteLine("Enter your choice please:");
            Console.WriteLine("1. To enter new bus");
            Console.WriteLine("2. Choose existing bus");
            Console.WriteLine("3. Refuel or maintenance update");
            Console.WriteLine("4. Print milage for all buses");
            Console.WriteLine("5. Exit");
            Int32.TryParse(Console.ReadLine(), out num);
            switch(num)
            {
                case 1:
                    {
                        Console.WriteLine("Please enter the bus license number: ");
                        Int32.TryParse(Console.ReadLine(), out num);
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

                default:
                    {
                        Console.WriteLine("Error, invalid input, choose again: ");
                        break;
                    }
            }

        }
    }
}
