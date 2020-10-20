using System;

namespace dotNet5781_00_5857_1544
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5857();
            Welcome1544();
            Console.Read();
        }

        static partial void Welcome1544();
        private static void Welcome5857()
        {
            Console.WriteLine("Enter your name: ");
            string name;
            name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
