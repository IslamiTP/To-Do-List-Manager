using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ToDoListManager
{
    private List<Task> tasks = new List<Task>();
    private int nextId = 1;
    private const string filePath = "tasks.txt"; // Define file path for persistence

    public ToDoListManager()
    {
        LoadTasks();
    }

    public void AddTask(string title, string description, string status, DateTime dueDate, string priority)
    {
        var task = new Task
        {
            ID = nextId++,
            Title = title,
            Description = description,
            Status = status,
            DueDate = dueDate,
            Priority = priority
        };
        tasks.Add(task);
        SaveTasks();
        Console.WriteLine($"Task added with ID: {task.ID}");
    }

    public void ViewTasks(string filter = null)
    {
        var filteredTasks = tasks;

        if (!string.IsNullOrEmpty(filter))
        {
            filteredTasks = tasks.Where(task =>
                task.ID.ToString() == filter ||
                task.Status.Equals(filter, StringComparison.OrdinalIgnoreCase) ||
                task.Priority.Equals(filter, StringComparison.OrdinalIgnoreCase) ||
                task.DueDate.ToString("MM-dd-yyyy") == filter).ToList();
        }

        foreach (var task in filteredTasks)
        {
            Console.WriteLine(task);
        }
    }

    public void DeleteTask(int id)
    {
        var task = tasks.FirstOrDefault(t => t.ID == id);
        if (task != null)
        {
            tasks.Remove(task);
            SaveTasks();
            Console.WriteLine("Task deleted.");
        }
        else
        {
            Console.WriteLine("Task not found.");
        }
    }

    public void UpdateTask(int id, string newStatus = null, DateTime? newDueDate = null)
    {
        var task = tasks.FirstOrDefault(t => t.ID == id);
        if (task != null)
        {
            if (!string.IsNullOrEmpty(newStatus)) task.Status = newStatus;
            if (newDueDate.HasValue) task.DueDate = newDueDate.Value;
            SaveTasks();
            Console.WriteLine("Task updated.");
        }
        else
        {
            Console.WriteLine("Task not found.");
        }
    }

    public void SearchTasks(string keyword)
    {
        var results = tasks.Where(task =>
            task.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            task.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var task in results)
        {
            Console.WriteLine(task);
        }
    }

    private void SaveTasks()
    {
        var lines = tasks.Select(task => 
            $"{task.ID},{task.Title},{task.Description},{task.Status},{task.DueDate:MM-dd-yyyy},{task.Priority}");
        File.WriteAllLines(filePath, lines);
    }

    private void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                tasks.Add(new Task
                {
                    ID = int.Parse(parts[0]),
                    Title = parts[1],
                    Description = parts[2],
                    Status = parts[3],
                    DueDate = DateTime.Parse(parts[4]),
                    Priority = parts[5]
                });
            }
            nextId = tasks.Max(t => t.ID) + 1;
        }
    }
}
