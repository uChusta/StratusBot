using System;
using System.Collections.Generic;

// Sentiment Detector
public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy, Sad }
public class SentimentDetector
{
    // dictionary mapping Sentiment -> list of trigger words
    private Dictionary<Sentiment, List<string>> _sentiment;

    public SentimentDetector()
    {
        // initialize the dictionary with trigger words for each sentiment
        _sentiment = new Dictionary<Sentiment, List<string>>
        {
            { Sentiment.Neutral, new List<string> { "okay", "fine", "alright" } },
            { Sentiment.Worried, new List<string> { "worried", "scared", "anxious" , "nervous" , "unsafe"  } },
            { Sentiment.Curious, new List<string> { "curious", "interested", "wondering" , "how does" , "want to know" } },
            { Sentiment.Frustrated, new List<string> { "frustrated", "annoyed", "confused" , "don't understand" } },
            { Sentiment.Happy, new List<string> { "great","thanks" , "awesome", "helpful", "love it" } },
            { Sentiment.Sad, new List<string> { "sad", "depressed", "lonely", "alone", "unhappy" } }
        };

    

    }

    // Detect(string input): loop through dictionary, return matching Sentiment
    public Sentiment Detect(string input)
    {
        foreach (var kvp in _sentiment)
        {
            foreach (var trigger in kvp.Value)
            {
                if (input != null && input.IndexOf(trigger, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return kvp.Key;
                }
            }
            
        }return Sentiment.Neutral;
    }

    // GetSentimentResponse(Sentiment s): return empathetic opening sentence
    public string GetSentimentResponse(Sentiment s)
    {
        switch (s)
        {
            case Sentiment.Worried:
                return "I understand that you're feeling worried. I'm here to help you.";
            case Sentiment.Curious:
                return "It's great that you're curious! What would you like to know?";
            case Sentiment.Frustrated:
                return "I'm sorry to hear that you're frustrated. Let's see if we can figure this out together.";
            case Sentiment.Happy:
                return "I'm glad to hear that you're happy! Is there anything else I can assist you with?";
            case Sentiment.Sad:
                return "I'm sorry to hear that you're sad. Let's see if we can figure this out together.";
            default:
                return "I'm here to help.";
        }
    }
}
