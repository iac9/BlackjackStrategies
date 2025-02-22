using BlackjackStrategies.Domain;
using FluentAssertions;
using Moq;

namespace BlackjackStrategies.Tests;

public class CardTests
{
    [Fact]
    public void IsBetween_ThrowsArgumentException_GivenEndIsGreaterThanStart()
    {
        Action action = () =>  It.IsAny<CardValue>().IsBetween(CardValue.Four, CardValue.Three);

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(CardValue.Two, CardValue.Two, CardValue.Two)]
    [InlineData(CardValue.Two, CardValue.Six, CardValue.Four)]
    public void IsBetween_ReturnsTrue_IfCardInRange(CardValue start, CardValue end, CardValue card)
    {
        var actual = card.IsBetween(start, end);

        actual.Should().BeTrue();
    }
}