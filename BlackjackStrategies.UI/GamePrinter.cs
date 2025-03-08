using BlackjackStrategies.Application;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.UI;

public interface IGamePrinter
{
    void Print(GameOutcome[] gameOutcomes);
}

public class GamePrinter(IGameAnalyser gameAnalyser, GameSettings gameSettings) : IGamePrinter
{
    public void Print(GameOutcome[] gameOutcomes)
    {
        var roundsUntilBankrupt = Array.IndexOf(gameOutcomes.Select(o => o.Money).ToArray(), 0);

        for (var gameNumber = 0; gameNumber < gameOutcomes.Length; gameNumber++)
        {
            var outcome = gameOutcomes[gameNumber];

            if (gameNumber == roundsUntilBankrupt)
                break;

            PrintGameOutcome(gameNumber, outcome);
        }

        var gameStatistics = gameAnalyser.GetGameStatistics(gameOutcomes);

        foreach (var gameResult in gameStatistics.GameResultCount.Keys)
        {
            var count = gameStatistics.GameResultCount[gameResult];
            var probability = count / (decimal)gameOutcomes.Length;
            var percentage = Math.Round(probability * 100, 2);

            Console.WriteLine($"{gameResult}: {count}/{gameOutcomes.Length} = {percentage}%");
        }

        Console.WriteLine($"EV: {gameStatistics.ExpectedValue}");
        Console.WriteLine($"Rounds until bankrupt: {roundsUntilBankrupt}");
        Console.WriteLine($"Highest winnings: ${gameOutcomes.Max(o => o.Money) - gameSettings.StartingAmount}");
        Console.WriteLine("");
    }

    private void PrintGameOutcome(int gameNumber, GameOutcome outcome)
    {
        Console.WriteLine($"Game: {gameNumber + 1}");
        Console.WriteLine($"Game outcome: {outcome.GameResult}");
        Console.WriteLine($"Doubled: {outcome.Doubled}");
        Console.WriteLine($"Split: {outcome.Split}");
        Console.WriteLine($"Cards: {outcome.CardsRemaining}/{gameSettings.NumberOfDecks * 52}");
        Console.WriteLine($"Player's Hand ({outcome.PlayerHand.GetValue()}): {outcome.PlayerHand}");
        Console.WriteLine($"Dealer's Hand ({outcome.DealerHand.GetValue()}): {outcome.DealerHand}");
        Console.WriteLine($"Current/Starting: ${outcome.Money}/{gameSettings.StartingAmount}");
        Console.WriteLine("");
    }
}