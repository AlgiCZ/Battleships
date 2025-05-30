using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ServiceDefaults.Models
{
    public class Ship
    {
        public Point? Position { get; set; }
        public byte Rotation { get; set; } = 0;
        public required IReadOnlyList<Point> Hull { get; set; }
    }

    public class ShipFactory
    {
        public static Ship CreateShip1() => new Ship { Hull = [new Point(0, 0)] };
        public static Ship CreateSmallShip2() => new Ship { Hull = [ new Point(0, 0), new Point(1, 0) ] };
        public static Ship CreateMediumShip3() => new Ship { Hull = [ new Point(0, 0), new Point(1, 0), new Point(2, 0) ] };
        public static Ship CreateLargeShipS() => new Ship { Hull = [ new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2) ] };
        public static Ship CreateLargeShipL() => new Ship { Hull = [ new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(3, 1) ] };

        public static Ship[] CreateDefaulShips() => [

            CreateShip1(), CreateShip1(),
            CreateSmallShip2(), CreateSmallShip2(),
            CreateMediumShip3(),
            CreateLargeShipS(),
            CreateLargeShipL()
        ];
    }

    public class Game
    {
        public int Id { get; set; }
        public GameStatus Status { get; set; }

        public Size BoardDimension { get; set; } = new Size(10, 10);
        public IReadOnlyList<Impact> Player1Impacts { get; set; } = [];
        public IReadOnlyList<Impact> Player2Impacts { get; set; } = [];
    }

    public enum GameStatus
    {
        Unknown,
        Created,
        Planning,
        Player1Turn,
        Player2Turn,
        Player1Won,
        Player2Won
    }

}
