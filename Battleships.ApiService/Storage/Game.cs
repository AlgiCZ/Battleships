using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.ApiService.Storage
{
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
