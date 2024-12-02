using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using todolistmanagercsharp.Models;

namespace todolistmanagercsharp.DataService
{
    internal class TaskDataService
    {
        private readonly string _filePath;
        private readonly string folderName = "todolistmanagercsharp";
        private readonly string fileName = "tasks.json";

        public TaskDataService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, folderName);
            string dataFolder = Path.Combine(appFolder, "data");

            // Create data folder if it does not exist
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _filePath = Path.Combine(dataFolder, fileName);

            InitializeFile();
        }


        // Initializer For the Files
        private void InitializeFile()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Task>()));
            }
        }



        // Task Loader
        public List<Task> LoadTasks()
        {
            string fileContent = File.ReadAllText(_filePath);
            var tasks = JsonConvert.DeserializeObject<List<Task>>(fileContent);
            foreach (var task in tasks)
            {
                if (string.IsNullOrEmpty(task.TaskPriority))
                {
                    task.TaskPriority = "None"; // Default to "None" if not set
                }
            }
            return tasks;
        }




        // Task Saver
        public void SaveTasks(List<Task> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Tasks saved to tasks.json successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }


        // Task Adder

        public void AddTask(Task newTask)
        {
            var tasks = LoadTasks();
            newTask.Id = GenTaskId(); // Generate a unique ID
            tasks.Add(newTask);
            SaveTasks(tasks);
        }


        // Task Updater
        public void UpdateTask(Task updatedTask)
        {
            var tasks = LoadTasks();
            var taskIndex = tasks.FindIndex(t => t.Id == updatedTask.Id);

            if (taskIndex != -1)
            {
                Console.WriteLine($"Task with ID {updatedTask.Id} updated successfully.");
                tasks[taskIndex] = updatedTask; // Update the task
                SaveTasks(tasks); // Save updated tasks back to JSON

            }
            else
            {
                Console.WriteLine($"Task with ID {updatedTask.Id} not found.");
            }
        }


        // Task Deleter
        public void DeleteTask(int taskId)
        {
            var tasks = LoadTasks();
            var taskToRemove = tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskToRemove != null)
            {
                Console.WriteLine($"Removing Task with ID: {taskId}");
                tasks.Remove(taskToRemove);
                SaveTasks(tasks);
            }
            else
            {
                Console.WriteLine($"Task with ID {taskId} not found.");
            }
        }



        // Task ID Generator
        public int GenTaskId()
        {
            var tasks = LoadTasks();
            return tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
        }
    }
}
