using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Application.Strategies;
using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application
{
    public interface IGameSimulator
    {
        IEnumerable<GameOutcome> Simulate(
            int numberOfDecks,
            int numberOfGames,
            decimal startingAmount,
            decimal bettingSize,
            StrategyType strateg
        );
    }

    public class GameSimulator(IPlayerService playerService, IBetServiceFactory betServiceFactory) : IGameSimulator
    {
        private readonly Hand DealerHand = new();
        private Deck Deck = new();

        public IEnumerable<GameOutcome> Simulate(
            int numberOfDecks,
            int numberOfGames,
            decimal startingAmount,
            decimal bettingSize,
            StrategyType strategy
        )
        {
            var betService = betServiceFactory.GetBetSerivce(strategy, startingAmount, bettingSize);
            var gameOutcomes = new List<GameOutcome>();
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

                LogGameOutcome(betService, gameOutcomes);
            }

            return gameOutcomes;
        }

        private void HandlePlayerTurn()
        {
            var dealerUpCard = DealerHand.Cards.First();
            var playerAction = playerService.GetAction(dealerUpCard);

            while (playerService.Hand.GetValue() < 22 && playerAction != HandAction.Stay)
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
            while (DealerHand.GetValue() < 17)
            {
                DrawCard(DealerHand, 1);
            }
        }

        private void DrawCard(Hand hand, int numberOfCards=1)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                hand.AddCard(Deck.Draw());
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
                GameResult = hand.GetGameResult(DealerHand),
                PlayerHand = new Hand([.. hand.Cards]),
                DealerHand = new Hand([.. DealerHand.Cards]),
                Doubled = playerService.Doubled,
                Split = playerService.SplitHands != null,
                CardsRemaining = Deck.Count,
            };
        }
    }
}
