using System.Reflection;
using AutoFixture;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using FluentAssertions;

namespace BlackjackStrategies.Tests;

public class MartingaleBetServiceTests
{
    private readonly IFixture _fixture;

    public MartingaleBetServiceTests()
    {
        _fixture = new Fixture();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void MakeBet_IncreasesAmountByNDoubled_GivenNConsecutiveLosses(int numberOfLosses)
    {
        const decimal startingAmount = 100;
        const decimal bettingSize = 15;
        var expectedAmount = startingAmount + bettingSize * (decimal)Math.Pow(2, numberOfLosses);
        var gameOutcome = _fixture.Build<GameOutcome>()
            .With(go => go.GameResult, GameResult.Win)
            .With(go => go.Doubled, false)
            .With(go => go.Split, false)
            .Create();
        var betService = _fixture.Build<MartingaleBetService>()
            .With(mbs => mbs.Amount, startingAmount)
            .With(mb => mb.SingleBetSize, bettingSize)
            .Create();
        var betServiceType = typeof(MartingaleBetService);
        var propertyInfo =
            betServiceType.GetField("_consecutiveLosses", BindingFlags.NonPublic | BindingFlags.Instance);
        propertyInfo?.SetValue(betService, numberOfLosses);

        betService.MakeBet(gameOutcome);

        gameOutcome.Money.Should().Be(expectedAmount);
    }
}