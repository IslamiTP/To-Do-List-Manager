using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolistmanagercsharp.Models
{
    internal class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., "Not Started", "In Progress", "Completed"
        public DateTime Duedate { get; set; }
        public string Priority { get; set; } // e.g., "Low", "Medium", "High"
        public string Recurrence { get; set; } // e.g., "None", "Daily", "Weekly", "Monthly"
        
        public TaskState TaskState { get; set; }
        public TaskPriority TaskPriority { get; set; }

    }

    public enum TaskState
    {   
        // <summary>
        // Task Has Not been Started
        // </summary>
        NotStarted,
        // <summary>
        // Task is inprogress
        // </summary>
        InProgress,
        // <summary>
        // Task is complete
        // </summary>
        Complete
    }

    public enum TaskPriority
    {
        // <summary>
        // Task is Low priority
        // </summary>
        Low,
        // <summary>
        // Task is Medium priority
        // </summary>
        Medium,
        // <summary>
        // Task is High Prioirty
        // </summary>
        High
    }
}
