using Battleships.ServiceDefaults;
using Battleships.ServiceDefaults.Infrastructure;
using Battleships.ServiceDefaults.Models;
using Battleships.Web.Services;
using System.Diagnostics;
using System.Drawing;

namespace Battleships.Web.Viewmodels
{
    public class GameViewModel
    {
        public Size Dimension { get; private set; }
        public List<Impact> PlayerImpacts { get; private set; } = [];
        public List<Impact> OpponentImpacts { get; private set; } = [];
        public List<Ship> PlayerShips { get; private set; }
        public List<Size> Dimensions { get; }
        public GameStatus Status { get; private set; } = GameStatus.Unknown;
        public bool IsPlayer1 { get => PlayerName == Player1Name; }

        public string Player1Name { get; set; } = "Player1";
        public string Player2Name { get; set; } = "Player2";

        public string PlayerName { get; private set; }

        public int Id { get; private set; }

        private readonly IApiClient _client;

        public GameViewModel(IApiClient client)
        {
            _client = client;
            Dimensions = [new Size(10, 10), new Size(10, 15), new Size(15, 10), new Size(15, 15), new Size(20, 20)];
            Dimension = Dimensions[0];
        }        

        public void SetDimension(Size dimension)
        {
            Dimension = dimension;
        }

        public async Task NewGameAsync()
        {
            var response = await _client.AddGameAsync(new AddGameRequest(Player1Name, Player2Name, Dimension));
            Id = response.GameId;
        }

        public async Task JoinGame(int id, string playerName)
        {
            Id = id;
            PlayerName = playerName;
            var game = await _client.JoinGame(new GetGameRequest(Id, PlayerName));
            if (game != null)
            {
                Dimension = game.Dimension;
                PlayerShips = [.. game.Ships];
                PlayerImpacts = [.. game.PlayerImpacts];
                OpponentImpacts = [.. game.OpponentImpacts];
                Status = game.GameStatus;
                Player1Name = game.Player1Name;
                Player2Name = game.Player2Name;
            }
            else
            {
                throw new InvalidOperationException("Failed to create or join the game.");
            }
        }

        public async Task ShufflePlayerShipsAsync()
        {
            var response = await _client.GetRandomShipPositionsAsync(new GetRandomShipPositionsRequest(Id, PlayerName));
            if (response != null && response.Ships != null && response.Ships.Count > 0)
            {
                PlayerShips = [.. response.Ships];
            }
            else
            {
                throw new InvalidOperationException("Failed to shuffle ships or no ships found.");
            }
        }

        public async Task PlayerReady()
        {
            var gameStatus = await _client.SendPlayerReady(Id, PlayerName);
            Status = gameStatus;
        }

        public async Task WaitForPlayerTurn()
        {
            while (true)
            {
                var gameStatus = await _client.GetGameStatus(Id);
                Status = gameStatus;

                if ((IsPlayer1 && Status == GameStatus.Player1Turn) || (!IsPlayer1 && Status == GameStatus.Player2Turn) || Status >= GameStatus.Player1Won)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }

        public string GetGameStatusMessage()
        {
            switch (Status)
            {
                case GameStatus.Unknown:
                    break;
                case GameStatus.Created:
                    break;
                case GameStatus.Planning:
                    return "Place your ships and start game.";
                case GameStatus.Player1Turn:
                    return IsPlayer1 ? "Your turn. Shoot" : "Player 1 turn. Wait";
                case GameStatus.Player2Turn:
                    return !IsPlayer1 ? "Your turn. Shoot" : "Player 2 turn. Wait";
                case GameStatus.Player1Won:
                    return "Player 1 have won!";
                case GameStatus.Player2Won:
                    return "Player 2 have won!";
                case GameStatus.WaitingForPlayer1:
                    return "Waiting for Player 1...";
                case GameStatus.WaitingForPlayer2:
                    return "Waiting for Player 2...";
                default:
                    break;
            }

            return string.Empty;
        }
        
        public async Task Shot(Point point)
        {
            if ((IsPlayer1 && Status != GameStatus.Player1Turn) || (!IsPlayer1 && Status != GameStatus.Player2Turn))
            {
                return;
            }

            var impact = OpponentImpacts.FirstOrDefault(x => x.Point == point);
            if (impact != null)
            {
                return;
            }

            var response = await _client.UpdateImpacts(Id, new UpdateImpactsRequest(Id, PlayerName, point));
            Status = response.GameStatus;
            OpponentImpacts = response.Impacts;
        }
    }
}
