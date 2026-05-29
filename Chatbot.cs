using System;

public class ChatBot
{
    private KeywordResponder _keywords;
    private SentimentDetector _sentiment;
    private MemoryStore _memory;
    private bool _awaitingName = true;
    private string _lastTopic;

    // Constructor to initialize components
    public ChatBot()
    {
        _keywords = new KeywordResponder();
        _sentiment = new SentimentDetector();
        _memory = new MemoryStore();
    }

    // Method to get the initial greeting message
    public string GetGreeting()
    {
        if (_awaitingName)
        {
            // Return the greeting message
            return "Hello! I'm StratusBot, your friendly chatbot. What's your name?";
        }
        return string.Empty;
    }
    // Method to process user input and generate a response
    public string ProcessInput(string input)
    {
        if (_awaitingName)
        {
            _memory.Store("UserName", input);
            _awaitingName = false;
            return $"Nice to meet you, {_memory.UserName}! What would you like to talk about today?";
        }
        // Check for keywords
        string keywordResponse = _keywords.GetResponse(input);
        if (!string.IsNullOrEmpty(keywordResponse))
        {
            _lastTopic = keywordResponse; // Store the last topic for follow-up
            return keywordResponse;
        }
        // Detect sentiment
        Sentiment sentiment = _sentiment.Detect(input);
        string sentimentResponse = _sentiment.GetSentimentResponse(sentiment);
        // Combine responses
        return $"{sentimentResponse} {keywordResponse}".Trim();
    }
}
