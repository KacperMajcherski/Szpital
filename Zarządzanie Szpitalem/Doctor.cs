using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    internal class Doctor : User
    {
        #region Properties

        public string Specialty { get; set; }

        public string PWZNumber { get; set; }

        public List<Shift> Shifts { get; set; }

        #endregion

        #region Constructor

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

        public override string GetUserType() => "Lekarz";

        public override string GetBasicInfo()
            => $"{FirstName} {LastName} ({Specialty})";

        public bool AddShift(Shift shift)
        {
            if (Shifts.Count >= 10)
                return false;

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
