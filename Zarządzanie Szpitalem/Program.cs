using System;
using System.Collections.Generic;
using System.Linq;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Główna klasa aplikacji systemu zarządzania szpitalem
    /// </summary>
    class Program
    {
        #region Fields

        /// <summary>
        /// Globalna instancja systemu szpitalnego
        /// </summary>
        private static HospitalSystem system;

        /// <summary>
        /// Serwis do serializacji danych
        /// </summary>
        private static SerializationService serializationService;

        /// <summary>
        /// Ścieżka do pliku z danymi
        /// </summary>
        private static string dataFilePath = "hospital_data.json";

        #endregion

        #region Main Entry Point

        /// <summary>
        /// Punkt wejścia aplikacji
        /// </summary>
        static void Main()
        {
            try
            {
                // Inicjalizujemy system
                system = new HospitalSystem();
                serializationService = new SerializationService();

                // Wczytujemy dane z pliku (jeśli istnieje)
                LoadData();

                // Jeśli system jest pusty, inicjalizujemy dane testowe
                if (system.GetUsers().Count == 0)
                {
                    InitializeTestData();
                }

                // Główna pętla aplikacji
                MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        #endregion

        #region Data Management

        /// <summary>
        /// Wczytuje dane z pliku JSON
        /// </summary>
        private static void LoadData()
        {
            try
            {
                var users = serializationService.LoadFromFile(dataFilePath);
                system.SetUsers(users);
                if (users.Count > 0)
                    Console.WriteLine($"✓ Dane wczytane z pliku ({users.Count} pracowników).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Brak danych, rozpoczynamy od nowa. ({ex.Message})");
            }
        }

        /// <summary>
        /// Zapisuje dane do pliku JSON
        /// </summary>
        private static void SaveData()
        {
            try
            {
                serializationService.SaveToFile(system.GetUsers(), dataFilePath);
                Console.WriteLine("✓ Dane zapisane do pliku.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy zapisie: {ex.Message}");
            }
        }

        /// <summary>
        /// Inicjalizuje dane testowe do demonstracji
        /// </summary>
        private static void InitializeTestData()
        {
            var admin = new Administrator("Jan", "Kowalski", "12345678901", "admin", "admin123");
            var doctor1 = new Doctor("Mariusz", "Nowak", "12345678902", "mnowak", "doc123", "Kardiolog", "PWZ001");
            var doctor2 = new Doctor("Anna", "Słowik", "12345678904", "aslowik", "doc456", "Urolog", "PWZ002");
            var nurse1 = new Nurse("Maria", "Lewandowska", "12345678903", "mlewandowska", "nurse123");

            system.AddUser(admin);
            system.AddUser(doctor1);
            system.AddUser(doctor2);
            system.AddUser(nurse1);

            // Dodajemy przykładowe dyżury
            doctor1.AddShift(new Shift(new DateTime(2026, 1, 15)));
            doctor1.AddShift(new Shift(new DateTime(2026, 1, 17)));
            nurse1.AddShift(new Shift(new DateTime(2026, 1, 20)));

            Console.WriteLine("✓ Dane testowe załadowane.");
        }

        #endregion

        #region Main Menu

        /// <summary>
        /// Wyświetla menu główne z opcją logowania
        /// </summary>
        private static void MainMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════╗");
                Console.WriteLine("║   SYSTEM ZARZĄDZANIA SZPITALEM           ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Zaloguj się");
                Console.WriteLine("2. Wyjście");
                Console.WriteLine();
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        SaveData();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("✗ Nieprawidłowa opcja!");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nNaciśnij Enter...");
                    Console.ReadLine();
                }
            }
        }

        #endregion

        #region Authentication

        /// <summary>
        /// Obsługuje logowanie użytkownika
        /// </summary>
        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("LOGOWANIE");
            Console.WriteLine("════════════════════════════════════════");
            Console.Write("Nazwa użytkownika: ");
            string username = Console.ReadLine();
            Console.Write("Hasło: ");
            string password = Console.ReadLine();

            var loggedInUser = system.Login(username, password);
            if (loggedInUser != null)
            {
                Console.WriteLine($"✓ Zalogowano jako: {loggedInUser.GetBasicInfo()}");

                if (loggedInUser is Administrator)
                {
                    AdminMenu((Administrator)loggedInUser);
                }
                else if (loggedInUser is Doctor || loggedInUser is Nurse)
                {
                    StaffMenu(loggedInUser);
                }
            }
            else
            {
                Console.WriteLine("✗ Błędne dane logowania!");
                System.Threading.Thread.Sleep(2000);
            }
        }

        #endregion

        #region Admin Menu

        /// <summary>
        /// Wyświetla menu administratora
        /// </summary>
        private static void AdminMenu(Administrator admin)
        {
            bool inAdminPanel = true;
            while (inAdminPanel)
            {
                Console.Clear();
                Console.WriteLine($"PANEL ADMINISTRATORA - {admin.FirstName} {admin.LastName}");
                Console.WriteLine("════════════════════════════════════════");
                Console.WriteLine("1. Wyświetl wszystkich pracowników");
                Console.WriteLine("2. Dodaj nowego pracownika");
                Console.WriteLine("3. Edytuj pracownika");
                Console.WriteLine("4. Usuń pracownika");
                Console.WriteLine("5. Dodaj dyżur pracownikowi");
                Console.WriteLine("6. Wyloguj się");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllUsers();
                        break;
                    case "2":
                        AddNewUser();
                        break;
                    case "3":
                        EditUser();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "5":
                        AddShiftToUser();
                        break;
                    case "6":
                        inAdminPanel = false;
                        break;
                    default:
                        Console.WriteLine("✗ Nieprawidłowa opcja!");
                        break;
                }

                if (inAdminPanel && choice != "6")
                {
                    Console.WriteLine("\nNaciśnij Enter...");
                    Console.ReadLine();
                }
            }
        }

        #endregion

        #region Staff Menu

        /// <summary>
        /// Wyświetla menu dla lekarza/pielęgniarki
        /// </summary>
        private static void StaffMenu(User staff)
        {
            bool inStaffPanel = true;
            while (inStaffPanel)
            {
                Console.Clear();
                Console.WriteLine($"PANEL PRACOWNIKA - {staff.GetBasicInfo()}");
                Console.WriteLine("════════════════════════════════════════");
                Console.WriteLine("1. Wyświetl listę lekarzy i pielęgniarek");
                Console.WriteLine("2. Wyświetl dyżury pracownika");
                Console.WriteLine("3. Wyświetl dyżury w miesiącu");
                Console.WriteLine("4. Wyloguj się");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayDoctorsAndNurses();
                        break;
                    case "2":
                        DisplayShifts(staff);
                        break;
                    case "3":
                        DisplayShiftsForMonth(staff);
                        break;
                    case "4":
                        inStaffPanel = false;
                        break;
                    default:
                        Console.WriteLine("✗ Nieprawidłowa opcja!");
                        break;
                }

                if (inStaffPanel && choice != "4")
                {
                    Console.WriteLine("\nNaciśnij Enter...");
                    Console.ReadLine();
                }
            }
        }

        #endregion

        #region Display Operations

        /// <summary>
        /// Wyświetla wszystkich pracowników w systemie
        /// </summary>
        private static void DisplayAllUsers()
        {
            Console.Clear();
            Console.WriteLine("WSZYSCY PRACOWNICY");
            Console.WriteLine("════════════════════════════════════════");
            var users = system.GetAllUsers();

            if (users.Count == 0)
            {
                Console.WriteLine("Brak pracowników w systemie.");
                return;
            }

            int index = 1;
            foreach (var user in users)
            {
                string specialtyInfo = "";
                if (user is Doctor doctor)
                {
                    specialtyInfo = $" - {doctor.Specialty} (PWZ: {doctor.PWZNumber})";
                }
                Console.WriteLine($"{index}. {user.FirstName} {user.LastName} - {user.GetUserType()}{specialtyInfo}");
                index++;
            }
        }

        /// <summary>
        /// Wyświetla lekarzy i pielęgniarki
        /// </summary>
        private static void DisplayDoctorsAndNurses()
        {
            Console.Clear();
            Console.WriteLine("LEKARZE I PIELĘGNIARKI");
            Console.WriteLine("════════════════════════════════════════");
            var staff = system.GetDoctorsAndNurses();

            if (staff.Count == 0)
            {
                Console.WriteLine("Brak lekarzy i pielęgniarek.");
                return;
            }

            int index = 1;
            foreach (var person in staff)
            {
                string specialtyInfo = "";
                if (person is Doctor doctor)
                {
                    specialtyInfo = $" - {doctor.Specialty}";
                }
                Console.WriteLine($"{index}. {person.GetBasicInfo()}{specialtyInfo}");
                index++;
            }
        }

        /// <summary>
        /// Wyświetla wszystkie dyżury pracownika
        /// </summary>
        private static void DisplayShifts(User staff)
        {
            Console.Clear();
            Console.WriteLine("WSZYSTKIE DYŻURY PRACOWNIKA");
            Console.WriteLine("════════════════════════════════════════");

            List<Shift> shifts = GetUserShifts(staff);

            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("Brak przypisanych dyżurów.");
                return;
            }

            var sortedShifts = shifts.OrderBy(s => s.Date).ToList();
            Console.WriteLine($"Razem dyżurów: {sortedShifts.Count}\n");

            for (int i = 0; i < sortedShifts.Count; i++)
            {
                string dayOfWeek = sortedShifts[i].Date.ToString("dddd");
                Console.WriteLine($"{i + 1}. {sortedShifts[i].Date:yyyy-MM-dd} ({dayOfWeek})");
            }

            // Podsumowanie
            Console.WriteLine($"\nPodsumowanie:");
            var groupedByMonth = sortedShifts
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month);

            foreach (var monthGroup in groupedByMonth)
            {
                string monthName = new DateTime(monthGroup.Key.Year, monthGroup.Key.Month, 1).ToString("MMMM yyyy");
                Console.WriteLine($"  - {monthName}: {monthGroup.Count()} dyżurów");
            }
        }

        /// <summary>
        /// Wyświetla dyżury pracownika w wybranym miesiącu z detailem
        /// </summary>
        private static void DisplayShiftsForMonth(User staff)
        {
            Console.Clear();
            Console.WriteLine("DYŻURY W WYBRANYM MIESIĄCU");
            Console.WriteLine("════════════════════════════════════════");

            Console.Write("Podaj rok (YYYY): ");
            if (!int.TryParse(Console.ReadLine(), out int year) || year < 2000 || year > 2100)
            {
                Console.WriteLine("✗ Nieprawidłowy rok!");
                return;
            }

            Console.Write("Podaj miesiąc (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
            {
                Console.WriteLine("✗ Nieprawidłowy miesiąc!");
                return;
            }

            List<Shift> shifts = GetUserShifts(staff);
            if (shifts == null || shifts.Count == 0)
            {
                Console.WriteLine("Brak dyżurów.");
                return;
            }

            var monthShifts = shifts
                .Where(s => s.Date.Year == year && s.Date.Month == month)
                .OrderBy(s => s.Date)
                .ToList();

            if (monthShifts.Count == 0)
            {
                Console.WriteLine($"Brak dyżurów w {month:D2}/{year}");
                return;
            }

            string monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");
            Console.WriteLine($"\nDyżury w {monthName}: ({monthShifts.Count}/10)");
            Console.WriteLine(new string('─', 42));

            for (int i = 0; i < monthShifts.Count; i++)
            {
                string dayOfWeek = monthShifts[i].Date.ToString("dddd");
                string dayName = GetDayName(dayOfWeek);
                Console.WriteLine($"{i + 1}. {monthShifts[i].Date:dd-MM-yyyy} ({dayName})");
            }

            Console.WriteLine(new string('─', 42));
            Console.WriteLine($"Razem: {monthShifts.Count} dyżurów");
        }

        /// <summary>
        /// Zwraca polską nazwę dnia tygodnia
        /// </summary>
        private static string GetDayName(string englishDayName)
        {
            return englishDayName switch
            {
                "Monday" => "Poniedziałek",
                "Tuesday" => "Wtorek",
                "Wednesday" => "Środa",
                "Thursday" => "Czwartek",
                "Friday" => "Piątek",
                "Saturday" => "Sobota",
                "Sunday" => "Niedziela",
                _ => englishDayName
            };
        }

        #endregion

        #region User Management Operations

        /// <summary>
        /// Dodaje nowego pracownika do systemu
        /// </summary>
        private static void AddNewUser()
        {
            Console.Clear();
            Console.WriteLine("DODAJ NOWEGO PRACOWNIKA");
            Console.WriteLine("════════════════════════════════════════");
            
            string type = GetValidUserType();
            if (type == null)
                return;

            Console.Write("Imię: ");
            string firstName = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("✗ Imię nie może być puste!");
                return;
            }

            Console.Write("Nazwisko: ");
            string lastName = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("✗ Nazwisko nie może być puste!");
                return;
            }

            Console.Write("PESEL (11 cyfr): ");
            string pesel = Console.ReadLine()?.Trim();
            if (!ValidatePESEL(pesel))
            {
                Console.WriteLine("✗ PESEL musi zawierać dokładnie 11 cyfr!");
                return;
            }

            Console.Write("Nazwa użytkownika: ");
            string username = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("✗ Nazwa użytkownika nie może być pusta!");
                return;
            }

            if (system.FindUserByUsername(username) != null)
            {
                Console.WriteLine("✗ Użytkownik z tą nazwą już istnieje!");
                return;
            }

            Console.Write("Hasło: ");
            string password = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("✗ Hasło nie może być puste!");
                return;
            }

            try
            {
                User newUser = null;
                if (type == "1")
                {
                    string specialty = GetValidSpecialty();
                    if (specialty == null)
                        return;

                    Console.Write("Numer PWZ (przykład: PWZ001): ");
                    string pwz = Console.ReadLine()?.Trim();
                    
                    if (string.IsNullOrWhiteSpace(pwz))
                    {
                        Console.WriteLine("✗ Numer PWZ nie może być pusty!");
                        return;
                    }

                    newUser = new Doctor(firstName, lastName, pesel, username, password, specialty, pwz);
                }
                else if (type == "2")
                {
                    newUser = new Nurse(firstName, lastName, pesel, username, password);
                }
                else if (type == "3")
                {
                    newUser = new Administrator(firstName, lastName, pesel, username, password);
                }

                if (newUser != null)
                {
                    system.AddUser(newUser);
                    Console.WriteLine("✓ Pracownik dodany pomyślnie!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Błąd: {ex.Message}");
            }
        }

        /// <summary>
        /// Dodaje dyżur pracownikowi
        /// </summary>
        private static void AddShiftToUser()
        {
            Console.Clear();
            Console.WriteLine("DODAJ DYŻUR PRACOWNIKOWI");
            Console.WriteLine("════════════════════════════════════════");

            var staff = system.GetDoctorsAndNurses();
            if (staff.Count == 0)
            {
                Console.WriteLine("Brak lekarzy i pielęgniarek.");
                return;
            }

            Console.WriteLine("Dostępni pracownicy:");
            for (int i = 0; i < staff.Count; i++)
            {
                var shifts = GetUserShifts(staff[i]);
                int shiftCount = shifts?.Count ?? 0;
                Console.WriteLine($"{i + 1}. {staff[i].GetBasicInfo()} (Dyżury: {shiftCount}/10)");
            }

            Console.Write("\nWybierz pracownika (numer): ");
            if (!int.TryParse(Console.ReadLine(), out int staffIndex) || staffIndex < 1 || staffIndex > staff.Count)
            {
                Console.WriteLine("✗ Nieprawidłowy wybór!");
                return;
            }

            var selectedStaff = staff[staffIndex - 1];
            var existingShifts = GetUserShifts(selectedStaff);

            Console.WriteLine("\n" + new string('═', 42));
            Console.WriteLine($"ISTNIEJĄCE DYŻURY - {selectedStaff.GetBasicInfo()}");
            Console.WriteLine(new string('═', 42));

            if (existingShifts == null || existingShifts.Count == 0)
            {
                Console.WriteLine("Brak przypisanych dyżurów.");
            }
            else
            {
                var sortedShifts = existingShifts.OrderBy(s => s.Date).ToList();
                for (int i = 0; i < sortedShifts.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {sortedShifts[i].Date:dddd, dd-MM-yyyy}");
                }
            }

            Console.WriteLine(new string('═', 42));
            Console.Write("\nData dyżuru (YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime shiftDate))
            {
                Console.WriteLine("✗ Nieprawidłowa data!");
                return;
            }

            // Sprawdzenie czy dyżur już istnieje
            if (existingShifts != null && existingShifts.Any(s => s.Date.Date == shiftDate.Date))
            {
                Console.WriteLine($"✗ Pracownik już ma dyżur w dniu {shiftDate:yyyy-MM-dd}!");
                return;
            }

            var shift = new Shift(shiftDate);

            if (selectedStaff is Doctor doctor)
            {
                if (doctor.AddShift(shift))
                {
                    Console.WriteLine("✓ Dyżur dodany pomyślnie!");
                }
                else
                {
                    string reason = GetShiftFailureReason(existingShifts, shiftDate, doctor);
                    Console.WriteLine($"✗ Nie można dodać dyżuru:\n   {reason}");
                }
            }
            else if (selectedStaff is Nurse nurse)
            {
                if (nurse.AddShift(shift))
                {
                    Console.WriteLine("✓ Dyżur dodany pomyślnie!");
                }
                else
                {
                    string reason = GetShiftFailureReason(existingShifts, shiftDate, nurse);
                    Console.WriteLine($"✗ Nie można dodać dyżuru:\n   {reason}");
                }
            }
        }

        /// <summary>
        /// Określa powód, dla którego nie można dodać dyżuru
        /// </summary>
        private static string GetShiftFailureReason(List<Shift> existingShifts, DateTime newShiftDate, User staff)
        {
            // Sprawdzenie limitu 10 dyżurów w miesiącu
            var monthShifts = existingShifts
                .Where(s => s.Date.Year == newShiftDate.Year && s.Date.Month == newShiftDate.Month)
                .ToList();

            if (monthShifts.Count >= 10)
            {
                return $"Pracownik ma już 10 dyżurów w {newShiftDate:MMMM yyyy}";
            }

            // Sprawdzenie dyżurów dzień po dniu
            foreach (var existingShift in existingShifts)
            {
                int daysDifference = Math.Abs((newShiftDate.Date - existingShift.Date.Date).Days);
                if (daysDifference == 1)
                {
                    var otherDay = newShiftDate.AddDays(daysDifference == 1 && newShiftDate > existingShift.Date ? -1 : 1);
                    return $"Pracownik ma dyżur {existingShift.Date:yyyy-MM-dd}, a nowy dyżur byłby dzień po dniu";
                }
            }

            return "Nie można dodać dyżuru z nieznanego powodu";
        }

        /// <summary>
        /// Pobiera i waliduje typ pracownika od użytkownika
        /// </summary>
        private static string GetValidUserType()
        {
            while (true)
            {
                Console.WriteLine("Typ pracownika:");
                Console.WriteLine("  1 - Lekarz");
                Console.WriteLine("  2 - Pielęgniarka");
                Console.WriteLine("  3 - Administrator");
                Console.WriteLine("  0 - Anuluj");
                Console.Write("\nWybierz opcję: ");
                
                string type = Console.ReadLine();

                if (type == "0")
                {
                    Console.WriteLine("Operacja anulowana.");
                    return null;
                }

                if (type == "1" || type == "2" || type == "3")
                {
                    return type;
                }

                Console.WriteLine("✗ Nieprawidłowa opcja! Wybierz 1, 2, 3 lub 0 aby anulować.\n");
            }
        }

        /// <summary>
        /// Pobiera i waliduje specjalność lekarza
        /// </summary>
        private static string GetValidSpecialty()
        {
            string[] validSpecialties = { "Kardiolog", "Urolog", "Neurolog", "Laryngolog" };

            while (true)
            {
                Console.WriteLine("\nDostępne specjalności:");
                for (int i = 0; i < validSpecialties.Length; i++)
                {
                    Console.WriteLine($"  {i + 1} - {validSpecialties[i]}");
                }
                Console.WriteLine("  0 - Anuluj");
                Console.Write("\nWybierz specjalność: ");
                
                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    Console.WriteLine("Operacja anulowana.");
                    return null;
                }

                if (int.TryParse(choice, out int index) && index >= 1 && index <= validSpecialties.Length)
                {
                    return validSpecialties[index - 1];
                }

                Console.WriteLine($"✗ Nieprawidłowy wybór! Wybierz 1-{validSpecialties.Length} lub 0 aby anulować.\n");
            }
        }

        /// <summary>           
        /// Waliduje format PESEL
        /// </summary>
        private static bool ValidatePESEL(string pesel)
        {
            return !string.IsNullOrWhiteSpace(pesel) && pesel.Length == 11 && pesel.All(char.IsDigit);
        }

        /// <summary>
        /// Edytuje dane pracownika
        /// </summary>
        private static void EditUser()
        {
            Console.Clear();
            Console.WriteLine("EDYTUJ PRACOWNIKA");
            Console.WriteLine("════════════════════════════════════════");
            DisplayAllUsers();

            Console.Write("\nPodaj nazwę użytkownika do edycji: ");
            string username = Console.ReadLine()?.Trim();
            var user = system.FindUserByUsername(username);

            if (user == null)
            {
                Console.WriteLine("✗ Nie znaleziono użytkownika!");
                return;
            }

            Console.WriteLine("\nEdytuj dane:");
            Console.Write("Nowe imię (Enter aby pominąć): ");
            string newFirstName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newFirstName))
                user.FirstName = newFirstName.Trim();

            Console.Write("Nowe nazwisko (Enter aby pominąć): ");
            string newLastName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newLastName))
                user.LastName = newLastName.Trim();

            Console.Write("Nowe hasło (Enter aby pominąć): ");
            string newPassword = Console.ReadLine();
            if (!string.IsNullOrEmpty(newPassword))
                user.Password = newPassword.Trim();

            Console.WriteLine("✓ Pracownik edytowany pomyślnie!");
        }

        /// <summary>
        /// Usuwa pracownika z systemu
        /// </summary>
        private static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("USUŃ PRACOWNIKA");
            Console.WriteLine("════════════════════════════════════════");
            DisplayAllUsers();

            Console.Write("\nPodaj nazwę użytkownika do usunięcia: ");
            string username = Console.ReadLine()?.Trim();

            if (system.RemoveUser(username))
            {
                Console.WriteLine("✓ Pracownik usunięty pomyślnie!");
            }
            else
            {
                Console.WriteLine("✗ Nie znaleziono użytkownika!");
            }
        }

        /// <summary>
        /// Pobiera listę dyżurów użytkownika
        /// </summary>
        private static List<Shift> GetUserShifts(User staff)
        {
            if (staff is Doctor doctor)
                return doctor.Shifts;
            else if (staff is Nurse nurse)
                return nurse.Shifts;
            return null;
        }

        #endregion
    }
}