using System;

namespace Szpital
{
    internal class Administrator : Pracownik
    {
        public override string Rola => "Administrator";

        public Administrator(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel)
            : base(imie, nazwisko, nazwaUzytkownika, haslo, pesel)
        {
        }
    }
}
