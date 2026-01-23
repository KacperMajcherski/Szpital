using System;
using System.Collections.Generic;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Klasa abstrakcyjna reprezentująca każdego użytkownika w systemie.
    /// Dziedziczy z niej: Lekarz, Pielęgniarka, Administrator.
    /// </summary>
    public abstract class User
    {
        #region Properties
        
        /// <summary>
        /// Właściwości wspólne dla wszystkich użytkowników
        /// </summary>
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Inicjalizuje wszystkie pola podstawowe użytkownika
        /// </summary>
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
        
        /// <summary>
        /// Metoda abstrakcyjna - każdy typ użytkownika wyświetla swoje dane inaczej
        /// </summary>
        public abstract string GetUserType();

        /// <summary>
        /// Wyświetla podstawowe informacje o użytkowniku
        /// </summary>
        public virtual string GetBasicInfo()
        {
            return $"{FirstName} {LastName}";
        }
        
        #endregion
    }
}