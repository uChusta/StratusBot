using System;

public class ChatBot
{
    private KeywordResponder _keywords;
    private SentimentDetector _sentiment;
    private MemoryStore _memory;
    private bool _awaitingName = true;
    private string _lastTopic;
    private Random _random = new Random();
    private List<string> _fallbacks = new List<string>
    {
        "Interesting — tell me more.",
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
        _awaitingName = true;
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

        // 1) If awaiting name capture it and return welcome
        if (_awaitingName)
        {
            _memory.Store("UserName", input);
            _awaitingName = false;
            string name = string.IsNullOrEmpty(_memory.UserName) ? input : _memory.UserName;
            return $"Nice to meet you, {name}! What would you like to talk about today?";
        }

        // 2) Follow-up phrases -> return more on last topic
        if (!string.IsNullOrEmpty(_lastTopic))
        {
            if (inputLower.Contains("tell me more") || inputLower.Contains("explain more") || inputLower.Contains("more info") || inputLower.Contains("more about that"))
            {
                return $"Here's more about {_lastTopic}: {_lastTopic} — if you want deeper detail ask a specific question.";
            }
        }

        // 3) Sentiment detection (opener if not Neutral)
        Sentiment sentiment = _sentiment.Detect(input);
        string sentimentOpener = string.Empty;
        try
        {
            if (sentiment != Sentiment.Neutral)
            {
                // Reuse existing method to get a sentiment opener/response
                sentimentOpener = _sentiment.GetSentimentResponse(sentiment);
                if (!string.IsNullOrEmpty(sentimentOpener))
                    sentimentOpener += " ";
            }
        }
        catch
        {
            // If SentimentDetector doesn't support GetSentimentResponse or throws, ignore opener
            sentimentOpener = string.Empty;
        }

        // 5) Special phrases: "how are you", "what can you do", "purpose"
        if (inputLower.Contains("how are you"))
        {
            string resp = _keywords.GetResponse("how are you");
            if (!string.IsNullOrEmpty(resp))
                return (sentimentOpener + resp).Trim();
        }
        if (inputLower.Contains("what can you do") || inputLower.Contains("what do you do") || inputLower.Contains("purpose"))
        {
            return "I can provide cybersecurity tips, explain concepts like phishing or malware, and answer common security questions. Try asking 'what is phishing' or 'general tips'.";
        }

        // 4) Keyword responder
        string keywordResponse = _keywords.GetResponse(input);
        if (!string.IsNullOrEmpty(keywordResponse) && keywordResponse != "I'm sorry, I don't understand.")
        {
            // store the last topic as the response text for follow-up use
            _lastTopic = keywordResponse;
            return (sentimentOpener + keywordResponse).Trim();
        }

        // 6) Fallback to a random response
        string fallback = _fallbacks[_random.Next(_fallbacks.Count)];
        return (sentimentOpener + fallback).Trim();
    }
}
