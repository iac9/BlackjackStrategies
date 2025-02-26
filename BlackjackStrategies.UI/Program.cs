using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Infrastructure;
using BlackjackStrategies.UI;

var gameSettings = new GameSettings
{
    NumberOfDecks = 6,
    StartingAmount = 200M,
    BettingSize = 15M,
    StrategyType = StrategyType.Martingale,
    AutomaticShuffler = false
};

var gameAnalyser = new GameAnalyser();
var gamePrinter = new GamePrinter(gameAnalyser, gameSettings);
var betServiceFactory = new BetServiceFactory();
var gameSimulator = new GameSimulator(new BasicStrategyPlayer(), betServiceFactory, gameSettings);
var csvWriter = new CsvWriter();

var gameOutcomes = gameSimulator.Simulate(1000).ToArray();
gamePrinter.Print(gameOutcomes.ToArray());

csvWriter.WriteToCsv(gameOutcomes, "../../../gameOutcomes.csv");
