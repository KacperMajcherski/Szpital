using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarządzanie_Szpitalem
{
    internal class Shift
    {
        #region Properties

        public DateTime Date { get; set; }

        #endregion

        #region Constructor

        public Shift(DateTime date)
        {
            Date = date;
        }

        #endregion

        #region Methods

        public override string ToString() => Date.ToString("yyyy-MM-dd");

        #endregion
    }
}
