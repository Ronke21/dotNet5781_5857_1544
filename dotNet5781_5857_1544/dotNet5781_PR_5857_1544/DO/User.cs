﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_PR_5857_1544.DO
{
    class User
    {
        #region UserName

        private string userName;
        public string UserName
        {
            get { return userName; }
            //set { userName=value; }
        }

        #endregion

        #region Password

        private string password;
        public string Password
        {
            get { return password; }
            //set { password = value; }
        }

        #endregion

        #region Authorization

        private Authorization permmision;
        public Authorization Authorization
        {
            get { return permmision; }
        }

        #endregion



        #region CTOR

        public User(string n, string pw, Authorization per)
        {
            this.userName = n;
            this.password= pw;
            this.permmision= per;
        }

        #endregion

    }
}
