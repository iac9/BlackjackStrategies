using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Domain.Deck;

namespace BlackjackStrategies.Application;

public interface IGameSimulator
{
    IEnumerable<GameOutcome> Simulate(GameSettings settings, int numberOfGames);
}

public class GameSimulator(
    BasePlayer player,
    IDealer dealer,
    IBetServiceFactory betServiceFactory,
    IDeckFactory deckFactory) : IGameSimulator
{
    private Deck _deck = new();

    public IEnumerable<GameOutcome> Simulate(GameSettings settings, int numberOfGames)
    {
        var betService =
            betServiceFactory.GetBetService(settings.StrategyType, settings.StartingAmount, settings.BettingSize);
        var gameOutcomes = new List<GameOutcome>();
        _deck = deckFactory.CreateDeck();
        _deck.Shuffle();

        for (var _ = 0; _ < numberOfGames; _++)
        {
            player.ResetState();
            dealer.ResetState();

            if (settings.AutomaticShuffler || _deck.Count < 16)
            {
                _deck = deckFactory.CreateDeck();
                _deck.Shuffle();
            }

            DrawCard(player.CurrentHand, 2);
            DrawCard(dealer.Hand, 2);

            HandlePlayerTurn();

            if (player.CurrentHand.GetValue() != Constants.Blackjack)
                HandleDealerTurn();

            LogGameOutcome(betService, gameOutcomes);
        }

        return gameOutcomes;
    }

    private void HandlePlayerTurn()
    {
        var dealerUpCard = dealer.Hand.Cards.First();
        var playerAction = player.GetAction(dealerUpCard);

        while (player.CurrentHand.GetValue() < Constants.Blackjack && playerAction != HandAction.Stay)
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

    private void HandleDealerTurn()
    {
        while (dealer.GetAction() != HandAction.Stay) DrawCard(dealer.Hand);
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
            GameResult = hand.GetGameResult(dealer.Hand),
            PlayerHand = new Hand([.. hand.Cards]),
            DealerHand = new Hand([.. dealer.Hand.Cards]),
            Doubled = player.Doubled,
            Split = player.Hands.Count > 1,
            CardsRemaining = _deck.Count
        };
    }
}