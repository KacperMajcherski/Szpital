using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szpital
{
    internal class Pracownik
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pesel { get; set; }
        public string NazwaUzytkownika { get; }
        public string Haslo { get; }

        public Pracownik(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            NazwaUzytkownika = nazwaUzytkownika;
            Haslo = haslo;
            Pesel = pesel;
        }
    }
}
