using BlackjackStrategies.Application;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Domain;

var playerService = new BasicStrategyPlayerService();
var betService = new MartingaleBetService();
var gameSimulator = new GameSimulator(playerService);

var numberOfDecks = 6;
var gameOutcomes = gameSimulator.Simulate(numberOfDecks, 200).ToArray();

var gameResultsCount = gameOutcomes.GroupBy(o => o.GameResult).ToDictionary(g => g.Key, g => g.Count());
var ev = 0M;
var profitLoss = betService.GetAmountOverTime(200, 15, gameSimulator.GameOutcomes).ToArray();
var roundsUntilBankrupt = 0;

for (var i = 0; i < gameOutcomes.Length; i++)
{
    var outcome = gameOutcomes[i];

    if (profitLoss[i] == -200)
    {
        roundsUntilBankrupt = i + 1;
        break;
    }

    Console.WriteLine($"Game: {i + 1}");
    Console.WriteLine($"Game outcome: {outcome.GameResult}");
    Console.WriteLine($"Doubled: {outcome.Doubled}");
    Console.WriteLine($"Split: {outcome.Split}");
    Console.WriteLine($"Cards: {outcome.CardsRemaining}/{numberOfDecks * 52}");
    Console.WriteLine($"Player's Hand ({outcome.PlayerHand.GetValue()}): {outcome.PlayerHand}");
    Console.WriteLine($"Dealer's Hand ({outcome.DealerHand.GetValue()}): {outcome.DealerHand}");
    Console.WriteLine($"Profit/Loss: ${Math.Round(profitLoss[i], 2)}");
    Console.WriteLine("");
}

foreach (var gameResult in gameResultsCount.Keys)
{
    var count = gameResultsCount[gameResult];
    var probability = count / (decimal)gameOutcomes.Length;
    var percentage = Math.Round(probability * 100, 2);

    ev += gameResult switch
    {
        GameResult.Win => probability,
        GameResult.Lose => -probability,
        GameResult.Blackjack => 1.5M * probability,
        _ => 0,
    };

    Console.WriteLine($"{gameResult}: {count}/{gameOutcomes.Length} = {percentage}%");
}

Console.WriteLine($"EV: {ev}");
Console.WriteLine($"Rounds until bankrupt: {roundsUntilBankrupt}");
Console.WriteLine($"Highest winnings: ${profitLoss.Max()}");
Console.WriteLine("");
