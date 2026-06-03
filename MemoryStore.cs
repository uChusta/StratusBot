using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text.Json;

public class MemoryStore
{
    private readonly ConcurrentDictionary<string, string> _store = new();
    public string UserName { get; private set; }
    public string FavouriteTopic { get; private set; }

    private readonly string _filePath;

    //persistence
    public MemoryStore()
    {
        string appDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StratusBot");
        Directory.CreateDirectory(appDir);
        _filePath = Path.Combine(appDir, "memory.json");

        LoadFromFile();
    }

    //persistence
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

        //persistence
        try
        {
            SaveToFile();
        }
        catch
        {
            //ignore
        }
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
    //
    private void SaveToFile()
    {
        var model = new PersistenceModel
        {
            Store = new Dictionary<string, string>(_store),
            UserName = this.UserName,
            FavouriteTopic = this.FavouriteTopic
        };

        var opts = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(model, opts);

        // atomic write
        string tmp = _filePath + ".tmp";
        File.WriteAllText(tmp, json);
        File.Move(tmp, _filePath, true);
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string json = File.ReadAllText(_filePath);
            var model = JsonSerializer.Deserialize<PersistenceModel>(json);
            if (model is null)
                return;

            if (model.Store != null)
            {
                foreach (var kvp in model.Store)
                    _store[kvp.Key] = kvp.Value;
            }

            UserName = model.UserName ?? UserName;
            FavouriteTopic = model.FavouriteTopic ?? FavouriteTopic;
        }
        catch
        {
            // ignore load errors for now (or log as needed)
        }
    }
    
    private class PersistenceModel
    {
        public Dictionary<string, string> Store { get; set; }
        public string UserName { get; set; }
        public string FavouriteTopic { get; set; }
    }
}

