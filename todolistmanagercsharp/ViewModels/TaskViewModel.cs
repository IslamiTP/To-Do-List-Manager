using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using todolistmanagercsharp.DataService;
using todolistmanagercsharp.Models;

namespace todolistmanagercsharp.ViewModels
{
    internal class TaskViewModel : INotifyPropertyChanged
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Duedate { get; set; }
        public string Recurrence { get; set; } // e.g., "None", "Daily", "Weekly", "Monthly"
        public string TaskPriority { get; set; } = "None"; // Default to "None"
        public string TaskState { get; set; } = "None";   // Default to "None"
        public ObservableCollection<TaskChecklist> TaskChecklist { get; set; }
        public ICommand IAddNewTask => new RelayCommand(AddNewTask);



        private readonly TaskDataService _taskDataService;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskViewModel()
        {
            _taskDataService = new TaskDataService(); // Initializing the TaskDataService
            TaskChecklist = new ObservableCollection<TaskChecklist>();
            LoadTasks(); // Load tasks at startup
            TaskPriority = this.TaskPriority;
            TaskState = this.TaskState;
            Recurrence = "None"; // Default recurrence
            Duedate = DateTime.Now;
        }

        private void LoadTasks()
        {
            var TaskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(TaskList);
            FilteredTasks = new ObservableCollection<Task>(TaskList); // Initially show all task
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterTasks();
            }
        }

        private void FilterTasks()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If no search text, show all tasks
                FilteredTasks = new ObservableCollection<Task>(Tasks);
            }
            else
            {
                // Filter tasks based on Title or Description
                var filtered = Tasks.Where(task =>
                    (task.Title?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                    (task.Description?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0);

                FilteredTasks = new ObservableCollection<Task>(filtered);
            }
        }


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

        public void AddNewTask()
        {
       
            Task newTask = new Task()
            {
                Title = this.Title,
                Description = this.Description,
                Id = _taskDataService.GenTaskId(),
                Duedate = this.Duedate,
                TaskPriority = this.TaskPriority,
                TaskState = this.TaskState,
                Recurrence = this.Recurrence,
            };

            _taskDataService.AddTask(newTask);

            ClearFields();

            LoadTasks();

        }

        private void ClearFields()
        {
            Title = "";
            Description = "";
            Duedate = DateTime.Now;
            TaskChecklist.Clear();

            OnPropertyChanged(Title);
            OnPropertyChanged(Description);
            OnPropertyChanged(nameof(Duedate));
        }

        public void UpdateTask(Task updateTask)
        {
            _taskDataService.UpdateTask(updateTask);
            LoadTasks();
        }

        public void DeleteTask(int taskId)
        {
            _taskDataService?.DeleteTask(taskId);
            LoadTasks();
        }


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


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
