using Battleships.ServiceDefaults.Infrastructure.Responses;
using Battleships.ServiceDefaults.Models;

namespace Battleships.ApiService.Services
{
    public interface IGameService
    {

    }

    public class GameService : IGameService
    {
        public GameResponse CreateGame(string playerSName)
        {
            return new GameResponse
            {
                Id = 0
            };
        }

        public IList<GameResponse> GetGames(GameStatus? gameStatus )
        {
            return [new GameResponse
            {
                Id = 0
            }];
        }

        public GameResponse JoinGame(int gameId, string playerName)
        {
            return null;
        }



    }
}
