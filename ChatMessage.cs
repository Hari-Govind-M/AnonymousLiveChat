using System;

public class ChatMessage
{
    // Unique message identifier.
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    // The unique ID of the sender (client).
    public string SenderId { get; set; } = string.Empty;
    
    // The display name for this message (a random sci‑fi name).
    public string DisplayName { get; set; } = string.Empty;
    
    // The message content.
    public string Message { get; set; } = string.Empty;
    
    // Timestamp for when the message was sent.
    public DateTime Timestamp { get; set; }
    
    // New property: the ID of the message being replied to.
    // This will be null if the message is not a reply.
    public string? ReplyToMessageId { get; set; }
}
