﻿using BlackjackStrategies.Application;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.UI
{
    public interface IGamePrinter
    {
        void Print(GameOutcome[] gameOutcome, int numberOfDecks, decimal startingAmount);
    }

    public class GamePrinter(IGameAnalyser gameAnalyser) : IGamePrinter
    {
        public void Print(GameOutcome[] gameOutcomes, int numberOfDecks, decimal startingAmount)
        {
            var roundsUntilBankrupt = gameOutcomes.Where(o => o.Money == -startingAmount).Count();

            for (var i = 0; i < gameOutcomes.Length; i++)
            {
                var outcome = gameOutcomes[i];

                if (outcome.Money <= -startingAmount)
                    break;

                Console.WriteLine($"Game: {i + 1}");
                Console.WriteLine($"Game outcome: {outcome.GameResult}");
                Console.WriteLine($"Doubled: {outcome.Doubled}");
                Console.WriteLine($"Split: {outcome.Split}");
                Console.WriteLine($"Cards: {outcome.CardsRemaining}/{numberOfDecks * 52}");
                Console.WriteLine($"Player's Hand ({outcome.PlayerHand.GetValue()}): {outcome.PlayerHand}");
                Console.WriteLine($"Dealer's Hand ({outcome.DealerHand.GetValue()}): {outcome.DealerHand}");
                Console.WriteLine($"Profit/Loss: ${outcome.Money}");
                Console.WriteLine("");
            }

            var gameResultCount = gameAnalyser.GetGameResultCount(gameOutcomes);

            foreach (var gameResult in gameResultCount.Keys)
            {
                var count = gameResultCount[gameResult];
                var probability = count / (decimal)gameOutcomes.Length;
                var percentage = Math.Round(probability * 100, 2);

                Console.WriteLine($"{gameResult}: {count}/{gameOutcomes.Length} = {percentage}%");
            }

            Console.WriteLine($"EV: {gameAnalyser.GetExpectedValue(gameOutcomes)}");
            Console.WriteLine($"Rounds until bankrupt: {roundsUntilBankrupt}");
            Console.WriteLine($"Highest winnings: ${gameOutcomes.Max(o => o.Money)}");
            Console.WriteLine("");

        }
    };
}
