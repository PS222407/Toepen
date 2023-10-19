using Toepen_10_Hub.Hubs;
using Toepen_10_Hub.Interfaces;
using Toepen_10_Hub.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(_ => new Dictionary<string, UserConnection>());
builder.Services.AddSingleton<IGameService, GameService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
    
app.MapHub<GameHub>("/chatHubApi");

app.Run();