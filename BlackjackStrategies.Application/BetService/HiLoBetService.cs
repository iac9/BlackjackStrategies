using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.BetService;

public class HiLoBetService : BaseBetService
{
    private GameOutcome? _lastGameOutcome;
    private int _runningCount;

    public override void MakeBet(GameOutcome gameOutcome)
    {
        if (_lastGameOutcome?.Money == 0)
            return;

        var trueCount = GetTrueCount();
        var bet = Math.Min(Amount, SingleBetSize * (gameOutcome.Doubled ? 2 : 1) * trueCount);

        UpdateAmount(gameOutcome, bet);

        UpdateRunningCount(gameOutcome);

        _lastGameOutcome = gameOutcome;
    }

    private int GetTrueCount()
    {
        var trueCount = _lastGameOutcome == null
            ? _runningCount
            : (int)Math.Round(_runningCount / ((decimal)_lastGameOutcome.CardsRemaining / Constants.StandardDeckSize));

        return Math.Max(trueCount, 1);
    }

    private void UpdateRunningCount(GameOutcome gameOutcome)
    {
        if (_lastGameOutcome?.CardsRemaining < gameOutcome.CardsRemaining) _runningCount = 0;

        var cards = gameOutcome.PlayerHand.Cards.Concat(gameOutcome.DealerHand.Cards);

        foreach (var card in cards)
            _runningCount += card.Value switch
            {
                CardValue.Two or CardValue.Three or CardValue.Four or CardValue.Five or CardValue.Six => 1,
                CardValue.Ten or CardValue.Jack or CardValue.Queen or CardValue.King or CardValue.Ace => -1,
                _ => 0
            };
    }
}