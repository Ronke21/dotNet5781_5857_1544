/*
This file contains the class for a bus entity (according to properties demands in exercise 1)
 */

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace dotNet5781_03B_5857_1544
{

    /// <summary>
    ///     /// class representing a bus unit
    /// </summary>
    public class Bus : INotifyPropertyChanged
    {
        MainWindow wnd = (MainWindow)Application.Current.MainWindow; //reperence to main window in order to update list box items(buses)

        public static Random r = new Random(DateTime.Now.Millisecond); //static random variable to initiallize buses randomly

        private int _LicenseNum;   // licenseNum is the bus id - can't be changed, so property has only get!
        public int LICENSENUM
        {
            get { return _LicenseNum; }
            set
            {
                _LicenseNum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LICENSENUM"));
            }
        }

        public string LICENSENUMSTR //bus id in string variable - 11-111-11 or 111-11-111
        {
            get
            {
                string num = LICENSENUM.ToString();
                return (num.Length > 7) ? num.Insert(3, "-").Insert(6, "-") : num.Insert(2, "-").Insert(6, "-");
            }
        }

        private double _Ride;
        public double RIDE
        {
            get { return _Ride; }

            set
            {
                _Ride = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RIDE"));
            }
        } //reflects a km num of ride if taken. usually 0

        private Status _BusState; //status of bus by enums (in seperate class)
        public Status BUSSTATE
        {
            get { return _BusState; }
            set
            {
                _BusState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BUSSTATE"));
            }
        }

        //public string BUSSTATESTR //converting the enum to string in a property to be binded in the main window
        //{
        //    get { return _BusState.ToString(); }
        //}

        public DateTime startService { get; set; }   //the day that the bus started riding

        private double _Mileage;    //the total kilometers the bus drived - private so it won't be changed to less, can be only added.
        public double MILEAGE
        {
            get { return Math.Round(_Mileage, 1); }
            set
            {
                _Mileage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MILEAGE"));
            }
        }

        //public string MILEAGESTR
        //{
        //    get { return Convert.ToString(_Mileage); }
        //}

        private double _Fuel;
        public double Fuel
        {
            get => Math.Round(_Fuel, 1);
            set
            {
                _Fuel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Fuel"));
            }
        }           //amount of fuel in tank
        //public string FUELTSTR
        //{
        //    get { return Convert.ToString(Fuel); }
        //}
        private double _LastMaintMileage;   //the Kilometers level in the last maintenance care. for qualifacation
        public double LastMaintMileage
        {
            get { return _LastMaintMileage; }
            set
            {
                _LastMaintMileage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastMaintMileage"));
            }
        }

        private double _MileageSinceLastMaint;
        public double MileageSinceLastMaint
        {
            get { return Math.Round(_MileageSinceLastMaint, 1); }

            set
            {
                _MileageSinceLastMaint = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MileageSinceLastMaint"));
            }

        } //counting 20,000 km since lat maint - to the next one

        private double _MaxRide = 1; //used to describe the ride available range - to the choose bus for a ride window
        public double MaxRide
        {
            get { return _MaxRide; }
            set
            {
                _MaxRide = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxRide"));
            }
        }

        private DateTime _lastMaintDate;         //the last date of maintenance care. for qualifacation
        public DateTime lastMaintDate
        {
            get { return _lastMaintDate.Date; }
            set
            {
                _lastMaintDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("lastMaintDate"));
            }
        }         //the last date of maintenance care. for qualifacation

        //public string MILAGESINCELASTMAINTSTR
        //{
        //    get { return Convert.ToString(_Mileage - lastMaintMileage); }
        //}
        /// <summary>
        /// / a function that checks the bus details and updates its current status
        /// </summary>
        /// <param name="ride"> can recieve number of km to a ride and check qualifacation to the ride. or dont get and it is 0 - so check regular qualifacation</param>
        public void SetStatus(double ride = 0)
        {
            if (!QualifiedDate() || !QualifiedMilage(ride) || !QualifiedFuel(ride)) // of fuel = 0 or bus need to maintain by date/mileage -> it is Unfit!
            {
                BUSSTATE = Status.Unfit;
            }
            else if(MileageSinceLastMaint + 1200 >= 20000 || lastMaintDate < DateTime.Today.AddMonths(-11)) //If bus has less then month or less than 1200 km to the next maintenance - the status is good for ride but tells the system to prepare for maint
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
        public void AddToMileage(double add)
        {
            if (add > 0)
            {
                MILEAGE += add;
                MileageSinceLastMaint += add;
                Fuel -= add;
            }
        }

        //public string LASTMAINTDATESTR
        //{
        //    get { return lastMaintDate.ToString("dd/MM/yyyy"); }
        //}
        /// <summary>
        /// //constructor, get all the details from user.
        /// </summary>
        /// <param name="ID">bus license number</param>
        /// <param name="begin">starting date of bus</param>
        /// <param name="delek">fuel amount</param>
        /// <param name="km">mileage of bus</param>
        /// <param name="maint">last datwe of maintenance</param>
        public Bus(int ID, DateTime begin, double delek, double km, DateTime maint)
        {
            _LicenseNum = ID;
            startService = begin;
            _Fuel = delek;
            _Mileage = km;
            lastMaintDate = maint;
            _MileageSinceLastMaint = km - _LastMaintMileage;
            SetStatus();
        }

        /// <summary>
        /// empty constructor, sets all properties to 0 and dates for today
        /// </summary>
        public Bus()
        {
            _LicenseNum = 0;
            startService = DateTime.Today;
            Fuel = 0;
            MILEAGE = 0;
            lastMaintDate = DateTime.Today;
            _MileageSinceLastMaint = 0;
            SetStatus();
        }

        /// <summary>
        ///checks if we passed 20,000 km from last maintenance care -so we can't ride until we do another care.
        /// </summary>
        /// <param name="ride">can recieve number of ride km and check qualifaction of mileage for the ride</param>
        /// <returns>true if qualified, else false</returns>
        private bool QualifiedMilage(double ride = 0)
        {
            return MILEAGE + ride - _LastMaintMileage <= 20000;
        }

        /// <summary>
        /// checks if we passed 1 year from last maintenance care -so we can't ride until we do another one
        /// </summary>
        /// <returns>true if qualified, else false</returns>
        private bool QualifiedDate()
        {
            return lastMaintDate.AddYears(1).CompareTo(DateTime.Now) > 0;
        }

        /// <summary>
        ///checks if we passed 1,200 km which means the fuel tank is empty -so we can't ride until we refuel
        /// <param name="ride">an recieve number of ride km and check qualifaction of fuel for the ride</param>
        /// <returns>true if qualified, else false</returns>
        private bool QualifiedFuel(double ride)
        {
            return Fuel - ride > 0;
        }

        /// <summary>
        /// a public function that gather all the private qualifaction checks
        /// <param name="ride">an recieve number of ride km and check qualifaction for the ride</param>
        /// <returns>true if qualified, else false</returns>
        public bool AllQuailified(double ride = 0)
        {
            return QualifiedFuel(ride) && QualifiedDate() && QualifiedMilage();
        }

        /// <summary>
        ///a func that prints a bus details
        /// </summary>
        //public void printMilageSinceLastMaint()
        //{
        //    Console.WriteLine("\nBus number: " + this.LICENSENUMSTR + "\t\tMileage since last maintenance: " + (this._Mileage - this.lastMaintMileage) + "\t\tFuel amount: " + this.Fuel + "\t\tMileage: " + this._Mileage + "\t\tDate of last maintenance: " + this.lastMaintDate);
        //}

        //public override string ToString()
        //{
        //    return this.LICENSENUMSTR + "\t" + (this._Mileage - this.lastMaintMileage) + "\t" + this.Fuel + "\t" + this._Mileage + "\t" + this.lastMaintDate.ToString("dd/MM/yyyy") + "\t" + this.BUSSTATE.ToString();
        //}

        /// <summary>
        /// refuals the bus according to recived amount, waits one second for refuling
        /// </summary>
        /// <param name="amount">fuel to refill</param>
        public void Refuel(double amount)
        {
            Thread.Sleep(1200);
            Fuel += amount;
        }

        /// <summary>
        /// sends the bus for a ride and waits 1 second
        /// </summary>
        /// <param name="km">ride length</param>
        public void Ride(double km)
        {
            int speed = 50;// r.Next(20, 50); //random speed in kmh
            double time = (double) km / speed; //the time in hours that takes 10% of the ride
            Thread.Sleep((int)(time * 6000)); //6000 is 6 seconds which is hour in the app clock
            RIDE += km;
            AddToMileage(km);
        }

        /// <summary>
        /// sends the bus for maintenance
        /// </summary>
        /// <param name="amount">way to measure the time for maint - divide th left mileage to maint and minus it</param>
        public void Maintain(double amount)
        {

            Thread.Sleep(14400);
            _MileageSinceLastMaint -= amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }

}
