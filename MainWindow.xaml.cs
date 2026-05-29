using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatBot chatBot;   
        public MainWindow()
        {
            InitializeComponent();

            //play the welcome sound
            Sound sound = new Sound();

            chatBot = new ChatBot();

            
            //string greeting = chatBot.GetGreeting();
           

        }

  


        // Method to update the user status indicator in the UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Simulate user status (for demonstration purposes)
            bool isUserOnline = true; // This could be determined by actual logic in a real application
            UpdateUserStatus(isUserOnline);
        }

        private void  UpdateUserStatus(bool isOnline)
        {
            if (isOnline)
            {
                StatusIndicator.Fill = new SolidColorBrush(Colors.Green);
            }
            else
            {
                StatusIndicator.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void SendMessage()
        {
            //read the input from the UI and call the chat bot to send the message and display the response in the UI
            string userInput = InputTextBox.Text;
            string response = chatBot.ProcessInput(userInput);
        }

        //ScrollViewer ScrollChanged event handler to auto-scroll to the bottom when new messages are added
        private void ChatScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                ChatScrollViewer.ScrollToBottom();
            }
        }

        //UserInput KeyDown event handler to send message when Enter key is pressed
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true;
                // Clear the input box after sending the message
                InputTextBox.Clear();
            }
        }

        private void  AppendUserMessage(string message, bool isBot)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = message,
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap,
                Background = isBot ? Brushes.LightBlue : Brushes.LightGreen,
                HorizontalAlignment = isBot ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                MaxWidth = 300
            };
            ChatDisplay.Children.Add(textBlock);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the SendMessage method when the send button is clicked
            SendMessage();
            // read the user input and append it to the chat display
            string userInput = InputTextBox.Text;
            AppendUserMessage(userInput, false);

            // get the chatbot response and append it to the chat display
            string response = chatBot.ProcessInput(userInput);
            AppendUserMessage(response, true);

            // Clear the input box after sending the message
            InputTextBox.Clear();
        }
    }
}