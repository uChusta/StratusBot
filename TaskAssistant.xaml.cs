using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StratusBot
{
    /// <summary>
    /// Interaction logic for TaskAssistant.xaml
    /// </summary>
    public partial class TaskAssistant : UserControl
    {
        private readonly TaskManager _taskManager; 
        public TaskAssistant()
        {
            InitializeComponent();
            _taskManager = new TaskManager();
            LoadTasks();
        }

        private void LoadTasks()
        {
            //
            var tasks = _taskManager.GetAllTasks() ?? new List<Cybertask>();
            var view = tasks.Select(t => new TaskDisplay
            {
                Id = t.Id,
                Title = t.Title,
                Description =t.Description,
                Reminder=t.Reminder,    
                IsCompleted= t.IsComplete
            }).ToList();

            LstTasks.ItemsSource = view;
        }
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = TxtTitle.Text?.Trim() ?? string.Empty;
            string description = TxtDescription.Text?.Trim()?? string.Empty;
            string reminder = TxtReminder.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Taskmanager.AddTask returns a confirmation message
            _taskManager.AddTask(title, description, reminder);

            //Show confrimation message to user then clear inputs and reload list
            MessageBox.Show("Task added succesfully","Add Task", MessageBoxButton.OK,MessageBoxImage.Information);
            TxtTitle.Clear();
            TxtDescription.Clear();
            TxtReminder.Clear();
            LoadTasks();
        }

        private void BtnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (LstTasks.SelectedItem is TaskDisplay selected)
            {
                _taskManager.MarkAsComplete(selected.Id);
                LoadTasks();
                MessageBox.Show($"Task '{selected.Id}' marked as complete.", "Task Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Select a task to mark a complete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (LstTasks.SelectedItem is TaskDisplay selected)
            {
               
                var res = MessageBox.Show($"Delete task '{selected.Id}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    _taskManager.MarkAsComplete(selected.Id);
                    LoadTasks();
                }
            }
            else
            {
                MessageBox.Show("Select a task to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private class TaskDisplay
        {
            public int Id { get; set;  }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Reminder { get; set; }
            public bool IsCompleted { get; set; }

        }
    }
}
