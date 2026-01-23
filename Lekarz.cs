using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szpital
{
    internal enum SpecjalizacjaLekarza
    {
        Kardiolog,
        Urolog,
        Neurolog,
        Laryngolog
    }

    internal class Lekarz : Pracownik
    {
        public SpecjalizacjaLekarza Specjalizacja { get; set; }
        public string NumerPWZ { get; set; }

        public Lekarz(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel, string specjalizacja, string numerPWZ) : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
            if (!Enum.TryParse<SpecjalizacjaLekarza>(specjalizacja, true, out var spec))
            {
                throw new ArgumentException("Nieprawidłowa specjalizacja. Dozwolone wartości: kardiolog, urolog, neurolog, laryngolog.", nameof(specjalizacja));
            }

            Specjalizacja = spec;
            NumerPWZ = numerPWZ;
        }
    }
}
