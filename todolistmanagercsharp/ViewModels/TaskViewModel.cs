using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using todolistmanagercsharp.DataService;
using todolistmanagercsharp.Models;
using todolistmanagercsharp.Views;




namespace todolistmanagercsharp.ViewModels
{

    internal class TaskViewModel : INotifyPropertyChanged
    {
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
            var tasks = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(tasks);
            FilteredTasks = new ObservableCollection<Task>(tasks);
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

        // Opening Edt Task Window
        public ICommand EditTaskCommand => new RelayCommand(OpenEditTaskWindow, CanEditTask);
        // Checks if Task is editable
        private bool CanEditTask()
        {
            return SelectedTask != null; // Only enable if a task is selected
        }


        // Opening Edt Task Window
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





        // Save Edited Task Command
        public ICommand SaveEditedTaskCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Executing task command...");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"DueDate: {Duedate}");
            Console.WriteLine($"TaskPriority: {TaskPriority}");
            Console.WriteLine($"TaskState: {TaskState}");
            Console.WriteLine($"Recurrence: {Recurrence}");

            // Close the window after saving (if this is called within the EditTaskWindow's DataContext)
            if (Application.Current.Windows.OfType<EditTaskWindow>().FirstOrDefault() is EditTaskWindow editTaskWindow)
            {
                editTaskWindow.Close();
            }
        });

        // Saves the Edited Tasks
        private void SaveEditedTask()
        {
            Console.WriteLine($"SelectedTask: {SelectedTask}");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"DueDate: {Duedate}");
            Console.WriteLine($"TaskPriority: {TaskPriority}");
            Console.WriteLine($"TaskState: {TaskState}");
            Console.WriteLine($"Recurrence: {Recurrence}");

            if (SelectedTask == null)
            {
                Console.WriteLine("Error: SelectedTask is null.");
                return;
            }

            var updatedTask = new Task
            {
                Id = SelectedTask.Id, // Maintain the original ID
                Title = this.Title,
                Description = this.Description,
                Duedate= DateTime.Now,
                TaskPriority = this.TaskPriority,
                TaskState = this.TaskState,
                Recurrence = this.Recurrence
            };

            _taskDataService.UpdateTask(updatedTask); // Update the task in storage
            LoadTasks(); // Reload tasks to reflect changes
        }



        // Filtering Tasks By  The Search Bar
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
                FilteredTasks = new ObservableCollection<Task>(Tasks);
            }
            else
            {
                FilteredTasks = new ObservableCollection<Task>(
                    Tasks.Where(t =>
                        (t.Title?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                        (t.Description?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0));
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
