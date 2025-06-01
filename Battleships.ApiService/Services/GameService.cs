using Battleships.ApiService.Logic;
using Battleships.ApiService.Storage;
using Battleships.ServiceDefaults;
using Battleships.ServiceDefaults.Infrastructure;
using Battleships.ServiceDefaults.Models;

namespace Battleships.ApiService.Services
{

    public class GameService : IGameService
    {
        private readonly IGamesStorage _storage;
        private readonly IShipFactory _shipFactory;
        private readonly IGameLogic _gameLogic;

        public GameService(IGamesStorage storage, IShipFactory shipFactory, IGameLogic gameLogic)
        {
            _shipFactory = shipFactory;
            _gameLogic = gameLogic;
            _storage = storage;
        }

        public GameResponse CreateGame(AddGameRequest request)
        {
            var player1Ships = _shipFactory.CreateDefaulShips();
            var player2Ships = _shipFactory.CreateDefaulShips();
            _gameLogic.RandomizeShips(player1Ships, request.Dimension);
            _gameLogic.RandomizeShips(player2Ships, request.Dimension);

            var newGame = new Game
            {
                Player1Name = request.Player1Name,
                Player2Name = request.Player2Name,
                Player1Ships = player1Ships,
                Player2Ships = player2Ships,
                PlayerImpacts = new List<Impact>(),
                OpponentImpacts = new List<Impact>(),
                Dimension = request.Dimension
            };

            var game = _storage.AddGame(newGame);

            return new GameResponse(game.Id);
        }

        public IList<GameResponse> GetGames(GameStatus? gameStatus)
        {
            return _storage.GetAllGames().Select(x => new GameResponse(x.Id)).ToList();
        }

        public GetGameResponse GetGame(int gameId, string playerName)
        {
            var game = _storage.GetGameById(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found.");
            }
            game.GameStatus = GameStatus.Planning;
            var isPlayer1 = game.Player1Name.Equals(playerName, StringComparison.OrdinalIgnoreCase);
            return new GetGameResponse(
                game.Dimension,
                isPlayer1 ? game.Player1Ships : game.Player2Ships,
                isPlayer1 ? game.PlayerImpacts : game.OpponentImpacts,
                isPlayer1 ? game.OpponentImpacts : game.PlayerImpacts,
                game.GameStatus,
                game.Player1Name,
                game.Player2Name
                );
        }

        public GetRandomShipPositionsResponse GetRandomShipPositions(int gameId, string playerName)
        {
            var game = _storage.GetGameById(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found.");
            }
            var isPlayer1 = game.Player1Name.Equals(playerName, StringComparison.OrdinalIgnoreCase);
            var ships = isPlayer1 ? game.Player1Ships : game.Player2Ships;
            _gameLogic.RandomizeShips(ships, game.Dimension);
            return new GetRandomShipPositionsResponse(ships);
        }

        public GameStatusResponse? UpdateReadyPlayerStatus(int gameId, string playerName)
        {
            var game = _storage.GetGameById(gameId);
            if (game == null)
            {
                return null;

            }
            if (game.GameStatus != GameStatus.Planning && game.GameStatus != GameStatus.WaitingForPlayer1 && game.GameStatus != GameStatus.WaitingForPlayer2)
            {
                throw new InvalidOperationException($"Game with ID {gameId} is not in planning status.");
            }

            var isPlayer1 = game.Player1Name.Equals(playerName, StringComparison.OrdinalIgnoreCase);
            if (isPlayer1)
            {
                game.GameStatus = game.GameStatus == GameStatus.WaitingForPlayer1 ? game.GameStatus = GameStatus.Player1Turn : GameStatus.WaitingForPlayer2;
            }
            else
            {
                game.GameStatus = game.GameStatus == GameStatus.WaitingForPlayer2 ? game.GameStatus = GameStatus.Player1Turn : GameStatus.WaitingForPlayer1;
            }

            return new GameStatusResponse(game.GameStatus);
        }

        public GameStatusResponse? GetGameStatus(int gameId)
        {
            var game = _storage.GetGameById(gameId);
            if (game == null)
            {
                return null;
            }
            return new GameStatusResponse(game.GameStatus);
        }

        public UpdateImpactsResponse? UpdateImpacts(int gameId, UpdateImpactsRequest request)
        {
            var game = _storage.GetGameById(gameId);
            if (game == null)
            {
                return null;
            }
            var isPlayer1 = game.Player1Name.Equals(request.PlayerName, StringComparison.OrdinalIgnoreCase);
            var impacts = isPlayer1 ? game.PlayerImpacts : game.OpponentImpacts;
            var ships = isPlayer1 ? game.Player2Ships : game.Player1Ships;


            var impact = new Impact {
                Point = request.Point,
                Type = _gameLogic.DecideImpactType(request.Point, ships)
            };

            impacts.Add(impact);
            _gameLogic.CheckDestroyedShips(ships, impacts);

            var destroyedHullsCount = impacts.Count(x => x.Type == ImpactType.Destroyed);
            var totalHulls = ships.Sum(s => s.Hull.Count);

            if (destroyedHullsCount >= totalHulls)
            {
                game.GameStatus = isPlayer1 ? GameStatus.Player1Won : GameStatus.Player2Won;
            }
            else if (impact.Type != ImpactType.Ship && impact.Type != ImpactType.Destroyed)
            {
                game.GameStatus = isPlayer1 ? GameStatus.Player2Turn : GameStatus.Player1Turn;
            }

            return new UpdateImpactsResponse(game.GameStatus, impacts);
        }
    }
}
