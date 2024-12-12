using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using todolistmanagercsharp.DataService;
using todolistmanagercsharp.Models;
using todolistmanagercsharp.Views;

namespace todolistmanagercsharp.ViewModels
{
    internal class LoginViewModel
    {
        private readonly UserDataService _userDataService;

        public string Username { get; set; }
        public string Password { get; set; }
        public User LoggedInUser { get; private set; }

        public ICommand LoginCommand => new RelayCommand(Login);
        public ICommand RegisterCommand => new RelayCommand(Register);

        public LoginViewModel()
        {
            _userDataService = new UserDataService(); // User management service
        }

        /// <summary>
        /// Login View Method 
        /// </summary>
        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoggedInUser = _userDataService.Authenticate(Username, Password);
            if (LoggedInUser != null)
            {
                Console.WriteLine($"Logged-in user: {LoggedInUser.Username}");
                Console.WriteLine($"Number of tasks: {LoggedInUser.Tasks?.Count ?? 0}");

                MessageBox.Show($"Welcome, {LoggedInUser.Username}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open the main window with user-specific data
                var mainWindow = new MainWindow
                {
                    DataContext = new TaskViewModel(LoggedInUser) // Pass logged-in user to TaskViewModel
                };
                mainWindow.Show();

                // Close the login window
                Application.Current.Windows[0].Close();
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Account Registration Method
        /// </summary>
        private void Register()
        {
            Console.WriteLine($"Register invoked: Username={Username}, Password={Password}");

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userDataService.Register(Username, Password))
            {
                MessageBox.Show("Registration successful. Please log in.", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Username already exists. Please try a different one.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

