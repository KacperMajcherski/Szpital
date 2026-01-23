using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    internal class Nurse : User
    {
        #region Properties

        public List<Shift> Shifts { get; set; }

        #endregion

        #region Constructor

        public Nurse(string firstName, string lastName, string pesel, string username, string password)
            : base(firstName, lastName, pesel, username, password)
        {
            Shifts = new List<Shift>();
        }

        #endregion

        #region Methods

        public override string GetUserType() => "Pielęgniarka";

        public override string GetBasicInfo()
        {
            return $"{FirstName} {LastName}";
        }

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
