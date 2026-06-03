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
            
            // Load the ASCII art
            LoadAsciiArt();
            
            // Initialize the chatbot
            chatBot = new ChatBot();

            // Set the user status indicator to online
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
        
        // Method to send a message
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

            //send the message to the chatbot
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
            // Create a TextBlock for the message
            TextBlock textBlock = new TextBlock
            {
                Text = message,
                Margin = new Thickness(0),
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Black, 
                MaxWidth = 300
            };

            // Create a bubble for the message
            Border bubble = new Border
            {
                Background = isBot ? Brushes.LightBlue : Brushes.LightGreen,
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                Child = textBlock, 
                HorizontalAlignment = isBot ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                MaxWidth = 420
            };

            // Set the name of the user
            UserDisplay.Text = chatBot._memory.UserName ?? "Unknown";

            // Add the bubble to the chat display
            ChatDisplay.Children.Add(bubble);
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the SendMessage method when the send button is clicked
            SendMessage();

            // Clear the input box after sending the message
            InputTextBox.Clear();
        }

    }
}