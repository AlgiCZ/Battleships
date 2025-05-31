using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.ApiService.Storage
{
    public class GamesStorage : IGamesStorage
    {
        private List<Game> _games;

        public GamesStorage()
        {
            _games = new List<Game>();
        }

        public IList<Game> GetAllGames()
        {
            return _games;
        }

        public Game? GetGameById(int id)
        {
            return _games.FirstOrDefault(g => g.Id == id);
        }

        public Game AddGame(Game game)
        {
            game.Id = _games.Count > 0 ? _games.Max(g => g.Id) + 1 : 1;
            _games.Add(game);

            return game;
        }

    }

    public class Game
    {
        public int Id { get; internal set; }
        public required string Player1Name { get; init; }
        public required string Player2Name { get; init; }
        public required List<Ship> Player1Ships { get; init; }
        public required List<Ship> Player2Ships { get; init; }
        public required List<Impact> PlayerImpacts { get; init; }
        public required List<Impact> OpponentImpacts { get; init; }
        public required Size Dimension { get; init; }
        public GameStatus GameStatus { get; set; } = GameStatus.Unknown;
    }
}
