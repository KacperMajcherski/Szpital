using System;
using System.Collections.Generic;

namespace Szpital
{
    internal class Lekarz : Pracownik
    {
        public enum SpecjalizacjaLekarza
        {
            Kardiolog,
            Urolog,
            Neurolog,
            Laryngolog
        }

        public override string Rola => "Lekarz";
        public SpecjalizacjaLekarza Specjalizacja { get; set; }
        public string NumerPWZ { get; set; }

        public Lekarz(
            string imie,
            string nazwisko,
            string nazwaUzytkownika,
            string haslo,
            string pesel,
            SpecjalizacjaLekarza specjalizacja,
            string numerPWZ)
            : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
            Specjalizacja = specjalizacja;
            NumerPWZ = numerPWZ;
        }
    }
}
