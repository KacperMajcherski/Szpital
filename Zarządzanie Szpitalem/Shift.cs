using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Klasa reprezentująca dyżur w systemie szpitalnym
    /// </summary>
    internal class Shift
    {
        #region Properties

        /// <summary>
        /// Data i godzina rozpoczęcia dyżuru
        /// </summary>
        public DateTime Date { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicjalizuje nowy dyżur z datą
        /// </summary>
        /// <param name="date">Data dyżuru</param>
        public Shift(DateTime date)
        {
            Date = date;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Zwraca reprezentację tekstową dyżuru w formacie YYYY-MM-DD
        /// </summary>
        public override string ToString() => Date.ToString("yyyy-MM-dd");

        #endregion
    }
}
