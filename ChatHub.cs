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

    // Updated SendMessage method with error logging.
    public async Task SendMessage(string senderId, string displayName, string message, string replyToMessageId)
    {
        try
        {
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                DisplayName = displayName,
                Message = message,
                Timestamp = DateTime.UtcNow,
                // Set ReplyToMessageId to null if no reply is intended.
                ReplyToMessageId = string.IsNullOrEmpty(replyToMessageId) ? null : replyToMessageId
            };

            _messageStore.AddMessage(chatMessage);

            // Broadcast the message to all connected clients.
            await Clients.All.SendAsync("ReceiveMessage", chatMessage);
        }
        catch(Exception ex)
        {
            // Log the exception details. You can use any logging mechanism; here we use Console.WriteLine.
            Console.WriteLine("Error in SendMessage: " + ex.ToString());
            // Re-throw the exception so that the client is notified.
            throw;
        }
    }

    // Called when a user is typing.
    public async Task Typing(string senderId, string displayName)
    {
        try
        {
            // Notify other clients (excluding the sender) that this user is typing.
            await Clients.Others.SendAsync("UserTyping", displayName);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error in Typing: " + ex.ToString());
            throw;
        }
    }
}
