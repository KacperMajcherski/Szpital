using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Klasa reprezentująca lekarza w systemie szpitalnym
    /// </summary>
    internal class Doctor : User
    {
        #region Properties

        /// <summary>
        /// Specjalizacja lekarza (np. Kardiolog, Urolog)
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// Numer PWZ (Prawo Wykonywania Zawodu)
        /// </summary>
        public string PWZNumber { get; set; }

        /// <summary>
        /// Lista dyżurów przypisanych do lekarza
        /// </summary>
        public List<Shift> Shifts { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicjalizuje nowego lekarza z podstawowymi danymi i specjalizacją
        /// </summary>
        public Doctor(string firstName, string lastName, string pesel, string username,
                      string password, string specialty, string pwzNumber)
            : base(firstName, lastName, pesel, username, password)
        {
            Specialty = specialty;
            PWZNumber = pwzNumber;
            Shifts = new List<Shift>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Zwraca typ użytkownika
        /// </summary>
        public override string GetUserType() => "Lekarz";

        /// <summary>
        /// Zwraca szczegółowe informacje o lekarzu wraz ze specjalizacją
        /// </summary>
        public override string GetBasicInfo()
            => $"{FirstName} {LastName} ({Specialty})";

        /// <summary>
        /// Dodaje dyżur do harmonogramu lekarza z walidacją
        /// </summary>
        /// <param name="shift">Dyżur do dodania</param>
        /// <returns>true jeśli dyżur został dodany, false jeśli nie spełnia warunków</returns>
        public bool AddShift(Shift shift)
        {
            // Sprawdzamy czy lekarz nie ma już 10 dyżurów w miesiącu
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
