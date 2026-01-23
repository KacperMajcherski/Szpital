using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szpital
{
    internal class Pielegniarka : Pracownik
    {
        public Pielegniarka(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel) : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
        }

    }
}
