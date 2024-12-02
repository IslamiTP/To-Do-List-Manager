using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using todolistmanagercsharp.DataService;
using todolistmanagercsharp.Models;
using todolistmanagercsharp.Views;

namespace todolistmanagercsharp.ViewModels
{
    internal class TaskViewModel : INotifyPropertyChanged
    {




        // Login Properties
        private User _loggedInUser;

        public TaskViewModel(User loggedInUser)
        {
            _loggedInUser = loggedInUser;
            LoadTasks();
        }


        // Task Properties
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Duedate { get; set; }
        public string Recurrence { get; set; } = "None";
        public string TaskPriority { get; set; } = "None";
        public string TaskState { get; set; } = "None";

        public ObservableCollection<TaskChecklist> TaskChecklist { get; set; }

        private readonly TaskDataService _taskDataService;

        // Tasks Data
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        // Task Filters
        private ObservableCollection<Task> _filteredTasks;
        public ObservableCollection<Task> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        private ICollectionView _filteredTasksView;
        public ICollectionView FilteredTasksView
        {
            get
            {
                if (_filteredTasksView == null)
                {
                    _filteredTasksView = CollectionViewSource.GetDefaultView(FilteredTasks);
                }
                return _filteredTasksView;
            }
        }

        // Task Selection
        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor  Task View Model
        public TaskViewModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            TaskPriority = "None";
            TaskState = "None";
            Recurrence = "None";
            Duedate = DateTime.Now;
            _taskDataService = new TaskDataService();
            TaskChecklist = new ObservableCollection<TaskChecklist>();
            LoadTasks();
        }

        // Loading Tasks to List Viewer
        private void LoadTasks()
        {
            if (_loggedInUser == null)
            {
                Console.WriteLine("Error: Logged-in user is null.");
                Tasks = new ObservableCollection<Task>(); // Initialize empty task list
                FilteredTasks = new ObservableCollection<Task>();
                return;
            }

            Console.WriteLine($"Logged-in user: {_loggedInUser.Username}");
            Console.WriteLine($"Number of tasks: {_loggedInUser.Tasks.Count}");

            // Load user-specific tasks
            Tasks = new ObservableCollection<Task>(_loggedInUser.Tasks);
            OnPropertyChanged(nameof(Tasks));

            // FilteredTasks setup
            FilteredTasks = new ObservableCollection<Task>(Tasks);
            if (_filteredTasksView == null)
            {
                _filteredTasksView = CollectionViewSource.GetDefaultView(FilteredTasks);
            }

            RefreshFilteredTasksView();
        }

        private void RefreshFilteredTasksView()
        {
            if (FilteredTasksView == null) return;

            FilteredTasksView.Filter = task =>
            {
                var t = task as Task;
                if (t == null) return false; // Replace `is not` with null check

                // Apply search filter
                var matchesSearch = string.IsNullOrWhiteSpace(SearchText) ||
                                    (t.Title?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                                    (t.Description?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;

                // Apply priority filter
                var matchesPriority = _priorityFilter == null || t.TaskPriority == _priorityFilter;

                // Return true only if both filters match
                return matchesSearch && matchesPriority;
            };

            FilteredTasksView.Refresh();
        }


        // Add Task Command
        public ICommand IAddNewTask => new RelayCommand(AddNewTask);

        private void AddNewTask()
        {
            var newTask = new Task
            {
                Id = _taskDataService.GenTaskId(),
                Title = this.Title,
                Description = this.Description,
                Duedate = this.Duedate,
                TaskPriority = this.TaskPriority,
                TaskState = this.TaskState,
                Recurrence = this.Recurrence
            };

            _taskDataService.AddTask(newTask);
            ClearFields();
            LoadTasks();
        }

        // ClearFields after input
        private void ClearFields()
        {
            Title = "";
            Description = "";
            Duedate = DateTime.Now;
            TaskChecklist.Clear();
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Duedate));
        }

        // Delete Task Command
        public ICommand DeleteTaskCommand => new RelayCommand(DeleteSelectedTask, CanDeleteTask);

        private bool CanDeleteTask()
        {
            // Enables the button only when a task is selected
            return SelectedTask != null;
        }

        private void DeleteSelectedTask()
        {
            if (SelectedTask != null)
            {
                Console.WriteLine($"Deleting Selected Task: {SelectedTask.Title} (ID: {SelectedTask.Id})");
                _taskDataService.DeleteTask(SelectedTask.Id); // Remove from storage
                LoadTasks(); // Reload tasks
            }
            else
            {
                Console.WriteLine("No task selected for deletion.");
            }
        }

        // Edit Task Command
        public ICommand EditTaskCommand => new RelayCommand(OpenEditTaskWindow, CanEditTask);

        private bool CanEditTask()
        {
            return SelectedTask != null; // Only enable if a task is selected
        }

        private void OpenEditTaskWindow()
        {
            if (SelectedTask != null)
            {
                var editTaskWindow = new EditTaskWindow
                {
                    DataContext = new TaskViewModel
                    {
                        Title = SelectedTask.Title,
                        Description = SelectedTask.Description,
                        Duedate = SelectedTask.Duedate,
                        TaskPriority = SelectedTask.TaskPriority,
                        TaskState = SelectedTask.TaskState,
                        Recurrence = SelectedTask.Recurrence
                    }
                };

                editTaskWindow.ShowDialog();

                // After dialog closes, update the original task with changes
                if (editTaskWindow.DataContext is TaskViewModel editedTaskViewModel)
                {
                    SelectedTask.Title = editedTaskViewModel.Title;
                    SelectedTask.Description = editedTaskViewModel.Description;
                    SelectedTask.Duedate = editedTaskViewModel.Duedate;
                    SelectedTask.TaskPriority = editedTaskViewModel.TaskPriority;
                    SelectedTask.TaskState = editedTaskViewModel.TaskState;
                    SelectedTask.Recurrence = editedTaskViewModel.Recurrence;

                    _taskDataService.UpdateTask(SelectedTask); // Save changes to storage
                    LoadTasks(); // Refresh the task list
                }
            }
            else
            {
                Console.WriteLine("Error: No task selected to edit.");
            }
        }

        // Filtering and Searching Logic
        private string _priorityFilter = null; // Store the currently applied priority filter
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                RefreshFilteredTasksView(); // Combine search and priority filters
            }
        }

        public ICommand FilterByHighPriorityCommand => new RelayCommand(() => ApplyFilterByPriority("High"));
        public ICommand FilterByMediumPriorityCommand => new RelayCommand(() => ApplyFilterByPriority("Medium"));
        public ICommand FilterByLowPriorityCommand => new RelayCommand(() => ApplyFilterByPriority("Low"));
        public ICommand ClearFilterCommand => new RelayCommand(ClearFilter);

        private void ApplyFilterByPriority(string priority)
        {
            _priorityFilter = priority; // Set the priority filter
            RefreshFilteredTasksView(); // Reapply the combined filter
        }

        private void ClearFilter()
        {
            _priorityFilter = null; // Clear the priority filter
            RefreshFilteredTasksView(); // Reapply the combined filter
        }


        public ICommand LogoutCommand => new RelayCommand(Logout);

        private void Logout()
        {
            _loggedInUser = null;

            // Close the main window
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Close();

            // Show the login view
            var loginView = new LoginView();
            loginView.Show();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
