using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarz¹dzanie_Szpitalem
{
    /// <summary>
    /// Klasa reprezentuj¹ca administratora w systemie szpitalnym
    /// </summary>
    internal class Administrator : User
    {
        #region Constructor

        /// <summary>
        /// Inicjalizuje nowego administratora z podstawowymi danymi
        /// </summary>
        public Administrator(string firstName, string lastName, string pesel, string username, string password)
            : base(firstName, lastName, pesel, username, password)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Zwraca typ u¿ytkownika
        /// </summary>
        public override string GetUserType() => "Administrator";

        #endregion
    }
}