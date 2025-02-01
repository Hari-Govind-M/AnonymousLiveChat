using System;
using System.Collections.Generic;
using System.Linq;

public class MessageStore
{
    private readonly List<ChatMessage> _messages = new List<ChatMessage>();
    private readonly object _lock = new object();

    public void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _messages.Add(message);
            // Remove any messages older than 24 hours.
            _messages.RemoveAll(m => (DateTime.UtcNow - m.Timestamp).TotalHours >= 24);
        }
    }

    public IEnumerable<ChatMessage> GetMessages()
    {
        lock (_lock)
        {
            // Return a copy of the message list.
            return _messages.ToList();
        }
    }
}
