using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Linq;

namespace Zarz¹dzanie_Szpitalem
{
    /// <summary>
    /// Serwis odpowiedzialny za serializacjê i deserializacjê danych
    /// </summary>
    internal class SerializationService
    {
        private readonly JsonSerializerOptions jsonOptions;

        public SerializationService()
        {
            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        /// <summary>
        /// Wczytuje u¿ytkowników z pliku JSON
        /// </summary>
        public List<User> LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<User>();

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<User>();

                var users = new List<User>();
                var jsonArray = JsonSerializer.Deserialize<JsonElement>(json);

                if (jsonArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var userElement in jsonArray.EnumerateArray())
                    {
                        var user = DeserializeUser(userElement);
                        if (user != null)
                            users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B³¹d przy wczytywaniu danych: {ex.Message}");
                return new List<User>();
            }
        }

        /// <summary>
        /// Zapisuje u¿ytkowników do pliku JSON
        /// </summary>
        public void SaveToFile(List<User> users, string filePath)
        {
            try
            {
                var serializedUsers = users.Select(u => SerializeUser(u)).ToList();
                string json = JsonSerializer.Serialize(serializedUsers, jsonOptions);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B³¹d przy zapisie danych: {ex.Message}");
            }
        }

        /// <summary>
        /// Serializuje u¿ytkownika do formatu JSON
        /// </summary>
        private object SerializeUser(User user)
        {
            if (user is Doctor doctor)
            {
                return new
                {
                    Type = "Doctor",
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    PESEL = doctor.PESEL,
                    Username = doctor.Username,
                    Password = doctor.Password,
                    Specialty = doctor.Specialty,
                    PWZNumber = doctor.PWZNumber,
                    Shifts = doctor.Shifts.Select(s => s.Date.ToString("yyyy-MM-dd")).ToList()
                };
            }
            else if (user is Nurse nurse)
            {
                return new
                {
                    Type = "Nurse",
                    FirstName = nurse.FirstName,
                    LastName = nurse.LastName,
                    PESEL = nurse.PESEL,
                    Username = nurse.Username,
                    Password = nurse.Password,
                    Shifts = nurse.Shifts.Select(s => s.Date.ToString("yyyy-MM-dd")).ToList()
                };
            }
            else if (user is Administrator admin)
            {
                return new
                {
                    Type = "Administrator",
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    PESEL = admin.PESEL,
                    Username = admin.Username,
                    Password = admin.Password
                };
            }

            return null;
        }

        /// <summary>
        /// Deserializuje u¿ytkownika z formatu JSON
        /// </summary>
        private User DeserializeUser(JsonElement userElement)
        {
            try
            {
                string type = userElement.GetProperty("Type").GetString();
                string firstName = userElement.GetProperty("FirstName").GetString();
                string lastName = userElement.GetProperty("LastName").GetString();
                string pesel = userElement.GetProperty("PESEL").GetString();
                string username = userElement.GetProperty("Username").GetString();
                string password = userElement.GetProperty("Password").GetString();

                User user = null;

                if (type == "Doctor")
                {
                    string specialty = userElement.GetProperty("Specialty").GetString();
                    string pwzNumber = userElement.GetProperty("PWZNumber").GetString();
                    user = new Doctor(firstName, lastName, pesel, username, password, specialty, pwzNumber);

                    if (userElement.TryGetProperty("Shifts", out var shiftsElement))
                    {
                        foreach (var shiftDate in shiftsElement.EnumerateArray())
                        {
                            if (DateTime.TryParse(shiftDate.GetString(), out DateTime date))
                            {
                                ((Doctor)user).Shifts.Add(new Shift(date));
                            }
                        }
                    }
                }
                else if (type == "Nurse")
                {
                    user = new Nurse(firstName, lastName, pesel, username, password);

                    if (userElement.TryGetProperty("Shifts", out var shiftsElement))
                    {
                        foreach (var shiftDate in shiftsElement.EnumerateArray())
                        {
                            if (DateTime.TryParse(shiftDate.GetString(), out DateTime date))
                            {
                                ((Nurse)user).Shifts.Add(new Shift(date));
                            }
                        }
                    }
                }
                else if (type == "Administrator")
                {
                    user = new Administrator(firstName, lastName, pesel, username, password);
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"B³¹d przy deserializacji u¿ytkownika: {ex.Message}");
                return null;
            }
        }
    }
}