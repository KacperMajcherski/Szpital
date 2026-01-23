using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarz¹dzanie_Szpitalem
{
    internal class Administrator : User
    {
        #region Constructor

        public Administrator(string firstName, string lastName, string pesel, string username, string password)
            : base(firstName, lastName, pesel, username, password)
        {
        }

        #endregion

        #region Methods

        public override string GetUserType() => "Administrator";

        #endregion
    }
}