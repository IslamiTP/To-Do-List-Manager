using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

            //Checks if data folder don't exists styll
            if (!Directory.Exists(dataFolder))
            {
                // Create the directory if folder dont be real.
                Directory.CreateDirectory(dataFolder);
            }

            _filePath = Path.Combine(dataFolder, fileName);

            InitializeFile();
        }

        private void InitializeFile()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Task>()));
            }

            // For Debug
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName));

        }



        public List<Task> LoadTasks()
        {
            string fileContent = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Task>>(fileContent);
        }

        public void SaveTasks(List<Task> tasks)
        {
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void AddTask(Task newTask)
        {
            newTask.Id = GenTaskId();

            // Load tasks
            var task = LoadTasks();
            // adding new task
            task.Add(newTask);
            SaveTasks(task);
        }

        public void UpdateTask(Task updateTask)
        {
            var tasks = LoadTasks();
            var taskIndex = tasks.FindIndex(t => t.Id == updateTask.Id);

            if (taskIndex != -1)
            {
                tasks[taskIndex] = updateTask;
                SaveTasks(tasks);
            }
        }

        public void DeleteTask(int taskId)
        {
            // loading json tasks
            var tasks = LoadTasks();
            // checks tasks
            tasks.RemoveAll(t => t.Id == taskId);
            SaveTasks(tasks);
        }

        public int GenTaskId()
        {
            var tasks = LoadTasks();

            if (!tasks.Any())
            {
                return 1;
            }

            int maxId = tasks.Max(t => t.Id);
            return maxId + 1;
        }
    }
}
