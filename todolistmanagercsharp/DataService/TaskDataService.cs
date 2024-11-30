using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolistmanagercsharp.DataService
{
    internal class TaskDataService
    {
        private readonly string _filePath;
        private readonly string folderName = "todolistmanagercsharp";
        private readonly string fileName = "task.json";

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

            InitializerFile();
        }

        private void InitializerFile()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Task>()));
            }

            // For Debug
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName));

        }


    }
}
