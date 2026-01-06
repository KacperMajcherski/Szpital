using System;
using System.Collections.Generic;
using System.Linq;

namespace Szpital
{
    internal class Harmonogram
    {
        // Lista wszystkich znanych pracowników (mo¿na inicjalizowaæ istniej¹cymi pracownikami)
        private readonly List<Pracownik> _pracownicy = new List<Pracownik>();

        public Harmonogram(IEnumerable<Pracownik>? pracownicy = null)
        {
            if (pracownicy != null)
                _pracownicy.AddRange(pracownicy);
        }

        public void RejestrujPracownika(Pracownik p)
        {
            if (!_pracownicy.Contains(p))
                _pracownicy.Add(p);
        }

        // Próba przypisania dy¿uru. Zwraca true jeœli OK, w error opis przyczyny w przeciwnym razie.
        public bool TryPrzypiszDyzur(Pracownik pracownik, DateTime date, out string? error)
        {
            date = date.Date;
            error = null;

            if (!_pracownicy.Contains(pracownik))
                _pracownicy.Add(pracownik);

            if (!pracownik.CanAssignDyzur(date))
            {
                error = "Pracownik przekroczy³ limit dy¿urów w miesi¹cu, ma ju¿ dy¿ur tego dnia lub ma dy¿ur dzieñ po dniu.";
                return false;
            }

            // Jeœli to lekarz — sprawdŸ czy na ten dzieñ nie ma ju¿ lekarza tej samej specjalizacji
            if (pracownik is Lekarz lek)
            {
                var konflikt = _pracownicy
                    .OfType<Lekarz>()
                    .FirstOrDefault(l => l.Dyzury.Contains(date) && l.Specjalizacja == lek.Specjalizacja);

                if (konflikt != null)
                {
                    error = $"Na {date:d} jest ju¿ przypisany lekarz o specjalizacji {lek.Specjalizacja} ({konflikt.Imie} {konflikt.Nazwisko}).";
                    return false;
                }
            }

            // Wszystko OK — przypisz
            pracownik.AddDyzur(date);
            return true;
        }

        // Pomocnicze: pobierz mapê dy¿urów wed³ug daty
        public IDictionary<DateTime, List<Pracownik>> GetDyzuryByDate()
        {
            var dict = new Dictionary<DateTime, List<Pracownik>>();

            foreach (var p in _pracownicy)
            {
                foreach (var d in p.Dyzury)
                {
                    if (!dict.TryGetValue(d, out var list))
                    {
                        list = new List<Pracownik>();
                        dict[d] = list;
                    }
                    list.Add(p);
                }
            }

            return dict;
        }
    }
}