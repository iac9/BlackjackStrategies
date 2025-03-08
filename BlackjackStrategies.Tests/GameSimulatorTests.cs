using AutoFixture;
using AutoFixture.AutoMoq;
using BlackjackStrategies.Application;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Application.BetService;
using BlackjackStrategies.Domain;
using BlackjackStrategies.Domain.Deck;
using FluentAssertions;
using Moq;

namespace BlackjackStrategies.Tests;

public class GameSimulatorTests
{
    private readonly IFixture _fixture;

    public GameSimulatorTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void Simulate_PlaysCorrectNumberOfGames_GivenSplits(int numberOfSplits)
    {
        var cards = _fixture
            .Build<Card>()
            .With(c => c.Value, CardValue.Ace)
            .CreateMany(numberOfSplits * 2 + 4);
        _fixture.Freeze<Mock<IDeckFactory>>()
            .Setup(m => m.CreateDeck())
            .Returns(new Deck(cards));
        var actions = new Queue<HandAction>(Enumerable
            .Range(0, numberOfSplits)
            .Select(_ => HandAction.Split));
        _fixture.Freeze<Mock<BasePlayer>>()
            .Setup(m => m.GetAction(It.IsAny<Card>()))
            .Returns(() => actions.Count > 0 ? actions.Dequeue() : HandAction.Stay);
        _fixture.Freeze<Mock<IDealer>>()
            .Setup(m => m.GetAction()).Returns(HandAction.Stay);
        _fixture.Freeze<Mock<IBetServiceFactory>>();
        var gameSimulator = _fixture.Create<GameSimulator>();

        var actualGameOutcomes = gameSimulator.Simulate(_fixture.Create<GameSettings>(), 1);
        
        actualGameOutcomes.Count().Should().Be(numberOfSplits + 1);
    }
}