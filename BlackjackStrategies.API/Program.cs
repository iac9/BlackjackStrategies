using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGameAnalyser, GameAnalyser>();
builder.Services.AddTransient<IGameSimulator, GameSimulator>();
builder.Services.AddTransient<BasePlayer, BasicStrategyPlayer>();
builder.Services.AddTransient<IBetServiceFactory, BetServiceFactory>();
builder.Services.AddTransient<IDealer, Dealer>();

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