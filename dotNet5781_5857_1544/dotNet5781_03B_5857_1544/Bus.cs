using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_03B_5857_1544
{
    /// <summary>
    ///     /// class representing a bus unit
    /// </summary>
    public class Bus
    {
        private int licenseNum;   // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUM
        {
            get { return licenseNum; }
        }

        private Status busState;
        public Status BUSSTATE
        {
            get { return busState; }
            set { busState = value; }
        }
        public DateTime startService { get; set; }   //the day that the bus started riding
        public double Fuel { get; set; }             //amount of fuel in tank

        private double mileage;                      //the total kilometers the bus drived - private so it won't be changed to less, can be only added.
        public double MILEAGE
        {
            get { return mileage; }
        }
        public void setStatus(int ride = 0)
        {
            if (!this.qualifiedDate() || !this.qualifiedMilage(ride))
                BUSSTATE = Status.inMaintenance;
            else if (!this.qualifiedFuel(ride))
                BUSSTATE = Status.reFueling;
            else
                BUSSTATE = Status.Ready;

        }
        public void addToMileage(int add)            //add to milage, public because it is used after rides
        {
            if (add > 0)
            {
                this.mileage += add;
                setStatus();
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
        /// a function that receive the bus id as an integer and returns as a string in the correct form (xx-xxx-xx)
        /// <returns></returns>
        public string stringLicenseNum()
        {
            string num = licenseNum.ToString();
            if (licenseNum > 9999999)
            {
                return num.Substring(0, 3) + "-" + num.Substring(3, 2) + "-" + num.Substring(5, 3);
            }
            return num.Substring(0, 2) + "-" + num.Substring(2, 3) + "-" + num.Substring(5, 2);
        }
        /// <summary>
        ///a func that prints a bus details
        /// </summary>
        public void printMilageSinceLastMaint()
        {
            Console.WriteLine("\nBus number: " + this.stringLicenseNum() + "\t\tMileage since last maintenance: " + (this.mileage - this.lastMaintMileage) + "\t\tFuel amount: " + this.Fuel + "\t\tMileage: " + this.mileage + "\t\tDate of last maintenance: " + this.lastMaintDate);
        }
        public override string ToString()
        {
            return this.stringLicenseNum() + "\t" + (this.mileage - this.lastMaintMileage) + "\t" + this.Fuel + "\t" + this.mileage + "\t" + this.lastMaintDate.ToString("dd/MM/yyyy") + "\t" + this.BUSSTATE.ToString();
        }
    }

}
