using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using FluentAssertions;
using Moq;
using System.Reflection;

namespace BlackjackStrategies.Tests;

public class MartingaleBetServiceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void MakeBet_IncreasesAmountByNDoubled_GivenNConsecutiveLosses(int numberOfLosses)
    {
        const decimal startingAmount = 100;
        const decimal bettingSize = 15;
        var expectedAmount = startingAmount + (bettingSize * (decimal)Math.Pow(2, numberOfLosses));
        var gameOutcome = new GameOutcome
        {
            GameResult = GameResult.Win,
            PlayerHand = It.IsAny<Hand>(),
            DealerHand = It.IsAny<Hand>(),
            Money = 0M,
            Doubled = false,
            Split = It.IsAny<bool>(),
            CardsRemaining = It.IsAny<int>()
        };
        var betService = new MartingaleBetService();
        var betServiceType = typeof(MartingaleBetService);
        var propertyInfo = betServiceType.GetField("consecutiveLosses", BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo?.SetValue(betService, numberOfLosses);

        betService.MakeBet(gameOutcome);

        gameOutcome.Money.Should().Be(expectedAmount);
    }
}