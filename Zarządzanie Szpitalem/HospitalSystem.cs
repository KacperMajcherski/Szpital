using System;
using System.Collections.Generic;
using System.Linq;

namespace Zarządzanie_Szpitalem
{
    public class HospitalSystem
    {
        #region Fields

        private List<User> users;

        #endregion

        #region Constructor

        public HospitalSystem()
        {
            users = new List<User>();
        }

        #endregion

        #region User Management

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

        public User Login(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        #endregion

        #region Queries

        public List<User> GetDoctorsAndNurses()
        {
            return users.Where(u => u is Doctor || u is Nurse).ToList();
        }

        public List<User> GetAllUsers() => new List<User>(users);

        public User FindUserByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username == username);
        }

        #endregion

        #region Serialization

        public List<User> GetUsers() => users;

        public void SetUsers(List<User> loadedUsers)
        {
            users = loadedUsers;
        }

        #endregion
    }
}