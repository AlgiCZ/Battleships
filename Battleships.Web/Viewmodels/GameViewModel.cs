using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.Web.Viewmodels
{
    public class GameViewModel
    {
        public int Id { get; init; }
        public Size Dimension { get; init; }
        public Impact[] PlayerImpacts { get; init; }
        public Impact[] OpponentImpacts { get; init; }
        public Ship[] Player1Ships { get; init; }
        public Ship[] Player2Ships { get; init; }

        public GameViewModel(Size dimension)
        {
            Id = 1; // Example ID, should be set appropriately
            Dimension = dimension;
            Player1Ships = ShipFactory.CreateDefaulShips();
            Player2Ships = ShipFactory.CreateDefaulShips();

            RandomizeShips(Player1Ships);
            RandomizeShips(Player2Ships);
        }

        private void RandomizeShips(Ship[] ships)
        {
            Random random = new Random();

            while (!ships.All(s => s.Position.HasValue))// Ensure all ships have a valid position
            {
                foreach (var ship in ships)
                {
                    Point? pos;
                    int tryCount = 0;
                    do
                    {
                        pos = new Point(random.Next(0, Dimension.Width), random.Next(0, Dimension.Height));
                        if (tryCount++ > 1000) // Prevent infinite loop in case of failure and start over
                        {
                            pos = null;
                            break;
                        }
                    } while (!IsValidPosition(ship, pos.Value, ships));

                    if (!pos.HasValue)
                        break;

                    ship.Position = pos;
                }
            }
        }

        private bool IsValidPosition(Ship ship, Point position, Ship[] allShips)
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
