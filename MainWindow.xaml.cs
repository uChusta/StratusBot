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

    public partial class MainWindow : Window
    {
        private ChatBot chatBot;
        private double _originalWidth;
        private double _originalHeight;


        public MainWindow()
        {
            InitializeComponent();

            // Set the initial size of the window
            _originalWidth = this.Width;
            _originalHeight = this.Height;
            
            // Initialize the chatbot
            chatBot = new ChatBot();

            //Quiz start event
            chatBot.QuizStartRequested += ChatBot_QuizStartRequested;

            // Set the user status indicator to online
            AppendUserMessage(chatBot.GetGreeting(), true);

            //play the welcome sound
            Sound sound = new Sound();
            sound.PlaySound();
        }

        // Event handler for when quiz is requested - opens the QuizWindow
        private void ChatBot_QuizStartRequested(object? sender, EventArgs e)
        {
            try
            {
                //resize the window
                this.Width = 800;
                this.Height = 600;

                // Create and open QuizWindow
                QuizWindow quizWindow = new QuizWindow();
                quizWindow.Owner = this; // Set MainWindow as owner
                quizWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                //handling window size when quiz is closed
                quizWindow.Closed += (s, args) =>
                {
                    // Notify chatbot that quiz has ended
                    chatBot.CloseQuiz();

                    //restore the window size
                    this.Width = _originalWidth;
                    this.Height = _originalHeight;
                };


                quizWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Quiz Window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            string input = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(input))
                return; // Don't send empty messages
            AppendUserMessage(input, false);

            //update the user status indicator
            UpdateUserStatus(true);

            //check if the user's input contains a specific keyword
           bool isKeyword = input.Contains(chatBot._keywords.GetAllKeywords(), StringComparison.OrdinalIgnoreCase);

            //send the message to the chatbot
            string response = chatBot.ProcessInput(input);
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
                e.Handled = true;
            }
          ;
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


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();
        }

    }
}