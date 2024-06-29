using BlackjackStrategies.Application;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Infrastructure;
using BlackjackStrategies.UI;

var startingAmount = 200M;
var bettingSize = 15M;
var numberOfGames = 1000;
var numberOfDecks = 6;

var gameAnalyser = new GameAnalyser();
var gamePrinter = new GamePrinter(gameAnalyser);
var betService = new MartingaleBetService(startingAmount, bettingSize);
var gameSimulator = new GameSimulator(new BasicStrategyPlayerService(), betService);
var csvWriter = new CsvWriter();

var gameOutcomes = gameSimulator.Simulate(numberOfDecks, numberOfGames);
gamePrinter.Print(gameOutcomes.ToArray(), numberOfGames, startingAmount);

csvWriter.WriteToCsv(gameOutcomes, "");

