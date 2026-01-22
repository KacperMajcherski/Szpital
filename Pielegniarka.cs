using System;

namespace Szpital
{
    internal class Pielegniarka : Pracownik
    {
        public override string Rola => "Pielęgniarka";

        public Pielegniarka(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel)
            : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
        }
    }
}
