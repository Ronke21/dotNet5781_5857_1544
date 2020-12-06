/*
This file contains the class for a bus entity (according to properties demands in exercise 1)
 */

using System;
using System.Threading;
using System.Windows;

namespace dotNet5781_03B_5857_1544
{

    /// <summary>
    ///     /// class representing a bus unit
    /// </summary>
    public class Bus
    {

        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reperence to main window in order to update list box items(buses)

        public static Random r = new Random(DateTime.Now.Millisecond); //static random variable to initiallize buses randomly

        private int licenseNum;   // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUMINT
        {
            get { return licenseNum; }
        }
        public string LICENSENUMSTR //bus id in string variable - 11-111-11 or 111-11-111
        {
            get
            {
                string num = licenseNum.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
        }

        public int RIDE { get; set; } //reflects a km num of ride if taken. usually 0

        private Status busState; //status of bus by enums (in seperate class)
        public Status BUSSTATE
        {
            get { return busState; }
            set { busState = value; }
        }
        public string BUSSTATESTR //converting the enum to string in a property to be binded in the main window
        {
            get { return busState.ToString(); }
        }

        public DateTime startService { get; set; }   //the day that the bus started riding
        public int Fuel { get; set; }             //amount of fuel in tank
        public string FUELTSTR
        {
            get { return Convert.ToString(Fuel); }
        }

        private int mileage;                      //the total kilometers the bus drived - private so it won't be changed to less, can be only added.
        public int MILEAGE
        {
            get { return mileage; }
        }
        public string MILEAGESTR
        {
            get { return Convert.ToString(mileage); }
        }
        public int lastMaintMileage { get; set; }   //the Kilometers level in the last maintenance care. for qualifacation
        public int mileageSinceLastMain { get; set; } //counting 20,000 km since lat maint - to the next one
        public string MILAGESINCELASTMAINTSTR
        {
            get { return Convert.ToString(mileage - lastMaintMileage); }
        }
        /// <summary>
        /// / a function that checks the bus details and updates its current status
        /// </summary>
        /// <param name="ride"> can recieve number of km to a ride and check qualifacation to the ride. or dont get and it is 0 - so check regular qualifacation</param>
        public void setStatus(int ride = 0) 
        {
            if (!this.qualifiedDate() || !this.qualifiedMilage(ride) || !this.qualifiedFuel(ride)) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
                BUSSTATE = Status.Unfit;
            else if ((this.lastMaintMileage + 1200 > 20000) || (lastMaintDate < DateTime.Today.AddMonths(-11))) //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
            {
                BUSSTATE = Status.MaintainSoon;
            }
            else
            {
                BUSSTATE = Status.Ready; //ready for ride!
            }
        }
        /// <summary>
        /// updates all properties after a ride - mileage, fuel, mileage since last maintenance and bus status
        /// </summary>
        /// <param name="add">the mileage number to be added</param>
        public void addToMileage(int add)            
        {
            if (add > 0)
            {
                this.mileage += add;
                this.mileageSinceLastMain += add;
                this.Fuel -= add;
                setStatus();
            }
        }

        public DateTime lastMaintDate { get; set; }         //the last date of maintenance care. for qualifacation
        public string LASTMAINTDATESTR
        {
            get { return lastMaintDate.ToString("dd/MM/yyyy"); }
        }
        /// <summary>
        /// //constructor, get all the details from user.
        /// </summary>
        /// <param name="ID">bus license number</param>
        /// <param name="begin">starting date of bus</param>
        /// <param name="delek">fuel amount</param>
        /// <param name="km">mileage of bus</param>
        /// <param name="maint">last datwe of maintenance</param>
        public Bus(int ID, DateTime begin, int delek, int km, DateTime maint) 
        {
            this.licenseNum = ID;
            this.startService = begin;
            this.Fuel = delek;
            this.mileage = km;
            this.lastMaintDate = maint;
            this.mileageSinceLastMain = mileage - lastMaintMileage;
            setStatus();
        }

        /// <summary>
        /// empty constructor, sets all properties to 0 and dates for today
        /// </summary>
        public Bus()
        {
            this.licenseNum = 0;
            this.startService = DateTime.Today;
            this.Fuel = 0;
            this.mileage = 0;
            this.lastMaintDate = DateTime.Today;
            this.mileageSinceLastMain = mileage - lastMaintMileage;
            setStatus();
        }

        /// <summary>
        ///checks if we passed 20,000 km from last maintenance care -so we can't ride until we do another care.
        /// </summary>
        /// <param name="ride">can recieve number of ride km and check qualifaction of mileage for the ride</param>
        /// <returns>true if qualified, else false</returns>
        private bool qualifiedMilage(int ride = 0)
        {
            return this.mileage + ride - this.lastMaintMileage <= 20000;
        }

        /// <summary>
        /// checks if we passed 1 year from last maintenance care -so we can't ride until we do another one
        /// </summary>
        /// <returns>true if qualified, else false</returns>
        public bool qualifiedDate()
        {
            return this.lastMaintDate.AddYears(1).CompareTo(DateTime.Now) > 0;
        }

        /// <summary>
        ///checks if we passed 1,200 km which means the fuel tank is empty -so we can't ride until we refuel
        /// <param name="ride">an recieve number of ride km and check qualifaction of fuel for the ride</param>
        /// <returns>true if qualified, else false</returns>
        private bool qualifiedFuel(int ride)
        {
            return Fuel - ride > 0;
        }

        /// <summary>
        /// a public function that gather all the private qualifaction checks
        /// <param name="ride">an recieve number of ride km and check qualifaction for the ride</param>
        /// <returns>true if qualified, else false</returns>
        public bool allQuailified(int ride)
        {
            return qualifiedFuel(ride) && qualifiedDate() && qualifiedMilage();
        }

        /// <summary>
        ///a func that prints a bus details
        /// </summary>
        public void printMilageSinceLastMaint()
        {
            Console.WriteLine("\nBus number: " + this.LICENSENUMSTR + "\t\tMileage since last maintenance: " + (this.mileage - this.lastMaintMileage) + "\t\tFuel amount: " + this.Fuel + "\t\tMileage: " + this.mileage + "\t\tDate of last maintenance: " + this.lastMaintDate);
        }
        public override string ToString()
        {
            return this.LICENSENUMSTR + "\t" + (this.mileage - this.lastMaintMileage) + "\t" + this.Fuel + "\t" + this.mileage + "\t" + this.lastMaintDate.ToString("dd/MM/yyyy") + "\t" + this.BUSSTATE.ToString();
        }

        /// <summary>
        /// refuals the bus according to recived amount, waits one second for refuling
        /// </summary>
        /// <param name="amount">fuel to refill</param>
        public void Refuel(int amount)
        {
            Thread.Sleep(1000);
            Fuel += amount;
        }

        public int MaxRide { get; set; } = 1; //used to describe the ride available range - to the choose bus for a ride window

        /// <summary>
        /// sends the bus for a ride and waits 1 second
        /// </summary>
        /// <param name="km">ride length</param>
        public void Ride(int km)
        {
            Thread.Sleep(1000);
            RIDE += km;
            addToMileage(km);
        }

        /// <summary>
        /// sends the bus for maintenance
        /// </summary>
        /// <param name="amount">way to measure the time for maint - divide th left mileage to maint and minus it</param>
        public void Maintain(int amount)
        {
            Thread.Sleep(1440);
            mileageSinceLastMain -= amount;
        }

    }

}
