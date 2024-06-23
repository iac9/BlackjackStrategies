namespace BlackjackStrategies.Application
{
    public interface IGameAnalyser
    {
        decimal GetExpectedValue(IEnumerable<GameOutcome> outcomes);
        Dictionary<GameResult, decimal> GetGameReseultProbabilities(IEnumerable<GameOutcome> outcomes);
    }

    public class GameAnalyser
    {
    }
}
