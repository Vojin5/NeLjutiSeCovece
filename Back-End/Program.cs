using Back_End.Models;
using Back_End.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Back_End.SignalR.Services;
using Back_End.IRepository;
using Back_End.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Parse("10.66.24.118"), 5295);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseCS"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.SetIsOriginAllowed(host => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = int.MaxValue;
});

builder.Services.AddSingleton<IGameLobby, GameLobby>();
builder.Services.AddSingleton<IActiveGames, ActiveGames>();
builder.Services.AddSingleton<IOnlinePlayers, OnlinePlayers>();
builder.Services.AddSingleton<IPendingGames, PendingGames>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMatchHistoryRepository, MatchHistoryRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/game");

app.Run();
