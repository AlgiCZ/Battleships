using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.ApiService.Logic
{
    public class ShipFactory : IShipFactory
    {
        private static Ship CreateShip1() => new Ship { Hull = [new Point(0, 0)] };
        private static Ship CreateSmallShip2() => new Ship { Hull = [new Point(0, 0), new Point(1, 0)] };
        private static Ship CreateMediumShip3() => new Ship { Hull = [new Point(0, 0), new Point(1, 0), new Point(2, 0)] };
        private static Ship CreateLargeShipS() => new Ship { Hull = [new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2)] };
        private static Ship CreateLargeShipL() => new Ship { Hull = [new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(3, 1)] };

        public List<Ship> CreateDefaulShips() => [

            CreateShip1(), CreateShip1(),
            CreateSmallShip2(), CreateSmallShip2(),
            CreateMediumShip3(),
            CreateLargeShipS(),
            CreateLargeShipL()
        ];
    }

}
