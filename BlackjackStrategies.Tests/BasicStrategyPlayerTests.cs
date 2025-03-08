using AutoFixture;
using AutoFixture.AutoMoq;
using BlackjackStrategies.Application.ActionService;
using BlackjackStrategies.Domain;
using FluentAssertions;
using Moq;

namespace BlackjackStrategies.Tests;

public class BasicStrategyPlayerTests
{
    private readonly IFixture _fixture;
    private readonly BasicStrategyPlayer _player;

    public BasicStrategyPlayerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _player = new BasicStrategyPlayer();
    }

    [Theory]
    [InlineData(CardValue.Ace, CardValue.Two)]
    public void GetAction_ReturnsSplitCorrectly_GivenDealerUpCard(CardValue playerDuplicateCardValue,
        CardValue dealerUpCardValue)
    {
        var playerCards = _fixture
            .Build<Card>()
            .With(c => c.Value, playerDuplicateCardValue)
            .CreateMany<Card>(2)
            .ToArray();
        _player.Hands = [new Hand(playerCards)];
        var dealerUpCard = new Card
        {
            Suit = It.IsAny<CardSuit>(),
            Value = dealerUpCardValue
        };

        var action = _player.GetAction(dealerUpCard);

        action.Should().Be(HandAction.Split);
    }

    // private static IEnumerable<object[]> GetSplitTestData() => 
    // [
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Ace, It.IsAny<CardValue>() },
    //     new object[] { CardValue.Eight, It.IsAny<CardValue>() },
    // ];
}