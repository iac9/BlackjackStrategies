using BlackjackStrategies.Domain;

namespace BlackjackStrategies.API.Models;

public class SimulateGameRequest
{
    public required GameSettings GameSettings { get; set; }
    public required int NumberOfGames { get; set; }
}