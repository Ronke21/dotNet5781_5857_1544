using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    ///     /// class representing a bus unit
    /// </summary>
    public class Bus
    {
        private int licenseNum;   // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUMINT
        {
            get { return licenseNum; }
        }
        public string LICENSENUMSTR
        {
            get
            {
                string num = licenseNum.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
        }

        private Status busState;
        public Status BUSSTATE
        {
            get { return busState; }
            set { busState = value; }
        }
        public string BUSSTATESTR
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
        public string MILAGESINCELASTMAINTSTR
        {
            get { return Convert.ToString(mileage - lastMaintMileage); }
        }

        public void setStatus(int ride = 0)
        {
            if (!this.qualifiedDate() || !this.qualifiedMilage(ride) || !this.qualifiedFuel(ride))
                BUSSTATE = Status.Unfit;
            else
                BUSSTATE = Status.Ready;
        }
        public void addToMileage(int add)            //add to milage, public because it is used after rides
        {
            if (add > 0)
            {
                this.mileage += add;
                this.Fuel -= add;
                setStatus();
            }
        }
        public int lastMaintMileage { get; set; } = 0;   //the Kilometers level in the last maintenance care. for qualifacation
        public DateTime lastMaintDate { get; set; }         //the last date of maintenance care. for qualifacation
        public string LASTMAINTDATESTR
        {
            get { return lastMaintDate.ToString("dd/MM/yyyy"); }
        }
        public Bus(int ID, DateTime begin, int delek, int km, DateTime maint) //constructor, get all the details from user.
        {
            this.licenseNum = ID;
            this.startService = begin;
            this.Fuel = delek;
            this.mileage = km;
            this.lastMaintDate = maint;
            setStatus();
        }

        public Bus()
        {
            this.licenseNum = 0;
            this.startService = DateTime.Today;
            this.Fuel = 0;
            this.mileage = 0;
            this.lastMaintDate = DateTime.Today;
            setStatus();
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

        public void Refuel()
        {
            BUSSTATE = Status.ReFueling;
            Thread.Sleep(3000);
            Fuel = 1200;
            setStatus();
        }

        public void Ride(int km)
        {
            BUSSTATE = Status.During;
            Thread.Sleep(km * 100); // (mileage / 60) * 6000, km/10 = num of seconds
            addToMileage(km);
            setStatus();
        }

        public void Maintain()
        {
            BUSSTATE = Status.InMaintenance;
            Thread.Sleep(5000);
            lastMaintMileage = mileage;
            Fuel = 1200;
            lastMaintDate = DateTime.Today;
            setStatus();
        }
    }

}
