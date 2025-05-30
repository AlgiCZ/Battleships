using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.Web.Viewmodels
{
    public class GameViewModel
    {
        public int Id { get; init; }
        public Size Dimension { get; init; }
        public List<Impact> PlayerImpacts { get; init; } = [];
        public List<Impact> OpponentImpacts { get; init; } = [];
        public List<Ship> Player1Ships { get; init; }
        public List<Ship> Player2Ships { get; init; }

        public GameStatus Status { get; private set; } = GameStatus.Unknown;

        private Random _random = new Random();

        public GameViewModel(Size dimension)
        {
            Id = 1; // Example ID, should be set appropriately
            Dimension = dimension;
            Player1Ships = ShipFactory.CreateDefaulShips();
            Player2Ships = ShipFactory.CreateDefaulShips();

            RandomizeShips(Player1Ships);
            RandomizeShips(Player2Ships);
        }

        public void NewGame()
        {
            Status = GameStatus.Planning;
        }

        public void ShufflePlayer1Ships()
        {
            ClearShipPositions(Player1Ships);
            RandomizeShips(Player1Ships);
        }   

        public void StartGame()
        {
            Status = GameStatus.Player1Turn + _random.Next(0, 1);
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
                    return "Your turn. Shoot";
                case GameStatus.Player2Turn:
                    return "Player 2 turn. Wait";
                case GameStatus.Player1Won:
                    return "Player 1 have won!";
                case GameStatus.Player2Won:
                    return "Player 2 have won!";
                default:
                    break;
            }

            return string.Empty;
        }

        private static void ClearShipPositions(List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                ship.Position = null;
            }
        }

        private void RandomizeShips(List<Ship> ships)
        {
            Random random = new Random();
            while (!ships.All(s => s.Position.HasValue))
            {
                var availablePositions = new List<Point>();
                for (int x = 0; x < Dimension.Width; x++)
                {
                    for (int y = 0; y < Dimension.Height; y++)
                    {
                        availablePositions.Add(new Point(x, y));
                    }
                }

                ClearShipPositions(ships);

                foreach (var ship in ships)
                {
                    var availablePositionsForShip = new List<Point>(availablePositions);
                    while (!ship.Position.HasValue)
                    {
                        Point pos = availablePositionsForShip[random.Next(0, availablePositionsForShip.Count - 1)];
                        if (IsValidPosition(ship, pos, ships))
                        {
                            ship.Position = pos;
                            RemoveAllAdjancedPoints(ship, availablePositions);
                            break;
                        }

                        availablePositionsForShip.Remove(pos);
                        if (availablePositionsForShip.Count == 0)
                            break;
                    }

                    if (!ship.Position.HasValue)
                        break;
                }
            }
        }

        private static void RemoveAllAdjancedPoints(Ship ship, List<Point> availablePositions)
        {
            if (!ship.Position.HasValue)
            {
                throw new InvalidOperationException("Ship position is not set.");
            }
            foreach (var hull in ship.Hull)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        availablePositions.Remove(new Point(ship.Position.Value.X + hull.X + dx, ship.Position.Value.Y + hull.Y + dy));
                    }
                }
            }
        }

        private bool IsValidPosition(Ship ship, Point position, List<Ship> allShips)
        {
            // Check if the ship can be placed at the given position
            foreach (var hull in ship.Hull)
            {
                var point = new Point(position.X + hull.X, position.Y + hull.Y);
                if (point.X < 0 || point.X >= Dimension.Width || point.Y < 0 || point.Y >= Dimension.Height)
                {
                    return false; // Out of bounds
                }

                if (allShips.Any(s => s.Position.HasValue && s.Hull.Any(h =>
                    IsPointAround(point, new Point(s.Position.Value.X + h.X, s.Position.Value.Y + h.Y))
                    )))
                {
                    return false; // Overlapping with another ship
                }


            }
            return true; // Valid position
        }

        private bool IsPointAround(Point point, Point point2)
        {
            int dx = Math.Abs(point.X - point2.X);
            int dy = Math.Abs(point.Y - point2.Y);

            return dx <= 1 && dy <= 1;
        }
    }
}
