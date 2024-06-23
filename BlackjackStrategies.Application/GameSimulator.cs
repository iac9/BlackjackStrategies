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
        private Hand DealerHand = new();
        private Deck Deck = new();
        private readonly List<GameOutcome> _gameOutcomes = [];

        public IEnumerable<GameOutcome> GameOutcomes => _gameOutcomes;

        public IEnumerable<GameOutcome> Simulate(int numberOfDecks, int numberOfGames)
        {
            Deck = new Deck(numberOfDecks);
            Deck.Shuffle();

            for (int i = 0; i < numberOfGames; i++)
            {
                playerService.SplitHands = null;

                if (Deck.Count < 16)
                {
                    Deck.ResetDeck(numberOfDecks);
                    Deck.Shuffle();
                }

                playerService.Hand = new Hand(Deck.Draw(), Deck.Draw());
                DealerHand = new Hand(Deck.Draw(), Deck.Draw());

                HandlePlayerTurn();
                HandleDealerTurn();

                LogGameOutcome();
            }

            return _gameOutcomes;
        }

        private void HandlePlayerTurn()
        {
            var dealerUpCard = DealerHand.Cards.First();
            var playerAction = playerService.GetAction(playerService.Hand, dealerUpCard);

            while (playerService.Hand.GetValue() < 22 && playerAction != HandAction.Stay)
            {
                if (playerAction == HandAction.Hit)
                {
                    DrawCard(playerService.Hand);
                    playerAction = playerService.GetAction(playerService.Hand, dealerUpCard);
                }
                else if (playerAction == HandAction.Double)
                {
                    DrawCard(playerService.Hand);
                    playerAction = HandAction.Stay;
                }
                else if (playerService.SplitHands == null && playerAction == HandAction.Split)
                {
                    var firstHand = new Hand(playerService.Hand.Cards.First());
                    var secondHand = new Hand(playerService.Hand.Cards.Last());
                    playerService.SplitHands = Tuple.Create(firstHand, secondHand);

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
            while (DealerHand.GetValue() < 17)
            {
                DrawCard(DealerHand);
            }
        }

        private void DrawCard(Hand hand) => hand.AddCard(Deck.Draw());

        private void LogGameOutcome()
        {
            if (playerService.SplitHands == null)
            {
                _gameOutcomes.Add(GetGameOutcome(playerService.Hand));
            }
            else
            {
                (Hand firstHand, Hand secondHand) = playerService.SplitHands;

                _gameOutcomes.Add(GetGameOutcome(firstHand));
                _gameOutcomes.Add(GetGameOutcome(secondHand));
            }
        }

        private GameOutcome GetGameOutcome(Hand hand)
        {
            return new GameOutcome
            {
                GameResult = GetGameResult(hand),
                PlayerHand = playerService.Hand,
                DealerHand = DealerHand,
                NumberOfCardsRemaining = Deck.Count
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

    public class GameOutcome
    {
        public GameResult GameResult { get; set; }
        public required Hand PlayerHand { get; set; }
        public required Hand DealerHand { get; set; }
        public required int NumberOfCardsRemaining { get; set; }
    }

    public enum GameResult
    {
        Win, 
        Lose, 
        Push,
        Blackjack
    }
}
