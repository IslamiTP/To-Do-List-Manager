using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todolistmanagercsharp.Models;

namespace todolistmanagercsharp.DataService
{
    internal class UserDataService
    {
        private readonly string _filePath;
        private List<User> _users;

        public UserDataService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "todolistmanagercsharp", "data");
            _filePath = Path.Combine(appFolder, "users.json");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<User>()));
            }

            LoadUsers();
        }

        /// <summary>
        /// Load User Data
        /// </summary>
        private void LoadUsers()
        {
            var fileContent = File.ReadAllText(_filePath);
            _users = JsonConvert.DeserializeObject<List<User>>(fileContent) ?? new List<User>();
        }

        /// <summary>
        /// Saves User Data
        /// </summary>
        private void SaveUsers()
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_users, Formatting.Indented));
        }

        /// <summary>
        /// Users Authentication 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool Register(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
            {
                return false; // Username already exists
            }

            var newUser = new User { Username = username, Password = password };
            _users.Add(newUser);
            SaveUsers();
            return true;
        }

        public List<User> GetAllUsers() => _users;
    }
}