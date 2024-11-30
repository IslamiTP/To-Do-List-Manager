using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using todolistmanagercsharp.Models;

namespace todolistmanagercsharp.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Properties
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> FilteredTasks { get; set; } = new ObservableCollection<Task>();

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

        // Commands
        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand FilterByIdCommand { get; }
        public ICommand FilterByLowPriorityCommand { get; }
        public ICommand FilterByMediumPriorityCommand { get; }
        public ICommand FilterByHighPriorityCommand { get; }
        public ICommand ClearFilterCommand { get; }

        private int _taskIdCounter = 1; // To auto-generate task IDs

        public MainWindowViewModel()
        {
            // Initialize commands
            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask, CanModifyTask);
            DeleteTaskCommand = new RelayCommand(DeleteTask, CanModifyTask);
            FilterByIdCommand = new RelayCommand(FilterById);
            FilterByLowPriorityCommand = new RelayCommand(() => FilterByPriority("Low"));
            FilterByMediumPriorityCommand = new RelayCommand(() => FilterByPriority("Medium"));
            FilterByHighPriorityCommand = new RelayCommand(() => FilterByPriority("High"));
            ClearFilterCommand = new RelayCommand(ClearFilter);

            // Load initial tasks (for testing or from persistence)
            LoadTasks();
        }

        private void LoadTasks()
        {
            // Load sample tasks or use persistence logic
            Tasks.Add(new Task { Id = _taskIdCounter++, Title = "Task 1", Priority = "Low", Description = "Sample task 1", Status = "Not Started" });
            Tasks.Add(new Task { Id = _taskIdCounter++, Title = "Task 2", Priority = "Medium", Description = "Sample task 2", Status = "In Progress" });
            Tasks.Add(new Task { Id = _taskIdCounter++, Title = "Task 3", Priority = "High", Description = "Sample task 3", Status = "Completed" });

            foreach (var task in Tasks)
            {
                FilteredTasks.Add(task);
            }
        }

        private void AddTask()
        {
            var newTask = new Task
            {
                Id = _taskIdCounter++,
                Title = "New Task",
                Description = "Description",
                Priority = "Low",
                Status = "Not Started"
            };
            Tasks.Add(newTask);
            FilteredTasks.Add(newTask);
        }

        private void EditTask()
        {
            // Example logic for editing tasks
            if (SelectedTask != null)
            {
                SelectedTask.Title += " (Edited)";
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
                FilteredTasks.Remove(SelectedTask);
            }
        }

        private bool CanModifyTask() => SelectedTask != null;

        private void FilterById()
        {
            FilteredTasks.Clear();
            foreach (var task in Tasks.OrderBy(t => t.Id))
            {
                FilteredTasks.Add(task);
            }
        }

        private void FilterByPriority(string priority)
        {
            FilteredTasks.Clear();
            foreach (var task in Tasks.Where(t => t.Priority == priority))
            {
                FilteredTasks.Add(task);
            }
        }

        private void ClearFilter()
        {
            FilteredTasks.Clear();
            foreach (var task in Tasks)
            {
                FilteredTasks.Add(task);
            }
        }
    }
}
