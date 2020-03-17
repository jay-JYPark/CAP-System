using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    public class UserAccount
    {
        #region Properties
        string identifier;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string departure;
        public string Departure
        {
            get { return departure; }
            set { departure = value; }
        }

        string telephone;
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion

        public UserAccount()
        {
        }
        public UserAccount(string userID, string userPWD, string userName, string userDeparture, string userTelephone, string userDescription)
        {
            this.identifier = userID;
            this.password = userPWD;
            this.name = userName;
            this.departure = userDeparture;
            this.telephone = userTelephone;
            this.description = userDescription;
        }


        #region ExtensionFunctions
        /// <summary>
        /// ID(Name) 반환
        /// </summary>
        /// <returns></returns>
        public string IDAndName
        {
            get { return (this.identifier + "(" + this.name + ")"); }
        }
        #endregion
    }
}
