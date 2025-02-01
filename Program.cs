using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Configure SignalR to use Azure SignalR Service.
// Make sure your connection string is set in your configuration (e.g., in appsettings.json).
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
});

// Register the in-memory message store as a singleton.
builder.Services.AddSingleton<MessageStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Serve static files (this serves the files in wwwroot).
app.UseDefaultFiles();
app.UseStaticFiles();

// Map the SignalR hub endpoint.
app.MapHub<ChatHub>("/chat");

// Endpoint to retrieve messages that are less than 24 hours old.
app.MapGet("/messages", (MessageStore store) =>
{
    var now = DateTime.UtcNow;
    // Only return messages sent in the last 24 hours.
    var messages = store.GetMessages().Where(m => (now - m.Timestamp).TotalHours < 24);
    return messages;
});

app.Run();
