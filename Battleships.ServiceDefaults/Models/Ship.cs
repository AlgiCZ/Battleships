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
}
