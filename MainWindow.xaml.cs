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
using System.Windows;

namespace StratusBot
{

    public partial class MainWindow : Window
    {
        private ChatBot chatBot;


        public MainWindow()
        {
            InitializeComponent();

            LoadAsciiArt();

            chatBot = new ChatBot();

            AppendUserMessage(chatBot.GetGreeting(), true);


            //play the welcome sound
            Sound sound = new Sound();
            sound.PlaySound();
        }

        private void LoadAsciiArt()
        {
            // Load the ASCII art

            AsciiTextBlock.Text = @"
             ████ █████ ████   ███  █████ █   █  ████ ████   ███  █████ 
            █       █   █   █ █   █   █   █   █ █     █   █ █   █   █   
             ███    █   ████  █████   █   █   █  ███  ████  █   █   █   
                █   █   █  █  █   █   █   █   █     █ █   █ █   █   █   
            ████    █   █   █ █   █   █    ███  ████  ████   ███    █ ";  
            
        }


        // Method to update the user status indicator in the UI
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
            SendMessageInternal();
        }

        private void SendMessageInternal()
        {
            //read the input from the UI and call the chat bot to send the message and display the response in the UI
            string userInput = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(userInput))
                return; // Don't send empty messages
            AppendUserMessage(userInput, false);

            //update the user status indicator
            UpdateUserStatus(true);

            //check if the user's input contains a specific keyword
           bool isKeyword = userInput.Contains(chatBot._keywords.GetAllKeywords(), StringComparison.OrdinalIgnoreCase);


            string response = chatBot.ProcessInput(userInput);
            AppendUserMessage(response, true);

            InputTextBox.Clear();
            InputTextBox.Focus();


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
            // Call the SendMessage method when the Enter key is pressed
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
            InputTextBox.Clear();
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

            UserDisplay.Text = chatBot._memory.UserName ?? "Unknown";

            ChatDisplay.Children.Add(textBlock);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the SendMessage method when the send button is clicked
            SendMessage();

            // Clear the input box after sending the message
            InputTextBox.Clear();
        }

        // Method to update the user status indicator in the UI

    }
}