using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StratusBot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLog _activitylog;

        //initialise TaskStorageHelper
        public TaskManager()
        {
            _storage = new TaskStorageHelper();
            _activitylog = new ActivityLog();

        }


        public string AddTask(string title, string description, string reminder)
        {
            try
            {
                //call AddTask
                _storage.AddTask(title, description, reminder);

                //log action
                _activitylog.LogAction("TASK", $"Task Added: '{title}'");

                //conformation message
                return $"Task '{title}' has been added successfully.";
            }
            catch (Exception ex)
            {
                string errorMessage = $"Failed to add task: {ex.Message}";
                _activitylog.LogAction("ERROR", errorMessage);
                return $"{errorMessage}";
            }
        }
        public List<Cybertask> GetAllTasks()
        {
            return _storage.GetAlltasks();
        }
        public void MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);
            _activitylog.LogAction("TASK", $"Task {id} marked as complete");
        }
        public void DeleteTask(int id)
        {
            _storage.DeleteTask(id);
            _activitylog.LogAction("TASK", $"Task {id} deleted");
        }
    }
}
