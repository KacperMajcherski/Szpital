using System;
using System.Collections.Generic;

namespace Zarządzanie_Szpitalem
{
    public abstract class User
    {
        #region Properties
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        #endregion

        #region Constructor
        
        protected User(string firstName, string lastName, string pesel, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            PESEL = pesel;
            Username = username;
            Password = password;
        }
        
        #endregion

        #region Methods
        
        public abstract string GetUserType();

        public virtual string GetBasicInfo()
        {
            return $"{FirstName} {LastName}";
        }
        
        #endregion
    }
}