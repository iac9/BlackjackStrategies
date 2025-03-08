using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Domain.Deck;
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
var deckFactory = new DeckFactory();
var player = new BasicStrategyPlayer();
var dealer = new Dealer();
var gameSimulator = new GameSimulator(player, dealer, betServiceFactory, deckFactory);
var csvWriter = new CsvWriter();

var gameOutcomes = gameSimulator.Simulate(gameSettings, 1000).ToArray();
gamePrinter.Print(gameOutcomes.ToArray());

csvWriter.WriteToCsv(gameOutcomes, "../../../gameOutcomes.csv");