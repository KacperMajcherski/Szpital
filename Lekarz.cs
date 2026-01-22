using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szpital
{
    internal class Lekarz : Pracownik
    {
        public enum SpecjalizacjaRodzaj
        {
            Kardiolog,
            Urolog,
            Neurolog,
            Laryngolog
        }

        public SpecjalizacjaRodzaj Specjalizacja { get; set; }

        public string NumerPWZ { get; set; }

        public Lekarz(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel, SpecjalizacjaRodzaj specjalizacja, string numerPWZ) : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
            Specjalizacja = specjalizacja;
            NumerPWZ = numerPWZ;
            

    }
        public List<Dyzur> Dyżury { get; set; } = new List<Dyzur>();

    }
}
