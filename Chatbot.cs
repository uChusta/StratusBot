using StratusBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public class ChatBot
{
    public KeywordResponder _keywords;
    private SentimentDetector _sentiment;
    public MemoryStore _memory;
    private TaskManager _taskManager;
    private ActivityLog _activityLog;
    private QuizManager _quizManager;
    private bool _awaitingName = true;
    private bool _quizActive = false;//tracks if quiz is active.
    private bool _showingFullLog = false; 
    private string _lastTopic;
    private Random _random = new Random();

    //event for opening quiz window
    public event EventHandler QuizStartRequested;


    // Intent phrase definitions
    private readonly Dictionary<string, string[]> _intentPhrases = new Dictionary<string, string[]>
    {
        { "AddTask", new[] { "add task", "add a task", "create task", "i need to", "enable", "set up" } },
        { "ViewTasks", new[] { "show tasks", "view tasks", "list tasks", "my tasks", "all tasks", "see tasks", "display tasks" } },
        { "CompleteTask", new[] { "complete task", "mark complete", "finish task", "done with task", "task done" } },
        { "DeleteTask", new[] { "delete task", "remove task", "remove", "delete", "get rid of" } },
        { "SetReminder", new[] { "remind me", "reminder", "set a reminder", "remind me to", "don't forget" } },
        { "StartQuiz", new[] { "start quiz", "take quiz", "test my knowledge", "quiz me", "play the game" } },
        { "ShowLog", new[] { "show activity log", "what have you done", "what did you do", "show log", "recent actions" } },
        { "ShowMore", new[] { "show more", "more logs", "full history", "all logs", "complete history" } },
        { "Cybersecurity", new[] { "password", "phishing", "privacy", "scam", "malware", "2fa" } },
        { "QuitQuiz", new[] { "quit quiz", "exit quiz", "stop quiz", "end quiz", "quit" } }
    };

    // Fallback responses for when no keywords or sentiment match
    private List<string> _fallbacks = new List<string>
    {
        "I don't have a ready answer for that, but I can help with cybersecurity topics.",
        "Could you rephrase that?",
        "I might not understand fully; try asking about phishing, malware, or passwords."
    };

    // Constructor to initialize components
    public ChatBot()
    {
        _keywords = new KeywordResponder();
        _sentiment = new SentimentDetector();
        _memory = new MemoryStore();
        _taskManager = new TaskManager();
        _activityLog = new ActivityLog();
        _quizManager = new QuizManager();
        _awaitingName = true;
        _quizActive = false;
    }

    // Property to access the memory store
    public MemoryStore Memory
    {
        get { return _memory; }
    }

    //Property to check if quiz is active
    public bool IsQuizActive
    {
        get { return _quizActive;}
    }

    // Method to get the initial greeting message
    public string GetGreeting()
    {
            // Return the greeting message
            return "Hello! I'm StratusBot, your friendly chatbot. What's your name?";

    }
    // Method to process user input and generate a response
    public string ProcessInput(string input)
    {

        string inputLower = input.ToLowerInvariant();

        //If awaiting name capture it and return welcome
        if (_awaitingName)
        {
            _memory.Store("UserName", input);
            _awaitingName = false;
            string name = string.IsNullOrEmpty(_memory.UserName) ? input : _memory.UserName;
            _activityLog.LogAction("INFO", $"User joined: {name}");
            return $"Nice to meet you, {name}! What would you like to talk about today?";
        }

        //Quiz answer recognition
        if (_quizActive)
        {
            // Check if user wants to quit quiz
            if (DetectIntent("QuitQuiz", inputLower))
            {
                return HandleQuitQuiz();
            }

            // Recognize quiz answers: A, B, C, D, True, False
            string quizAnswer = RecognizeQuizAnswer(input);
            if (!string.IsNullOrEmpty(quizAnswer))
            {
                return HandleQuizAnswer(quizAnswer, input);
            }

            // If quiz is active but input is not a valid answer
            return PromptForQuizAnswer();
        }

        //favourite topic
        if (!string.IsNullOrEmpty(_memory.FavouriteTopic))
        {
            _lastTopic = _memory.FavouriteTopic;
        }

        // ============= STEP 1: Check for Add Task Intent =============
        if (DetectIntent("AddTask", inputLower))
        {
            return HandleAddTask(input);
        }
        // ============= STEP 2: Check for View Tasks Intent =============
        if (DetectIntent("ViewTasks", inputLower))
        {
            return HandleViewTasks();
        }

        // ============= STEP 3: Check for Set Reminder Intent =============
        if (DetectIntent("SetReminder", inputLower))
        {
            return HandleSetReminder(input);
        }

        // ============= STEP 4: Check for Start Quiz Intent =============
        if (DetectIntent("StartQuiz", inputLower))
        {
            return HandleStartQuiz();
        }

        // ============= STEP 5: Check for Show Activity Log Intent =============
        if (DetectIntent("ShowLog", inputLower))
        {
            _showingFullLog = false;
            return HandleShowLog();
        }

        // ============= STEP 6: Check for Cybersecurity Topics Intent =============
        if (DetectIntent("Cybersecurity", inputLower))
        {
            return HandleCybersecurityTopic(input);
        }

        // ============= STEP 7: Check for Complete Task Intent =============
        if (DetectIntent("CompleteTask", inputLower))
        {
            return HandleCompleteTask(input);
        }

        // ============= STEP 8: Check for Delete Task Intent =============
        if (DetectIntent("DeleteTask", inputLower))
        {
            return HandleDeleteTask(input);
        }

        //Follow-up phrases -> return more on last topic
        if (!string.IsNullOrEmpty(_lastTopic))
        {
            string followUpResponse = DetectFollowUp(inputLower);
            if (!string.IsNullOrEmpty(followUpResponse))
            {
                return followUpResponse;
            }
        }

        //Sentiment detection (opener if not Neutral)
        Sentiment sentiment = _sentiment.Detect(input);
        string sentimentOpener = string.Empty;
        try
        {
            if (sentiment != Sentiment.Neutral)
            {
                //Reuse existing method to get a sentiment opener/response
                sentimentOpener = _sentiment.GetSentimentResponse(sentiment);
                if (!string.IsNullOrEmpty(sentimentOpener))
                    sentimentOpener += " ";
            }
        }
        catch
        {
            //If SentimentDetector doesn't support GetSentimentResponse or throws, ignore opener
            sentimentOpener = string.Empty;
        }

        //Special phrases: "how are you", "what can you do", "purpose"
        if (inputLower.Contains("how are you"))
        {
            string resp = _keywords.GetResponse("how are you");
            if (!string.IsNullOrEmpty(resp))
                _activityLog.LogAction("INTERACTION", "NLP recognised greeting: 'how are you'");
                return (sentimentOpener + resp).Trim();
        }
        if (inputLower.Contains("what can you do") || inputLower.Contains("what do you do") || inputLower.Contains("purpose"))
        {
            _activityLog.LogAction("INTERACTION", "NLP recognised query: 'what can you do'");
            return "I can provide cybersecurity tips, explain concepts like phishing or malware, and answer common security questions. Try asking 'what is phishing' or 'general tips'.";
        }

        //Keyword responder
        string keywordResponse = _keywords.GetResponse(input);
        if (!string.IsNullOrEmpty(keywordResponse) && keywordResponse != "I'm sorry, I don't understand.")
        {
            //store the last topic as the response text for follow-up use
            _lastTopic = keywordResponse;
            _activityLog.LogAction("KEYWORD", $"Keyword matched: {ExtractKeyword(input)} - response delivered");
            return (sentimentOpener + keywordResponse).Trim();
        }
        //Rephrase or ask a more specific question
        if (!string.IsNullOrWhiteSpace(input))
        {
            _activityLog.LogAction("INTERACTION", $"NLP unmatched input: '{input}'");
            string clarification = "Could you rephrase that or ask a more specific question?";
            return (sentimentOpener + clarification).Trim();
        }

        //Default response if no other conditions match
        return (sentimentOpener + _fallbacks[_random.Next(_fallbacks.Count)]).Trim();
    }

    //=========================Quiz Helper methods================================================

    // Recognizes if user input is a valid quiz answer (A, B, C, D, True, False)
    private string RecognizeQuizAnswer(string input)
    {
        string trimmed = input.Trim().ToUpperInvariant();

        // Single letter answers
        if (trimmed.Length == 1 && "ABCDF".Contains(trimmed))
        {
            return trimmed;
        }

        // True/False answers
        if (trimmed == "TRUE" || trimmed == "T")
        {
            return "True";
        }
        if (trimmed == "FALSE" || trimmed == "F")
        {
            return "False";
        }

        return string.Empty;
    }

    // Handles quiz answer submission
    private string HandleQuizAnswer(string answer, string rawInput)
    {
        try
        {
            // Submit answer to QuizManager
            bool isCorrect = _quizManager.SubmitAnswer(answer);

            // Get feedback
            string feedback = _quizManager.GetFeedback();
            string resultMessage;

            if (isCorrect)
            {
                resultMessage = $"**Correct!** {feedback}";
                _activityLog.LogAction("QUIZ", $"Quiz answer submitted: {answer} - CORRECT");
            }
            else
            {
                resultMessage = $"**Incorrect.** {feedback}";
                _activityLog.LogAction("QUIZ", $"Quiz answer submitted: {answer} - INCORRECT");
            }
            // Check if quiz is finished
            if (_quizManager.IsFinished())
            {
                return HandleQuizCompletion(resultMessage);
            }

            // Show next question
            QuizQuestion nextQuestion = _quizManager.GetCurrentQuestion();
            if (nextQuestion != null)
            {
                resultMessage += "\n\n" + FormatQuizQuestion(nextQuestion);
            }

            return resultMessage;
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to process quiz answer: {ex.Message}");
            return "Sorry, I couldn't process your answer. Please try again.";
        }
    }

    //handles quiz completion
    private string HandleQuizCompletion(string resultMessage)
    {
        try
        {
            string finalScore = _quizManager.GetFinalScore();
            string finalMessage = _quizManager.GetFinalMessage();
            double percentage = _quizManager.GetScorePercentage();

            _quizActive = false;
            _activityLog.LogAction("QUIZ", $"Quiz completed - score: {finalScore} ({percentage:F1}%)");

            string completionMessage = $"{resultMessage}\n\n";
            completionMessage += $"**Quiz Complete!**\n";
            completionMessage += $"**Final Score:** {finalScore}\n";
            completionMessage += $"**Percentage:** {percentage:F1}%\n\n";
            completionMessage += $"{finalMessage}\n\n";
            completionMessage += "Want to take another quiz? Type 'start quiz' to try again!";

            return completionMessage;
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to complete quiz: {ex.Message}");
            _quizActive = false;
            return "Quiz completed. Type 'start quiz' to try another one!";
        }
    }
    //prompts user for valid quiz answer
    private string PromptForQuizAnswer()
    {
        QuizQuestion currentQ = _quizManager.GetCurrentQuestion();
        if (currentQ != null)
        {
            string prompt = "Please enter your answer (";
            if (currentQ.IsTrueFalse)
            {
                prompt += "True or False";
            }
            else
            {
                prompt += "A, B, C, or D";
            }
            prompt += ")";
            return prompt;
        }
        return "Invalid answer. Please select A, B, C, D or True/False.";
    }

    //Handles quiz quit/exit
    private string HandleQuitQuiz()
    {
        try
        {
            string currentScore = _quizManager.GetFinalScore();
            _quizActive = false;
            _activityLog.LogAction("QUIZ", $"Quiz exited - Final score: {currentScore}");
            return $"Quiz ended. Your score was {currentScore}. Type 'start quiz' to try again!";
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to quit quiz: {ex.Message}");
            _quizActive = false;
            return "Quiz ended. What would you like to do next?";
        }
    }

    // ============= TASK MANAGEMENT METHODS =============

    //Handle View tasks and displays all tasks
    private string HandleViewTasks()
    {
        try
        {
            var tasks = _taskManager.GetAllTasks();
            _activityLog.LogAction("TASK", "User viewed all tasks");

            if (tasks == null || tasks.Count == 0)
            {
                return "📋 **No tasks yet!** Type 'add task' to create a new one.";
            }

            return FormatTasksForDisplay(tasks);
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to retrieve tasks: {ex.Message}");
            return "Sorry, I couldn't retrieve your tasks. Try again later.";
        }
    }

    //Handles complete Task
    private string HandleCompleteTask(string input)
    {
        // Extract task ID or name from input
        string taskReference = ExtractTaskReference(input);

        if (string.IsNullOrWhiteSpace(taskReference))
        {
            return "Which task would you like to mark as complete? (e.g., 'complete task 1' or 'mark task 2 as done')";
        }

        try
        {
            if (int.TryParse(taskReference, out int taskId))
            {
                _taskManager.MarkAsComplete(taskId);
                _activityLog.LogAction("TASK", $"Task {taskId} marked as complete");
                return $"✓ Task {taskId} marked as complete!";
            }
            else
            {
                return $"Please provide a task ID number. (e.g., 'complete task 1')";
            }
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to complete task: {ex.Message}");
            return "Sorry, I couldn't mark that task as complete.";
        }
    }

    //Handle delete Task
    private string HandleDeleteTask(string input)
    {
        string taskReference = ExtractTaskReference(input);

        if (string.IsNullOrWhiteSpace(taskReference))
        {
            return "Which task would you like to delete? (e.g., 'delete task 1' or 'remove task 2')";
        }

        try
        {
            if (int.TryParse(taskReference, out int taskId))
            {
                _taskManager.DeleteTask(taskId);
                _activityLog.LogAction("TASK", $"Task {taskId} deleted");
                return $"✓ Task {taskId} has been deleted.";
            }
            else
            {
                return "Please provide a task ID number. (e.g., 'delete task 1')";
            }
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to delete task: {ex.Message}");
            return "Sorry, I couldn't delete that task.";
        }
    }

    //Format tasks for display
    private string FormatTasksForDisplay(List<Cybertask> tasks)
    {
        if (tasks == null || tasks.Count == 0)
        {
            return "📋 **No tasks yet!** Type 'add task' to create a new one.";
        }

        string output = "📋 **Your Tasks:**\n\n";

        var incompleteTasks = tasks.Where(t => !t.IsComplete).ToList();
        var completedTasks = tasks.Where(t => t.IsComplete).ToList();

        // Display incomplete tasks
        if (incompleteTasks.Count > 0)
        {
            output += "**⏳ Active Tasks:**\n";
            foreach (var task in incompleteTasks)
            {
                output += $" {task.Id}. ☐ **{task.Title}**\n";
                if (!string.IsNullOrEmpty(task.Description))
                    output += $"    Description: {task.Description}\n";
                if (!string.IsNullOrEmpty(task.Reminder))
                    output += $"    Reminder: {task.Reminder}\n";
                output += $"    Created: {task.CreatedAt}\n";
            }
            output += "\n";
        }

        // Display completed tasks
        if (completedTasks.Count > 0)
        {
            output += "**✅ Completed Tasks:**\n";
            foreach (var task in completedTasks)
            {
                output += $" {task.Id}. ✓ ~~{task.Title}~~\n";
            }
            output += "\n";
        }

        output += "**Commands:**\n";
        output += " • 'complete task 1' - Mark task as done\n";
        output += " • 'delete task 2' - Remove a task\n";
        output += " • 'add task ...' - Add a new task\n";

        return output;
    }

    //===HELPER METHODS=====

    //Detects if user input contains any phrases that give intent
    private bool DetectIntent(string intentKey, string inputLower)
    {
      if (_intentPhrases.TryGetValue(intentKey, out var phrases))
      {
        return phrases.Any(phrase => inputLower.Contains(phrase));
      }
      return false;
    }

    //handles "Add Task" intent
    private string HandleAddTask(string input)
    {
        string taskName = ExtractTaskName(input);

        if (string.IsNullOrWhiteSpace(taskName))
        {
            return "Could you tell me what task you'd like to add? (e.g., 'add task review passwords')";
        }

        try
        {
            string result = _taskManager.AddTask(taskName, $"Added via chat", "");
            _memory.Store("LastTask", taskName);
            _activityLog.LogAction("Task", $"Add Task intent detected: '{taskName}'");
            return $"{result} Would you like to set a reminder for this?";
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to add task: {ex.Message}");
            return $"Sorry, I couldn't add that task. Try again later.";
        }
    }
    //Handles "Set Reminder" intent
    private string HandleSetReminder(string input)
    {
        string reminderText = ExtractReminderText(input);

        if (string.IsNullOrWhiteSpace(reminderText))
        {
            return "What would you like to be reminded about? (e.g., 'remind me to update my password')";
        }

        try
        {
            // get the date if provided or use current date
            string reminderDate = ExtractReminderDate(input);
            if (!string.IsNullOrEmpty(reminderDate))
            {
                reminderDate = DateTime.UtcNow.ToString("dd MMM yyyy"); 
            }

            _memory.Store("LastReminder", reminderText);
            _activityLog.LogAction("REMINDER", $"Set Reminder intent detected: '{reminderText}'");
            return $"Reminder set: {reminderText}";
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to set reminder: {ex.Message}");
            return $"Sorry, I couldn't set that reminder. Try again later.";
        }
    }

    //Handles "Start Quiz" intent
    private string HandleStartQuiz()
    {
        try
        {
            _quizActive = true;
            _activityLog.LogAction("QUIZ", "Start Quiz detected");

            //raise event for QuizWindow
            QuizStartRequested?.Invoke(this, EventArgs.Empty);
            return "**Quiz Window Opening!** Your cybersecurity quiz is loading...";
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to start quiz: {ex.Message}");
            _quizActive = false;
            return "Sorry, I couldn't start the quiz. Try again later.";
        }
    }

    //handles "Show Actvity log" intent
    private string HandleShowLog()
    {
        try
        {
            _activityLog.LogAction("LOG", "Activity log viewed");
            return _activityLog.GetFormattedRecentLogs(5);
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to retrieve logs: {ex.Message}");
            return "Sorry, I couldn't retrieve the activity log. Try again later.";
        }
    }

    //Show more
    private string HandleShowMoreLogs()
    {
        try
        {
            _showingFullLog = true;
            _activityLog.LogAction("LOG", "Full activity history requested");
            return _activityLog.GetFormattedAllLogs();
        }
        catch (Exception ex)
        {
            _activityLog.LogAction("ERROR", $"Failed to retrieve full log history: {ex.Message}");
            return "Sorry, I couldn't retrieve the full activity history. Try again later.";
        }
    }


    // handles Cybersecurity Topics
    private string HandleCybersecurityTopic(string input)
    {
        _activityLog.LogAction("INTERACTION", $"Cybersecurity topic detected: {input}");

        string keywordResponse = _keywords.GetResponse(input);
        if (!string.IsNullOrEmpty(keywordResponse))
        {
            _lastTopic = keywordResponse;
            _activityLog.LogAction("KEYWORD", $"Keyword matched: {_keywords} - response delivered");
            return keywordResponse;
        }

        return "I can help with cybersecurity topics like phishing, malware, passwords, privacy, scams, and 2FA. What would you like to know?";
    }

    //Detects follow-up phrases for the last topic
    private string DetectFollowUp(string inputLower)
    {
        string[] followUpPhrases = { "tell me more", "explain more", "more info", "more about that", "details", "elaborate" };

        if (followUpPhrases.Any(phrase => inputLower.Contains(phrase)))
        {
            _activityLog.LogAction("INTERACTION", "NLP recognised follow-up request");
            return $"Here's more about {_lastTopic}: {_lastTopic} — if you want deeper detail, ask a specific question.";
        }

        return string.Empty;
    }

    //get opening sentiment text
    private string GetSentimentOpener(Sentiment sentiment)
    {
        try
        {
            if (sentiment != Sentiment.Neutral)
            {
                string opener = _sentiment.GetSentimentResponse(sentiment);
                return string.IsNullOrEmpty(opener) ? "" : opener + " ";
            }
        }
        catch { /* Silent fail */ }
        return string.Empty;
    }

    private string ExtractTaskReference(string userInput)
    {
        // Extract task ID or number from input like "complete task 1" or "delete task 2"
        string inputLower = userInput.ToLowerInvariant();

        // Remove common phrases
        inputLower = inputLower.Replace("complete task", "").Replace("mark", "").Replace("done", "")
                                .Replace("delete task", "").Replace("remove task", "").Replace("remove", "")
                                .Replace("task", "").Trim();

        // Extract any remaining numbers
        string numbers = new string(inputLower.Where(char.IsDigit).ToArray());
        return numbers;
    }

    //Extracts task name by removing intent phrase
    private string ExtractTaskName(string input)
    {
        string extracted = input.ToLowerInvariant();
        foreach (var phrase in _intentPhrases["AddTask"])
        {
            extracted = extracted.Replace(phrase, "").Trim();
        }
        return extracted;
    }

    //extracts reminder tetx by removing intent phrases
    private string ExtractReminderText(string input)
    {
        string extracted = input.ToLowerInvariant();
        foreach (var phrase in _intentPhrases["SetReminder"])
        {
            extracted = extracted.Replace(phrase, "").Trim();
        }
        return extracted;
    }


    // Extracts date from reminder text if provided
    private string ExtractReminderDate(string input)
    {
        
        //return empty to use current date
        return string.Empty;
    }


    /// Extracts the main keyword from user input
    private string ExtractKeyword(string input)
    {
        string[] keywords = { "password", "phishing", "privacy", "scam", "malware", "2fa" };
        foreach (var kw in keywords)
        {
            if (input.ToLowerInvariant().Contains(kw))
                return kw;
        }
        return "unknown";
    }
    //formats quiz question for display
    private string FormatQuizQuestion(QuizQuestion question)
    {
        string formatted = $" **Question {_quizManager.GetCurrentQuestionNumber()} of {_quizManager.GetTotalQuestions()}:**\n";
        formatted += $"{question.Question}\n";

        if (question.IsTrueFalse)
        {
            formatted += "A) True\nB) False\n";
        }
        else if (question.Options != null && question.Options.Count > 0)
        {
            for (int i = 0; i < question.Options.Count; i++)
            {
                formatted += $"{(char)('A' + i)}) {question.Options[i]}\n";
            }
        }

        formatted += $"\nYour score: {_quizManager.GetCurrentScore()}/{_quizManager.GetTotalQuestions()}";
        return formatted;
    }
}
