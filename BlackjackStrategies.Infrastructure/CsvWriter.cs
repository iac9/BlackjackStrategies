using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Infrastructure;

public interface ICsvWriter
{
    public void WriteToCsv(IEnumerable<GameOutcome> gameOutcomes, string filePath);
}

public class CsvWriter : ICsvWriter
{
    public void WriteToCsv(IEnumerable<GameOutcome> gameOutcomes, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        // Write the header
        writer.WriteLine(
            "GameResult,PlayerHand,DealerHand,PlayerHandValue,DealerHandValue,Money,Doubled,Split,CardsRemaining");

        // Write each game outcome
        foreach (var outcome in gameOutcomes)
            writer.WriteLine(
                $"{outcome.GameResult}," +
                $"{outcome.PlayerHand}," +
                $"{outcome.DealerHand}," +
                $"{outcome.PlayerHand.GetValue()}," +
                $"{outcome.DealerHand.GetValue()}," +
                $"{outcome.Money}," +
                $"{outcome.Doubled}," +
                $"{outcome.Split}," +
                $"{outcome.CardsRemaining}");
    }
}