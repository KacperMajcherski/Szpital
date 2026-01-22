using System;
using System.Collections.Generic;
using System.Linq;

namespace Szpital
{
    public abstract class Pracownik
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pesel { get; set; }
        public string NazwaUzytkownika { get; }
        public string Haslo { get; }
        public abstract string Rola { get; }

        // Lista przypisanych dyżurów (tylko data, bez czasu)
        public List<DateTime> Dyzury { get; } = new List<DateTime>();

        public Pracownik(string imie, string nazwisko, string nazwaUzytkownika, string haslo, string pesel)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            NazwaUzytkownika = nazwaUzytkownika;
            Haslo = haslo;
            Pesel = pesel;
        }

        // Sprawdza ograniczenia lokalne dla pracownika:
        // - max 10 dyżurów w tym samym miesiącu
        // - brak dyżuru tego samego dnia
        // - brak dyżuru dzień przed lub dzień po
        public bool CanAssignDyzur(DateTime date)
        {
            date = date.Date;

            if (Dyzury.Contains(date))
                return false;

            int monthCount = Dyzury.Count(d => d.Year == date.Year && d.Month == date.Month);
            if (monthCount >= 10)
                return false;

            if (Dyzury.Contains(date.AddDays(-1)) || Dyzury.Contains(date.AddDays(1)))
                return false;

            return true;
        }

        // Dodaje dyżur po pozytywnej walidacji
        public void AddDyzur(DateTime date)
        {
            date = date.Date;
            if (!CanAssignDyzur(date))
                throw new InvalidOperationException("Nie można przypisać dyżuru (limit lub konflikt kolejności dni).");

            Dyzury.Add(date);
        }
    }
}
