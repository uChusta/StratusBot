using System;
using System.Security.Permissions;

public class MemoryStore
{
    public string UserName { get; set; }
    public string FavouriteTopic { get; set; }

    public void Store(string key, string value)
    {
        // Implementation for storing key-value pairs

    }

    public string Retrieve(string key)
    {
        // Implementation for retrieving value by key
        return string.Empty;
    }

    public void GetPersonalisedOpener()
    {
        // Implementation for generating a personalised opener based on stored information
    }
}

