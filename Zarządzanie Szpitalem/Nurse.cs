using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Klasa reprezentująca pielęgniarkę w systemie szpitalnym
    /// </summary>
    internal class Nurse : User
    {
        #region Properties

        /// <summary>
        /// Lista dyżurów przypisanych do pielęgniarki
        /// </summary>
        public List<Shift> Shifts { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicjalizuje nową pielęgniarkę z podstawowymi danymi
        /// </summary>
        public Nurse(string firstName, string lastName, string pesel, string username, string password)
            : base(firstName, lastName, pesel, username, password)
        {
            Shifts = new List<Shift>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Zwraca typ użytkownika
        /// </summary>
        public override string GetUserType() => "Pielęgniarka";

        /// <summary>
        /// Zwraca podstawowe informacje o pielęgniarce
        /// </summary>
        public override string GetBasicInfo()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Dodaje dyżur do harmonogramu pielęgniarki z walidacją
        /// Analogicznie do metody w klasie Doctor
        /// </summary>
        /// <param name="shift">Dyżur do dodania</param>
        /// <returns>true jeśli dyżur został dodany, false jeśli nie spełnia warunków</returns>
        public bool AddShift(Shift shift)
        {
            // Sprawdzamy czy pielęgniarka nie ma już 10 dyżurów w miesiącu
            if (Shifts.Count >= 10)
                return false;

            // Sprawdzamy czy dyżury nie są dzień po dniu
            foreach (var existingShift in Shifts)
            {
                if (Math.Abs((shift.Date - existingShift.Date).TotalDays) == 1)
                    return false;
            }

            Shifts.Add(shift);
            return true;
        }

        #endregion
    }
}
