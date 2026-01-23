using System;
using System.Collections.Generic;
using System.Linq;

namespace Zarządzanie_Szpitalem
{
    /// <summary>
    /// Klasa zarządzająca systemem szpitalnym
    /// Odpowiada za zarządzanie użytkownikami, ich autentykacją i wyszukiwaniem
    /// </summary>
    public class HospitalSystem
    {
        #region Fields

        /// <summary>
        /// Lista wszystkich użytkowników w systemie
        /// </summary>
        private List<User> users;

        #endregion

        #region Constructor

        /// <summary>
        /// Inicjalizuje nowy system szpitalny z pustą listą użytkowników
        /// </summary>
        public HospitalSystem()
        {
            users = new List<User>();
        }

        #endregion

        #region User Management

        /// <summary>
        /// Dodaje nowego użytkownika do systemu
        /// </summary>
        /// <param name="user">Użytkownik do dodania</param>
        /// <exception cref="Exception">Wyrzuca wyjątek jeśli użytkownik o tej nazwie już istnieje</exception>
        public void AddUser(User user)
        {
            if (!users.Any(u => u.Username == user.Username))
            {
                users.Add(user);
            }
            else
            {
                throw new Exception("Użytkownik z tą nazwą już istnieje!");
            }
        }

        /// <summary>
        /// Usuwa użytkownika z systemu na podstawie nazwy użytkownika
        /// </summary>
        /// <param name="username">Nazwa użytkownika do usunięcia</param>
        /// <returns>true jeśli użytkownik został usunięty, false jeśli nie znaleziono</returns>
        public bool RemoveUser(string username)
        {
            var user = FindUserByUsername(username);
            if (user != null)
            {
                return users.Remove(user);
            }
            return false;
        }

        #endregion

        #region Authentication

        /// <summary>
        /// Loguje użytkownika na podstawie nazwy i hasła
        /// </summary>
        /// <param name="username">Nazwa użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <returns>Obiekt użytkownika jeśli dane są poprawne, null w innym przypadku</returns>
        public User Login(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        #endregion

        #region Queries

        /// <summary>
        /// Pobiera wszystkich lekarzy i pielęgniarki z systemu
        /// </summary>
        /// <returns>Lista lekarzy i pielęgniarek</returns>
        public List<User> GetDoctorsAndNurses()
        {
            return users.Where(u => u is Doctor || u is Nurse).ToList();
        }

        /// <summary>
        /// Pobiera wszystkich użytkowników z systemu
        /// </summary>
        /// <returns>Kopia listy wszystkich użytkowników</returns>
        public List<User> GetAllUsers() => new List<User>(users);

        /// <summary>
        /// Wyszukuje użytkownika po nazwie użytkownika
        /// </summary>
        /// <param name="username">Nazwa użytkownika do wyszukania</param>
        /// <returns>Znaleziony użytkownik lub null</returns>
        public User FindUserByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username == username);
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Pobiera listę użytkowników do serializacji
        /// </summary>
        /// <returns>Lista wszystkich użytkowników</returns>
        public List<User> GetUsers() => users;

        /// <summary>
        /// Wczytuje użytkowników z deserializacji
        /// </summary>
        /// <param name="loadedUsers">Lista wczytanych użytkowników</param>
        public void SetUsers(List<User> loadedUsers)
        {
            users = loadedUsers;
        }

        #endregion
    }
}