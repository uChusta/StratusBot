using System;
using System.Collections.Generic;
using System.Windows.Input;

public class KeywordResponder
{
    private Dictionary<string, List<string>> _responses;
    private Random _random = new Random();

    public KeywordResponder()
    {
        _responses = new Dictionary<string, List<string>>
        {
            { "hello", new List<string> { "Hi there!", "Hello!", "Greetings!" } },
            { "hi", new List<string> { "Hi there!", "Hello!", "Greetings!" } },
            { "what's up", new List<string> { "Not much, how about you?", "Just here, what's new?" } },
            { "how are you", new List<string> { "I'm doing well, thank you!", "I'm fine, how about you?", "All good here!" } },
            { "bye", new List<string> { "Goodbye!", "See you later!", "Take care!" } },
            { "what can I ask", new List<string> { "You can ask me anything!", "I'm here to help!" } }
        };
    }

    public string GetResponse(string input)
    {
        foreach (var kvp in _responses)
        {
            if (input.Contains(kvp.Key))
            {
                return kvp.Value[_random.Next(kvp.Value.Count)];
            }
        }
        return "I'm sorry, I don't understand.";
    }
}
