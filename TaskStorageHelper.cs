using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Newtonsoft.Json;



namespace StratusBot
{
    public class Cybertask
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; }
    }


    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";
        private List<Cybertask> LoadTasks()
        {
            try
            {
                //if file does not exist
                if (!File.Exists(FilePath))
                {
                    return new List<Cybertask>();
                }
               
                //read tasks.json
                string jsonString = File.ReadAllText(FilePath); 
                List<Cybertask> tasks = JsonConvert.DeserializeObject<List<Cybertask>>(jsonString);

                //return empty list if there are no tasks
                return tasks ?? new List<Cybertask>();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading tasks: {ex.Message}");
                //Console.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<Cybertask>();
            }
        }
        private void SaveTasks(List<Cybertask> tasks) 
        {
            try
            {
           
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving Task: {ex.Message}");
            }
        }
        private void AddTask( string title, string description, string reminder)
        {
            try
            {
                //Display current tasks
                var tasks = LoadTasks();

                //finds the largets existing Id and sets newId to the value plus 1
                int newId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;

                //Add new task to CyberTask
                var task = new Cybertask
                {
                    Id = newId,
                    Title = title,
                    Description = description,
                    Reminder = reminder,
                    IsComplete = false,
                    CreatedAt = DateTime.UtcNow.ToString("o"), //created uses DateTime instead of string
                };
                //update list
                tasks.Add(task);
                SaveTasks(tasks);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding task: {ex.Message}");
            }

        }
        private void MarkAsComplete(int id)
        {
            try
            {
                //Display current tasks
                var tasks = LoadTasks();
                if (File.Exists(FilePath))
                {
                    string jsonString = File.ReadAllText(FilePath);
                    tasks = JsonConvert.DeserializeObject<List<Cybertask>>(jsonString);
                }

                //Find task to mark as complete
                var task = tasks.FirstOrDefault(task => task.Id == id);
                if (task != null && !task.IsComplete)
                {
                    task.IsComplete = true;
                    SaveTasks(tasks);
                    //change and save to file
                   // string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                    //File.WriteAllText(FilePath, json);
                }
            }
            catch(Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine($"Error marking task complete: {ex.Message}");
            }
        }
        private void DeleteTask(int id) 
        {
            try
            {
                //calling LoadTasks()
                var tasks = LoadTasks();

                //remove task using Id and  save
                int removed = tasks.RemoveAll(t => t.Id == id);
                if (removed > 0)
                {
                    SaveTasks(tasks);
                }

            }
            catch( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting task: {ex.Message}");
            }
        }
    }
}
