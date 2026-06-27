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
using System.Windows.Shapes;

namespace StratusBot
{
    
    // Interaction logic for QuizWindow.xaml
    public partial class QuizWindow : Window
    {
        private readonly QuizManager _quizManager;
        private QuizQuestion _currentQuestion;
        private bool _answered = false;
        private RadioButton _selectedRadioButton;
        public QuizWindow()
        {
            InitializeComponent();
            _quizManager = new QuizManager();
            DisplayQuestion();
        }

        private void DisplayQuestion()
        {
            _answered = false;
            _selectedRadioButton = null;

            _currentQuestion = _quizManager.GetCurrentQuestion();

            if (_currentQuestion == null)
            {
                ShowResults();
                return;
            }

            // Update header
            QuestionNumberBlock.Text = $"Question {_quizManager.GetCurrentQuestionNumber()} of {_quizManager.GetTotalQuestions()}";
            TopicBlock.Text = $"Topic: {_currentQuestion.Topic}";
            ScoreBlock.Text = $"Score: {_quizManager.GetCurrentScore()}/{_quizManager.GetTotalQuestions()}";
            QuestionBlock.Text = _currentQuestion.Question;

            // Clear previous options
            OptionsPanel.Children.Clear();

            // Display options
            int optionIndex = 0;
            foreach (var option in _currentQuestion.Options)
            {
                RadioButton rb = new RadioButton
                {
                    Content = option,
                    FontSize = 12,
                    Margin = new Thickness(0, 10, 0, 0),
                    Padding = new Thickness(5),
                    GroupName = "QuizOptions",
                    Tag = optionIndex
                };

                // Store the index for answer mapping
                rb.Tag = optionIndex;
                OptionsPanel.Children.Add(rb);
                optionIndex++;
            }

            // Hide feedback and next/retake buttons
            FeedbackBorder.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
            RetakeButton.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Visible;
            SubmitButton.IsEnabled = true;
            ResultsPanel.Visibility = Visibility.Collapsed;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (_answered) return;

            // Get selected answer
            _selectedRadioButton = null;
            foreach (RadioButton rb in OptionsPanel.Children)
            {
                if (rb.IsChecked == true)
                {
                    _selectedRadioButton = rb;
                    break;
                }
            }

            if (_selectedRadioButton == null)
            {
                MessageBox.Show("Please select an answer!", "No Answer Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _answered = true;

            // Get the answer option (A, B, C, D or True/False)
            string answerOption = GetAnswerOption();

            // Submit answer
            bool isCorrect = _quizManager.SubmitAnswer(answerOption);

            // Show feedback
            string feedback = _quizManager.GetFeedback();
            FeedbackTitleBlock.Text = isCorrect ? "✓ Correct!" : "✗ Incorrect";
            FeedbackTitleBlock.Foreground = isCorrect ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            FeedbackTextBlock.Text = feedback;
            FeedbackBorder.BorderBrush = isCorrect ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            FeedbackBorder.Visibility = Visibility.Visible;

            // Disable selecting new answers
            foreach (RadioButton rb in OptionsPanel.Children)
            {
                rb.IsEnabled = false;
            }

            // Show next button, hide submit
            SubmitButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Visible;
            ScoreBlock.Text = $"Score: {_quizManager.GetCurrentScore()}/{_quizManager.GetTotalQuestions()}";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.IsFinished())
            {
                ShowResults();
            }
            else
            {
                DisplayQuestion();
            }
        }

        private void RetakeButton_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.ResetQuiz();
            ResultsPanel.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Visible;
            DisplayQuestion();
        }

        private void ShowResults()
        {
            OptionsPanel.Children.Clear();
            SubmitButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
            FeedbackBorder.Visibility = Visibility.Collapsed;

            string finalScore = _quizManager.GetFinalScore();
            string finalMessage = _quizManager.GetFinalMessage();
            double percentage = _quizManager.GetScorePercentage();

            FinalScoreBlock.Text = $"Final Score: {finalScore}";
            ResultsMessageBlock.Text = finalMessage;
            PercentageBlock.Text = $"{(int)percentage}%";

            // Color percentage based on score
            if (percentage >= 90)
                PercentageBlock.Foreground = new SolidColorBrush(Colors.Green);
            else if (percentage >= 70)
                PercentageBlock.Foreground = new SolidColorBrush(Colors.Orange);
            else
                PercentageBlock.Foreground = new SolidColorBrush(Colors.Red);

            ResultsPanel.Visibility = Visibility.Visible;
            RetakeButton.Visibility = Visibility.Visible;

            // Update header to show completion
            QuestionNumberBlock.Text = "Quiz Completed";
            TopicBlock.Text = $"Percentage: {(int)percentage}%";
        }

        private string GetAnswerOption()
        {
            if (_selectedRadioButton == null) return null;

            int optionIndex = (int)_selectedRadioButton.Tag;

            if (_currentQuestion.IsTrueFalse)
            {
                return _selectedRadioButton.Content.ToString(); // "True" or "False"
            }
            else
            {
                // Convert index to A, B, C, D
                char[] options = { 'A', 'B', 'C', 'D' };
                return options[optionIndex].ToString();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitQuiz();
        }

        // Exits the quiz with confirmation dialog
        private void ExitQuiz()
        {
            // Show confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to exit the quiz?\n\nYour current score is: {_quizManager.GetCurrentScore()}/{_quizManager.GetTotalQuestions()}\n\nYour progress will not be saved.",
                "Exit Quiz",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No
            );

            if (result == MessageBoxResult.Yes)
            {
                // Log quiz exit
                ActivityLog activityLog = new ActivityLog();
                activityLog.LogAction("QUIZ", $"Quiz exited - Final score: {_quizManager.GetCurrentScore()}/{_quizManager.GetTotalQuestions()}");

                // Close the window
                this.Close();
            }
        }
    }
}
