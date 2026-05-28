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

    public string GetGreeting()
    {
        return "Hello! I'm StratusBot, your friendly chatbot. What's your name?";
    }

    public string GetResponse(string input)
    {
        // Check if we're awaiting the user's name
        if (_awaitingName)
        {
            _memory.UserName = input;
            _awaitingName = false;
            return $"Nice to meet you, {_memory.UserName}! What topic are you interested in?";
        }
        // Check for keywords and get response
        string keywordResponse = _keywords.GetResponse(input);
        if (keywordResponse != "I'm sorry, I don't understand.")
        {
            return keywordResponse;
        }
        // Detect sentiment and get empathetic response
        Sentiment sentiment = _sentiment.Detect(input);
        string sentimentResponse = _sentiment.GetSentimentResponse(sentiment);
        // Store favourite topic if mentioned
        if (input.Contains("topic"))
        {
            _lastTopic = input; // Simplified for demonstration
            _memory.FavouriteTopic = _lastTopic;
            return $"{sentimentResponse} I see you're interested in {_lastTopic}. I'll remember that!";
        }
        return $"{sentimentResponse} I'm not sure how to respond to that. Can you tell me more?";
    }
}
