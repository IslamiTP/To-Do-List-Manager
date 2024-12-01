using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public DateTime Duedate { get; set; }
        public string TaskState { get; set; } // e.g., "Not Started", "In Progress", "Completed", or "None"
        public string TaskPriority { get; set; } // e.g., "Low", "Medium", "High", or "None"

        public string Recurrence { get; set; } // e.g., "None", "Daily", "Weekly", "Monthly"
       

        public ObservableCollection<TaskChecklist> TaskChecklist { get; set; }
    }
}
