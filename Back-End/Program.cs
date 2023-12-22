using Back_End.Models;
using Back_End.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Back_End.SignalR.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Parse("192.168.28.8"), 5295);
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
        builder.WithOrigins(new string[]{ "http://127.0.0.1:5500", "http://192.168.28.8:5500"}).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
builder.Services.AddSignalR();

builder.Services.AddSingleton<IGameLobby, GameLobby>();
builder.Services.AddSingleton<IActiveGames, ActiveGames>();
builder.Services.AddSingleton<IOnlinePlayers, OnlinePlayers>();

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
