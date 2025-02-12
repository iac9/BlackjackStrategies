using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Infrastructure;
using BlackjackStrategies.UI;

var gameSettings = new GameSettings
{
    NumberOfDecks = 6,
    NumberOfGames = 1000,
    StartingAmount = 200M,
    BettingSize = 15M,
    StrategyType = StrategyType.Martingale,
    AutomaticShuffler = false
};

var gameAnalyser = new GameAnalyser();
var gamePrinter = new GamePrinter(gameAnalyser);
var betServiceFactory = new BetServiceFactory();
var gameSettingsValidator = new GameSettingValidator();
var gameSimulator = new GameSimulator(new BasicStrategyPlayerService(), betServiceFactory, gameSettingsValidator);
var csvWriter = new CsvWriter();

var gameOutcomes = gameSimulator.Simulate(gameSettings).ToArray();
gamePrinter.Print(gameOutcomes.ToArray(), gameSettings.NumberOfDecks, gameSettings.StartingAmount);

csvWriter.WriteToCsv(gameOutcomes, "../../../gameOutcomes.csv");
