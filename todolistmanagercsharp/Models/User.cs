using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolistmanagercsharp.Models
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
    }
}