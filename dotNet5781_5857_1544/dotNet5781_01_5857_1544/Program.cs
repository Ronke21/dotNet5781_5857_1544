using System;
using System.ComponentModel;
using System.Xml;

namespace dotNet5781_01_5857_1544
{

    public class Bus
    {
        public int licenseNum;
        public DateTime startService;

        public double fuel = 0;
        public double mileage = 0;
        public double lastMaintMileage = 0;
        public DateTime lastMaintDate = DateTime.Now;

        public Bus(int ID, DateTime begin)
        {
            this.licenseNum = ID;
            this.startService = begin;

            //Console.WriteLine("What is the fuel level?");
            //string input = Console.ReadLine();
            //double.TryParse(input, out this.fuel);

            //Console.WriteLine("What is the mileage?");
            //input = Console.ReadLine();
            //double.TryParse(input, out this.mileage);

            //Console.WriteLine("What is the mileage?");
            //input = Console.ReadLine();
            //double.TryParse(input, out this.mileage);
        }

        public bool qualified()
        {
            if (this.mileage - this.lastMaintMileage >= 20000)
            {
                return false;
            }

            TimeSpan t = (DateTime.Now - this.lastMaintDate);
            DateTime test = Convert.ToDateTime(t);

            return test.Year >= 1;
        }

        public void printBus()
        {
            Console.WriteLine(this.licenseNum);
            Console.WriteLine(this.mileage);
        }
    }

    class Program
    {

        public
        static void Main(string[] args)
        {
            Bus bus1 = new Bus(1234567, DateTime.Today);
            bus1.printBus();

        }
    }
}
