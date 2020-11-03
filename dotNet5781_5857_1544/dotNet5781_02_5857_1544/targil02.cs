using System;

namespace dotNet5781_02_5857_1544
{
    //מסעיף 6 ואילך

    class Program
    {
        static void Main(string[] args)
        {
            //לאתחל רשימה

            int num;

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