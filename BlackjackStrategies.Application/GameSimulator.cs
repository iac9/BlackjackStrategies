using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application;

public interface IGameSimulator
{
    IEnumerable<GameOutcome> Simulate(int numberOfGames);
}

public class GameSimulator(
    BasePlayer player,
    IBetServiceFactory betServiceFactory,
    GameSettings settings) : IGameSimulator
{
    private readonly Hand _dealerHand = new();
    private Deck _deck = new();

    public IEnumerable<GameOutcome> Simulate(int numberOfGames)
    {
        var betService =
            betServiceFactory.GetBetService(settings.StrategyType, settings.StartingAmount, settings.BettingSize);
        var gameOutcomes = new List<GameOutcome>();
        _deck = new Deck(settings.NumberOfDecks);
        _deck.Shuffle();

        for (var _ = 0; _ < numberOfGames; _++)
        {
            player.ResetState();
            _dealerHand.Clear();

            if (settings.AutomaticShuffler || _deck.Count < 16)
            {
                _deck.ResetDeck();
                _deck.Shuffle();
            }

            DrawCard(player.CurrentHand, 2);
            DrawCard(_dealerHand, 2);

            HandlePlayerTurn();
            
            if (player.CurrentHand.GetValue() != Constants.Blackjack)
                HandleDealerTurn();

            LogGameOutcome(betService, gameOutcomes);
        }

        return gameOutcomes;
    }

    private void HandlePlayerTurn()
    {
        var dealerUpCard = _dealerHand.Cards.First();
        var playerAction = player.GetAction(dealerUpCard);

        while (player.CurrentHand.GetValue() < Constants.Blackjack && playerAction != HandAction.Stay)
        {
            if (playerAction == HandAction.Hit)
            {
                DrawCard(player.CurrentHand);
                playerAction = player.GetAction(dealerUpCard);
            }
            else if (playerAction == HandAction.Double)
            {
                DrawCard(player.CurrentHand);
                player.Doubled = true;
                playerAction = HandAction.Stay;
            }
            else if (playerAction == HandAction.Split)
            {
                var splitCard = player.CurrentHand.PopLastCard();
                player.Hands.Add(new Hand(splitCard));
                DrawCard(player.CurrentHand);
                HandlePlayerTurn();
                
                player.NextHand();
                DrawCard(player.CurrentHand);
                playerAction = player.GetAction(dealerUpCard);
            }
        }
    }

    private void HandleDealerTurn()
    {
        while (_dealerHand.GetValue() < Constants.DealerStayThreshold)
        {
            DrawCard(_dealerHand);
        }
    }

    private void DrawCard(Hand hand, int numberOfCards = 1)
    {
        for (var i = 0; i < numberOfCards; i++)
            hand.AddCard(_deck.Draw());
    }

    private void LogGameOutcome(IBetService betService, List<GameOutcome> gameOutcomes)
    {
        foreach (var gameOutcome in player.Hands.Select(GetGameOutcome))
        {
            betService.MakeBet(gameOutcome);
            gameOutcomes.Add(gameOutcome);
        }
    }

    private GameOutcome GetGameOutcome(Hand hand)
    {
        return new GameOutcome
        {
            GameResult = hand.GetGameResult(_dealerHand),
            PlayerHand = new Hand([.. hand.Cards]),
            DealerHand = new Hand([.. _dealerHand.Cards]),
            Doubled = player.Doubled,
            Split = player.Hands.Count > 1,
            CardsRemaining = _deck.Count,
        };
    }
}