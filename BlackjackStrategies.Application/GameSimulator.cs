using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application;

public interface IGameSimulator
{
    IEnumerable<GameOutcome> Simulate(GameSettings settings);
}

public class GameSimulator(
    IPlayerService playerService,
    IBetServiceFactory betServiceFactory,
    IGameSettingValidator gameSettingValidator) : IGameSimulator
{
    private readonly Hand _dealerHand = new();
    private Deck _deck = new();

    public IEnumerable<GameOutcome> Simulate(GameSettings settings)
    {
        gameSettingValidator.ValidateGameSettings(settings);
        
        var betService =
            betServiceFactory.GetBetService(settings.StrategyType, settings.StartingAmount, settings.BettingSize);
        var gameOutcomes = new List<GameOutcome>();
        _deck = new Deck(settings.NumberOfDecks);
        _deck.Shuffle();

        for (var _ = 0; _ < settings.NumberOfGames; _++)
        {
            playerService.ResetState();
            _dealerHand.Clear();

            if (settings.AutomaticShuffler || _deck.Count < 16)
            {
                _deck.ResetDeck();
                _deck.Shuffle();
            }

            DrawCard(playerService.Hand, 2);
            DrawCard(_dealerHand, 2);

            HandlePlayerTurn();
            HandleDealerTurn();

            LogGameOutcome(betService, gameOutcomes);
        }

        return gameOutcomes;
    }

    private void HandlePlayerTurn()
    {
        var dealerUpCard = _dealerHand.Cards.First();
        var playerAction = playerService.GetAction(dealerUpCard);

        while (playerService.Hand.GetValue() < Constants.Blackjack && playerAction != HandAction.Stay)
        {
            if (playerAction == HandAction.Hit)
            {
                DrawCard(playerService.Hand);
                playerAction = playerService.GetAction(dealerUpCard);
            }
            else if (playerAction == HandAction.Double)
            {
                DrawCard(playerService.Hand);
                playerService.Doubled = true;
                playerAction = HandAction.Stay;
            }
            else if (playerService.SplitHands == null && playerAction == HandAction.Split)
            {
                var firstHand = new Hand(playerService.Hand.Cards.First());
                var secondHand = new Hand(playerService.Hand.Cards.Last());
                playerService.SplitHands = [firstHand, secondHand];

                DrawCard(firstHand);
                playerService.Hand = firstHand;
                HandlePlayerTurn();

                DrawCard(secondHand);
                playerService.Hand = secondHand;
                HandlePlayerTurn();

                playerAction = HandAction.Stay;
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
        {
            hand.AddCard(_deck.Draw());
        }
    }

    private void LogGameOutcome(IBetSerivce betService, List<GameOutcome> gameOutcomes)
    {
        if (playerService.SplitHands == null)
        {
            var gameOutcome = GetGameOutcome(playerService.Hand);
            betService.MakeBet(gameOutcome);
            gameOutcomes.Add(gameOutcome);
        }
        else
        {
            foreach (var gameOutcome in playerService.SplitHands.Select(GetGameOutcome))
            {
                betService.MakeBet(gameOutcome);
                gameOutcomes.Add(gameOutcome);
            }
        }
    }

    private GameOutcome GetGameOutcome(Hand hand)
    {
        return new GameOutcome
        {
            GameResult = hand.GetGameResult(_dealerHand),
            PlayerHand = new Hand([.. hand.Cards]),
            DealerHand = new Hand([.. _dealerHand.Cards]),
            Doubled = playerService.Doubled,
            Split = playerService.SplitHands != null,
            CardsRemaining = _deck.Count,
        };
    }
}