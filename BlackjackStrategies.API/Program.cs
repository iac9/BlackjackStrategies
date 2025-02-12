using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IGameAnalyser, GameAnalyser>();
builder.Services.AddSingleton<IGameSimulator, GameSimulator>();
builder.Services.AddSingleton<IPlayerService, BasicStrategyPlayerService>();
builder.Services.AddSingleton<IBetServiceFactory, BetServiceFactory>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
