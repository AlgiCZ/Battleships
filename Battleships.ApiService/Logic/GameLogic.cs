using Battleships.ApiService.Storage;
using Battleships.ServiceDefaults;
using Battleships.ServiceDefaults.Models;
using System;
using System.Drawing;

namespace Battleships.ApiService.Logic
{
    public class GameLogic : IGameLogic
    {
        private Random _random;

        public GameLogic()
        {
            _random = new Random();
        }

        private static void ClearShipPositions(List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                ship.Position = null;
            }
        }

        public ImpactType DecideImpactType(Point point, List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                foreach (var hull in ship.Hull)
                {
                    var hullPoint = new Point(ship.Position.Value.X + hull.X, ship.Position.Value.Y + hull.Y);
                    if (point == hullPoint)
                    {
                        return ImpactType.Ship;
                    }
                }
            }

            return ImpactType.Water;
        }

        public void CheckDestroyedShips(List<Ship> ships, List<Impact> impacts)
        {
            foreach (var ship in ships)
            {
                if (ship.Position.HasValue && ship.Hull.All(h => impacts.Any(i => i.Point == new Point(ship.Position.Value.X + h.X, ship.Position.Value.Y + h.Y) && i.Type == ImpactType.Ship)))
                {
                    foreach (var hull in ship.Hull)
                    {
                        var point = new Point(ship.Position.Value.X + hull.X, ship.Position.Value.Y + hull.Y);
                        impacts.First(x => x.Point == point).Type = ImpactType.Destroyed;
                    }
                }
            }
        }

        public void RandomizeShips(List<Ship> ships, Size dimension)
        {
            //lets place it from the biggest ship
            ships.Sort((x,y) => y.Hull.Count.CompareTo(x.Hull.Count));
            ClearShipPositions(ships);
            while (!ships.All(s => s.Position.HasValue))
            {
                var availablePositions = new List<Point>();
                for (int x = 0; x < dimension.Width; x++)
                {
                    for (int y = 0; y < dimension.Height; y++)
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
                        Point pos = availablePositionsForShip[_random.Next(0, availablePositionsForShip.Count - 1)];
                        if (IsValidPosition(ship, pos, ships, dimension))
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

        private bool IsValidPosition(Ship ship, Point position, List<Ship> allShips, Size dimension)
        {
            // Check if the ship can be placed at the given position
            foreach (var hull in ship.Hull)
            {
                var point = new Point(position.X + hull.X, position.Y + hull.Y);
                if (point.X < 0 || point.X >= dimension.Width || point.Y < 0 || point.Y >= dimension.Height)
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
