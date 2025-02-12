using BlackjackStrategies.Domain;

namespace BlackjackStrategies.Application;

public interface IGameSettingValidator
{
    void ValidateGameSettings(GameSettings gameSetting);
}

public class GameSettingValidator : IGameSettingValidator
{
    public void ValidateGameSettings(GameSettings gameSetting)
    {
        if (gameSetting.NumberOfGames < 1)
        {
            throw new ArgumentException("Number of games must be greater than 0.");
        }
        
        if (gameSetting.BettingSize > gameSetting.StartingAmount)
        {
            throw new ArgumentException("Betting size cannot exceed starting amount.");
        }
        
        if (gameSetting.NumberOfDecks < 1)
        {
            throw new ArgumentException("Number of decks must be greater than 0.");
        }
    }
}