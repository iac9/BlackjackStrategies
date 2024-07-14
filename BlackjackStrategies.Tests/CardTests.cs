using BlackjackStrategies.Domain;
using Moq;
using FluentAssertions;

namespace BlackjackStrategies.Tests.Domain
{
    public class CardTests
    {
        [Fact]
        public void InRange_ThrowsArgumentException_GivenEndIsGreaterThanStart()
        {
            Action action = () => CardValueExtensions.InRange(CardValue.Four, CardValue.Three, It.IsAny<CardValue>());

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(CardValue.Two, CardValue.Two, CardValue.Two)]
        [InlineData(CardValue.Two, CardValue.Six, CardValue.Four)]
        public void InRange_ReturnsTrue_IfCardInRange(CardValue start, CardValue end, CardValue card)
        {
            var actual = CardValueExtensions.InRange(start, end, card);

            actual.Should().BeTrue();
        }
    }
}
