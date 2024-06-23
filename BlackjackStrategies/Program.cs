using BlackjackStrategies.Application;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Domain;

var deck = new Deck(6);
deck.Shuffle();

var playerService = new BasicStrategyPlayerService();
var betService = new MartingaleBetService(0.05M);
var gameSimulator = new GameSimulator(playerService);


for (int i = 0; i < 200; i++)
{
    var gameOutcomes = gameSimulator.Simulate(6, 1);

    var gameResultsCount = gameOutcomes.GroupBy(o => o.GameResult).ToDictionary(g => g.Key, g => g.Count());
    var ev = 0M;

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

    Console.WriteLine($"Game outcome: {gameSimulator.GameOutcomes.Last().GameResult}");
    Console.WriteLine($"Player's Hand: {gameSimulator.GameOutcomes.Last().PlayerHand}");
    Console.WriteLine($"Dealer's Hand: {gameSimulator.GameOutcomes.Last().DealerHand}");
    Console.WriteLine($"EV: {ev}");
    Console.WriteLine($"Profit/Loss ($): {betService.GetProfitLoss(1000, gameSimulator.GameOutcomes)})");
    Console.WriteLine("");
}
