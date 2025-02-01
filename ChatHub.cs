using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly MessageStore _messageStore;

    public ChatHub(MessageStore messageStore)
    {
        _messageStore = messageStore;
    }

    // Sends a message without reply functionality.
    public async Task SendMessage(string senderId, string displayName, string message)
    {
        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            DisplayName = displayName,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        _messageStore.AddMessage(chatMessage);

        // Broadcast the message to all connected clients.
        await Clients.All.SendAsync("ReceiveMessage", chatMessage);
    }

    // Called when a user is typing.
    public async Task Typing(string senderId, string displayName)
    {
        // Notify other clients (excluding the sender) that this user is typing.
        await Clients.Others.SendAsync("UserTyping", displayName);
    }
}
