﻿using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application.Strategies
{
    public interface IPlayerService
    {
        public Hand Hand { get; set; }
        public Tuple<Hand, Hand>? SplitHands { get; set; }

        HandAction GetAction(Hand playerHand, Card dealerUpCard);
    }

    public enum HandAction
    {
        Hit,
        Stay,
        Split,
        Double
    }
}