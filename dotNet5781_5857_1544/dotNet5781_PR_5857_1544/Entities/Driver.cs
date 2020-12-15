using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class Driver
    {
        #region ID

        private readonly int id;

        public int ID
        {
            get { return id; }
        }

        #endregion

        #region FirstName

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
        }

        #endregion

        #region LastName

        private string lastName;

        public string LastName
        {
            get { return lastName; }
        }

        #endregion

        #region Address

        private string address;

        public string Address
        {
            get { return address; }
        }

        #endregion

        #region Phone

        private int phone;

        public int Phone
        {
            get { return phone; }
        }

        #endregion

        #region Started

        private DateTime started;

        public DateTime Started
        {
            get { return started; }
        }

        #endregion

        #region Birthday

        private DateTime birthday;

        public DateTime Birthday
        {
            get { return birthday; }
        }

        #endregion

        #region Picture

        private string pictureLink;

        public string PictureLink
        {
            get { return pictureLink; }
        }

        #endregion

        #region Active

        private bool active;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        #endregion


        #region CTOR

        public Driver(int id, string fn, string ln, string add, int ph, DateTime start, DateTime bday, string picture)
        {
            this.id = id;
            this.firstName = fn;
            this.lastName = ln;
            this.address = add;
            this.phone = ph;
            this.started = start;
            this.birthday = bday;
            this.pictureLink = picture;
            this.active = true;
        }

        #endregion
    }
}
