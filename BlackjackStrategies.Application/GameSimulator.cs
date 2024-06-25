using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application
{
    public interface IGameSimulator
    {
        public IEnumerable<GameOutcome> GameOutcomes { get; }
        IEnumerable<GameOutcome> Simulate(int numberOfDecks, int numberOfGames);
    }

    public class GameSimulator(IPlayerService playerService) : IGameSimulator
    {
        private readonly Hand DealerHand = new();
        private Deck Deck = new();
        private readonly List<GameOutcome> _gameOutcomes = [];
        public IEnumerable<GameOutcome> GameOutcomes => _gameOutcomes;

        public IEnumerable<GameOutcome> Simulate(int numberOfDecks, int numberOfGames)
        {
            Deck = new Deck(numberOfDecks);
            Deck.Shuffle();

            for (int _ = 0; _ < numberOfGames; _++)
            {
                playerService.ResetState();
                DealerHand.Clear();

                if (Deck.Count < 16)
                {
                    Deck.ResetDeck();
                    Deck.Shuffle();
                }

                DrawCard(playerService.Hand, 2);
                DrawCard(DealerHand, 2);

                HandlePlayerTurn();
                HandleDealerTurn();

                LogGameOutcome();
            }

            return _gameOutcomes;
        }

        private void HandlePlayerTurn()
        {
            var dealerUpCard = DealerHand.Cards.First();
            var playerAction = playerService.GetAction(dealerUpCard);

            while (playerService.Hand.GetValue() < 22 && playerAction != HandAction.Stay)
            {
                if (playerAction == HandAction.Hit)
                {
                    DrawCard(playerService.Hand, 1);
                    playerAction = playerService.GetAction(dealerUpCard);
                }
                else if (playerAction == HandAction.Double)
                {
                    DrawCard(playerService.Hand, 1);
                    playerService.Doubled = true;
                    playerAction = HandAction.Stay;
                }
                else if (playerService.SplitHands == null && playerAction == HandAction.Split)
                {
                    var firstHand = new Hand(playerService.Hand.Cards.First());
                    var secondHand = new Hand(playerService.Hand.Cards.Last());
                    playerService.SplitHands = [firstHand, secondHand];

                    DrawCard(firstHand, 1);
                    playerService.Hand = firstHand;
                    HandlePlayerTurn();

                    DrawCard(secondHand, 1);
                    playerService.Hand = secondHand;
                    HandlePlayerTurn();

                    playerAction = HandAction.Stay;
                }
            }
        }

        private void HandleDealerTurn()
        {
            while (DealerHand.GetValue() < 17)
            {
                DrawCard(DealerHand, 1);
            }
        }

        private void DrawCard(Hand hand, int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                hand.AddCard(Deck.Draw());
            }
        }

        private void LogGameOutcome()
        {
            if (playerService.SplitHands == null)
                _gameOutcomes.Add(GetGameOutcome(playerService.Hand));
            else
                _gameOutcomes.AddRange(playerService.SplitHands.Select(GetGameOutcome));
        }

        private GameOutcome GetGameOutcome(Hand hand)
        {
            return new GameOutcome
            {
                GameResult = GetGameResult(hand),
                PlayerHand = new Hand([.. hand.Cards]),
                DealerHand = new Hand([.. DealerHand.Cards]),
                Doubled = playerService.Doubled,
                Split = playerService.SplitHands != null,
                CardsRemaining = Deck.Count,
            };
        }

        private GameResult GetGameResult(Hand hand)
        {
            var playerHandValue = hand.GetValue();
            var dealerHandValue = DealerHand.GetValue();

            if (playerHandValue > 21)
                return GameResult.Lose;
            else
            {
                if (playerHandValue == 21)
                    return GameResult.Blackjack;

                if (dealerHandValue > 21)
                    return GameResult.Win;

                if (playerHandValue == dealerHandValue)
                    return GameResult.Push;
                else if (playerHandValue < dealerHandValue)
                    return GameResult.Lose;
                else
                    return GameResult.Win;
            }
        }
    }
}
