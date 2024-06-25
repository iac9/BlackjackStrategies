using BlackjackStrategies.Application;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Domain;

var playerService = new BasicStrategyPlayerService();
var betService = new MartingaleBetService(0.05M);
var gameSimulator = new GameSimulator(playerService);


var numberOfDecks = 6;
var gameOutcomes = gameSimulator.Simulate(numberOfDecks, 1000);

var gameResultsCount = gameOutcomes.GroupBy(o => o.GameResult).ToDictionary(g => g.Key, g => g.Count());
var ev = 0M;

foreach (var outcome in gameOutcomes)
{
    Console.WriteLine($"Game outcome: {outcome.GameResult}");
    Console.WriteLine($"Doubled: {outcome.Doubled}");
    Console.WriteLine($"Cards: {outcome.CardsRemaining}/{numberOfDecks * 52}");
    Console.WriteLine($"Player's Hand: {outcome.PlayerHand}");
    Console.WriteLine($"Dealer's Hand: {outcome.DealerHand}");
    Console.WriteLine("");
}

foreach (var gameResult in gameResultsCount.Keys)
{
    var count = gameResultsCount[gameResult];
    var probability = count / (decimal)gameOutcomes.Count();
    var percentage = Math.Round(probability * 100, 2);

    ev += gameResult switch
    {
        GameResult.Win => probability,
        GameResult.Lose => -probability,
        GameResult.Blackjack => 1.5M * probability,
        _ => 0,
    };

    Console.WriteLine($"{gameResult}: {count}/{gameOutcomes.Count()} = {percentage}%");
}

var profitLoss = betService.GetAmountToBet(1000, gameSimulator.GameOutcomes);

Console.WriteLine($"EV: {ev}");
Console.WriteLine($"Profit/Loss: ${profitLoss})");
Console.WriteLine("");
