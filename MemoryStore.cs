using System;
using System.Security.Permissions;
using System.Collections.Concurrent;

public class MemoryStore
{
    private readonly ConcurrentDictionary<string, string> _store = new();
    public string UserName { get; private set; }
    public string FavouriteTopic { get; private set; }

    public void Store(string key, string value)
    {
        // Implementation for storing  any key-value pairs
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key must not be null or whitespace.", nameof(key));

        value ??= string.Empty;
        _store.AddOrUpdate(key, value, (_, __) => value);

        // Keep convenient properties in sync for commonly used keys
        if (string.Equals(key, nameof(UserName), StringComparison.OrdinalIgnoreCase))
            UserName = value;
        else if (string.Equals(key, nameof(FavouriteTopic), StringComparison.OrdinalIgnoreCase))
            FavouriteTopic = value;
    
    }

    public string Recall(string key)
    {
        // Implementation for retrieving stored value by key
        _store.TryGetValue(key, out string value);
        return value ?? string.Empty;
    }

    public void GetPersonalisedOpener()
    {
        // Implementation for generating a personalised opener based on stored information
        string opener = $"Hello {this.UserName}, it's great to see you again! How can I assist you today?";
        if (!string.IsNullOrEmpty(this.FavouriteTopic))
        {
            opener += $" I remember you mentioned that you're interested in {this.FavouriteTopic}. " +
                $"Would you like to talk more about that?";

        }

    }
}

