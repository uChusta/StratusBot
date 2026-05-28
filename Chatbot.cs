using System;

public class ChatBot
{
    private KeywordResponder _keywords;
    private SentimentDetector _sentiment;
    private MemoryStore _memory;
    private bool _awaitingName = true;
    private string _lastTopic;

}
